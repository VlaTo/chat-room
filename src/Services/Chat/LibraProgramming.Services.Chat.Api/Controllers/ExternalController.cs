using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core;
using LibraProgramming.ChatRoom.Services.Chat.Api.Extensions;
using LibraProgramming.ChatRoom.Services.Chat.Api.Models;
using LibraProgramming.ChatRoom.Services.Chat.Persistence.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public sealed class ExternalController : Controller
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly IClientStore clientStore;
        private readonly IEventService eventService;
        private readonly SignInManager<Customer> signInManager;
        private readonly UserManager<Customer> customerManager;
        private readonly ILogger<ExternalController> logger;

        public ExternalController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService eventService,
            SignInManager<Customer> signInManager,
            UserManager<Customer> customerManager,
            ILogger<ExternalController> logger)
        {
            this.interaction = interaction;
            this.clientStore = clientStore;
            this.eventService = eventService;
            this.signInManager = signInManager;
            this.customerManager = customerManager;
            this.logger = logger;
        }

        // GET: /External/challenge/
        [HttpGet("challenge")]
        public async Task<IActionResult> Challenge(string scheme, string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "~/";
            }

            if (false == Url.IsLocalUrl(returnUrl) && false == interaction.IsValidReturnUrl(returnUrl))
            {
                logger.LogError($"[ExternalController.Challenge] Parameter '{nameof(returnUrl)}' is invalid");
                throw new Exception();
            }

            if (scheme == AccountOptions.WindowsAuthenticationScheme)
            {
                return await ProcessWindowsLoginAsync(returnUrl);
            }

            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback)),
                Items =
                {
                    {"returnUrl", returnUrl},
                    {"scheme", scheme}
                }
            };

            return Challenge(properties, scheme);
        }

        // GET /External/Callback
        [HttpGet("callback")]
        public async Task<IActionResult> Callback()
        {
            var result = await HttpContext.AuthenticateAsync(
                IdentityServerConstants.ExternalCookieAuthenticationScheme
            );

            if (false == result?.Succeeded)
            {
                logger.LogError($"[ExternalController.Callback] {result.Failure}");
                throw result.Failure ?? new Exception();
            }

            var (user, provider, providerUserId, claims) = await FindCustomerFromExternalProviderAsync(result);

            if (null == user)
            {
                //user = await AutoProvisionCustomerAsync(provider, providerUserId, claims);
                return await AutoProvisionCustomerAsync(provider, providerUserId, claims);
            }

            var localClaims = new List<Claim>();
            var localProperties = new AuthenticationProperties();

            ProcessLoginCallbackForOidc(result, localClaims, localProperties);
            //ProcessLoginCallbackForWsFed(result, localClaims, localProperties);
            //ProcessLoginCallbackForSaml2p(result, localClaims, localProperties);

            // issue authentication cookie for user
            await eventService.RaiseAsync(new UserLoginSuccessEvent(
                provider,
                providerUserId,
                user.Id.ToString(),
                user.UserName)
            );

            var signin = await signInManager.ExternalLoginSignInAsync(provider, providerUserId, true);

            if (false == signin.Succeeded)
            {
                return BadRequest();
            }

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // retrieve return URL
            var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";
            // check if external login is in the context of an OIDC request
            var context = await interaction.GetAuthorizationContextAsync(returnUrl);

            if (null != context)
            {
                if (await clientStore.IsPkceClientAsync(context.ClientId))
                {
                    // if the client is PKCE then we assume it's native, so this change in how to
                    // return the response is for better UX for the end user.
                    return View("Redirect", new RedirectModel { RedirectUrl = returnUrl });
                }
            }

            return Redirect(returnUrl);
        }

        private async Task<(Customer customer, string provider, string providerUserId, IList<Claim> claims)>
            FindCustomerFromExternalProviderAsync(AuthenticateResult result)
        {
            var principal = result.Principal;
            var userIdClaim = principal.FindFirst(JwtClaimTypes.Subject) ??
                              principal.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");
            var claims = principal.Claims.ToList();

            claims.Remove(userIdClaim);

            var provider = result.Properties.Items["scheme"];
            var providerUserId = userIdClaim.Value;

            //var user = customerManager.FindByExternalProvider(scheme, providerUserId);
            var user = await customerManager.FindByLoginAsync(provider, providerUserId);

            return (user, provider, providerUserId, claims);
        }

        private async Task<IActionResult> AutoProvisionCustomerAsync(string provider, string providerUserId, IList<Claim> claims)
        {
            var email = GetEmailFromClaims(claims);
            var name = GetContactNameFromClaims(claims);

            await Task.CompletedTask;

            return RedirectToAction("Signup", "Account", new SignUpModel
            {
                Email = email,
                UserName = GetUserNameFromClaims(claims) ?? name
            });

            /*

            if (null == email)
            {
                return null;
            }

            var name = GetContactNameFromClaims(claims);
            var customer = new Customer
            {
                UserName = GetUserNameFromClaims(claims) ?? name,
                Email = email,
                ContactName = name
            };

            var result = await customerManager.CreateAsync(customer);

            if (result.Succeeded)
            {
                customer = await customerManager.FindByEmailAsync(email);
            }

            if (customerManager.SupportsUserLogin)
            {
                var principal = await signInManager.CreateUserPrincipalAsync(customer);
                //var id = new ClaimsIdentity(provider);

                //id.AddClaim(new Claim(JwtClaimTypes.Subject, customer.Email));
                //id.AddClaim(new Claim(JwtClaimTypes.Name, customer.UserName));

                result = await customerManager.AddLoginAsync(customer,
                    //new ExternalLoginInfo(new ClaimsPrincipal(id), provider, providerUserId, customer.UserName)
                    new ExternalLoginInfo(principal, provider, providerUserId, customer.UserName)
                );

                if (result.Succeeded)
                {
                    result = await customerManager.AddToRoleAsync(customer, CustomerRoleNames.Shopper);

                    if (result.Succeeded)
                    {
                        return customer;
                    }
                }
            }

            return null;
            */
        }

        private async Task<IActionResult> ProcessWindowsLoginAsync(string returnUrl)
        {
            // see if windows auth has already been requested and succeeded
            var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationScheme);

            if (result?.Principal is WindowsPrincipal wp)
            {
                // we will issue the external cookie and then redirect the
                // user back to the external callback, in essence, treating windows
                // auth the same as any other external authentication mechanism
                var props = new AuthenticationProperties()
                {
                    RedirectUri = Url.Action("Callback"),
                    Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", AccountOptions.WindowsAuthenticationScheme }
                    }
                };

                var id = new ClaimsIdentity(AccountOptions.WindowsAuthenticationScheme);

                id.AddClaim(new Claim(JwtClaimTypes.Subject, wp.Identity.Name));
                id.AddClaim(new Claim(JwtClaimTypes.Name, wp.Identity.Name));

                // add the groups as claims -- be careful if the number of groups is too large
                if (AccountOptions.IncludeWindowsGroups)
                {
                    var wi = wp.Identity as WindowsIdentity;
                    var groups = wi.Groups.Translate(typeof(NTAccount));
                    var roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));

                    id.AddClaims(roles);
                }

                await HttpContext.SignInAsync(
                    IdentityServerConstants.ExternalCookieAuthenticationScheme,
                    new ClaimsPrincipal(id),
                    props);

                return Redirect(props.RedirectUri);
            }

            // trigger windows auth
            // since windows auth don't support the redirect uri,
            // this URL is re-triggered when we call challenge

            return Challenge(AccountOptions.WindowsAuthenticationScheme);
        }

        private static void ProcessLoginCallbackForOidc(
            AuthenticateResult result,
            ICollection<Claim> claims,
            AuthenticationProperties properties)
        {
            var sid = result.Principal.Claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.SessionId);

            if (null != sid)
            {
                claims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            const string idTokenName = "id_token";

            var idToken = result.Properties.GetTokenValue(idTokenName);

            if (null != idToken)
            {
                properties.StoreTokens(new[]
                {
                    new AuthenticationToken
                    {
                        Name = idTokenName,
                        Value = idToken
                    }
                });
            }
        }

        private static string GetEmailFromClaims(IList<Claim> claims)
        {
            var email = claims.FirstOrDefault(claim => ClaimTypes.Email == claim.Type);
            return email?.Value;
        }

        private static string GetUserNameFromClaims(IList<Claim> claims)
        {
            var username = claims.FirstOrDefault(claim => ClaimTypes.GivenName == claim.Type)
                           ?? claims.FirstOrDefault(claim => ClaimTypes.NameIdentifier == claim.Type);
            return username?.Value;
        }

        private static string GetContactNameFromClaims(IList<Claim> claims)
        {
            var name = claims.FirstOrDefault(claim => ClaimTypes.Name == claim.Type)
                       ?? claims.FirstOrDefault(claim => ClaimTypes.Actor == claim.Type);
            return name?.Value;
        }
    }
}