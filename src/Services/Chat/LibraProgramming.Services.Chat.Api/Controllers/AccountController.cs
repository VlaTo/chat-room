using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core;
using LibraProgramming.ChatRoom.Services.Chat.Api.Extensions;
using LibraProgramming.ChatRoom.Services.Chat.Api.Models;
using LibraProgramming.ChatRoom.Services.Chat.Persistence.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public sealed class AccountController : Controller
    {
        private readonly IMediator mediator;
        private readonly IIdentityServerInteractionService interactions;
        private readonly IClientStore clientStore;
        private readonly IAuthenticationSchemeProvider schemeProvider;
        private readonly UserManager<Customer> customerManager;
        //private readonly ICaptcha captcha;
        private readonly IWebHostEnvironment environment;
        private readonly IEventService eventService;
        //private readonly IStringLocalizer<AccountController> localizer;
        private readonly ILogger<AccountController> logger;

        public AccountController(
            IMediator mediator,
            IIdentityServerInteractionService interactions,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            UserManager<Customer> customerManager,
            //ICaptcha captcha,
            IWebHostEnvironment environment,
            IEventService eventService,
            //IStringLocalizer<AccountController> localizer,
            ILogger<AccountController> logger)
        {
            this.mediator = mediator;
            this.interactions = interactions;
            this.clientStore = clientStore;
            this.schemeProvider = schemeProvider;
            this.customerManager = customerManager;
            //this.captcha = captcha;
            this.environment = environment;
            this.eventService = eventService;
            //this.localizer = localizer;
            this.logger = logger;
        }

        // GET /account/signin
        [HttpGet("signin")]
        public async Task<IActionResult> Signin([FromQuery] string returnUrl)
        {
            var model = await CreateSigninModelAsync(returnUrl);

            if (model.IsExternalLoginOnly)
            {
                return RedirectToAction("Challenge", "External", new
                {
                    scheme = model.ExternalAuthenticationScheme,
                    returnUrl
                });
            }

            return View(model);
        }

        // POST /account/signin
        [HttpPost("signin")]
        [Consumes("application/x-www-form-urlencoded")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signin([FromForm] SignInInputModel model, [FromForm] string button)
        {
            var context = await interactions.GetAuthorizationContextAsync(model.ReturnUrl);

            if ("signin" != button)
            {
                logger.LogDebug("Signin executing");

                if (null != context)
                {
                    await interactions.GrantConsentAsync(context, ConsentResponse.Denied);

                    if (await clientStore.IsPkceClientAsync(context.ClientId))
                    {
                        return View("Redirect", new RedirectModel { RedirectUrl = model.ReturnUrl });
                    }

                    return Redirect(model.ReturnUrl);
                }

                return Redirect("~/");
            }

            if (ModelState.IsValid)
            {
                var result = await mediator.Send(new GetCustomerQuery(model.Email, model.Password), HttpContext.RequestAborted);

                if (result.IsFailed)
                {
                    return View();
                }

                if (result.IsNotAllowed)
                {
                    return View();
                }

                if (result.IsLockedOut)
                {
                    return View();
                }

                if (result.RequiresTwoFactor)
                {
                    return View();
                }

                if (result.IsSucceeded)
                {
                    var customer = result.Customer;

                    await eventService.RaiseAsync(new UserLoginSuccessEvent(
                        IdentityServerConstants.LocalIdentityProvider,
                        customer.NormalizedUserName,
                        customer.UserName,
                        customer.ContactName)
                    );

                    await mediator.Send(new SignInCommand(customer, model.RememberMe), HttpContext.RequestAborted);

                    if (null != context)
                    {
                        if (await clientStore.IsPkceClientAsync(context.ClientId))
                        {
                            return View("Redirect", new RedirectModel { RedirectUrl = model.ReturnUrl });
                        }

                        return Redirect(model.ReturnUrl);
                    }

                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    if (String.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }

                    var uri = new Uri(model.ReturnUrl);

                    if (uri.IsAbsoluteUri)
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    throw new Exception("Invalid redirect url");
                }
            }

            var invalidCredentials = localizer.InvalidCredentials(context?.UiLocales);
            await eventService.RaiseAsync(new UserLoginFailureEvent(model.Email, invalidCredentials));

            ModelState.AddModelError(String.Empty, "Invalid credentials");

            return View(await CreateSigninModelAsync(model));
        }

        // GET /account/error
        [HttpGet("error")]
        public async Task<IActionResult> Error([FromQuery] string errorId)
        {
            // retrieve error details from identityserver
            var error = await interactions.GetErrorContextAsync(errorId);

            if (null == error)
            {
                return View(new ErrorModel(null));
            }

            if (false == environment.IsDevelopment())
            {
                // only show in development
                error.ErrorDescription = null;
            }

            if (OidcConstants.ResponseModes.Fragment == error.ResponseMode)
            {
                return View(new ErrorModel(error));
            }

            return View(new ErrorModel(null));
        }

        private async Task<SignInViewModel> CreateSigninModelAsync(string returnUrl)
        {
            var context = await interactions.GetAuthorizationContextAsync(returnUrl);

            if (null != context?.IdP)
            {
                return new SignInViewModel
                {
                    EnableLocalLogin = false,
                    ReturnUrl = returnUrl,
                    Email = context.LoginHint,
                    ExternalProviders =
                    {
                        new ExternalProvider
                        {
                            AuthenticationScheme = context.IdP
                        }
                    }
                };
            }

            var schemes = await schemeProvider.GetAllSchemesAsync();
            var comparer = StringComparer.OrdinalIgnoreCase;
            bool IsWindowsAuthentication(AuthenticationScheme scheme) =>
                null != scheme.DisplayName || comparer.Equals(scheme.Name, AccountOptions.WindowsAuthenticationScheme);
            var canSigninLocal = true;
            var providers = schemes
                .Where(IsWindowsAuthentication)
                .Select(scheme => new ExternalProvider
                {
                    DisplayName = scheme.DisplayName,
                    AuthenticationScheme = scheme.Name
                })
                .ToArray();

            if (null != context?.ClientId)
            {
                var client = await clientStore.FindEnabledClientByIdAsync(context.ClientId);

                if (null != client)
                {
                    canSigninLocal = client.EnableLocalLogin;

                    if (null != client.IdentityProviderRestrictions && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers
                            .Where(provider =>
                                client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)
                            )
                            .ToArray();
                    }
                }
            }

            //captcha.Create(HttpContext);

            return new SignInViewModel
            {
                AllowRememberMe = AccountOptions.AllowRememberMe,
                EnableLocalLogin = canSigninLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Email = context?.LoginHint,
                ExternalProviders = providers
            };
        }

        private async Task<SignInViewModel> CreateSigninModelAsync(SignInInputModel signIn)
        {
            var model = await CreateSigninModelAsync(signIn.ReturnUrl);

            model.Email = signIn.Email;
            model.RememberMe = signIn.RememberMe;

            return model;
        }
    }
}