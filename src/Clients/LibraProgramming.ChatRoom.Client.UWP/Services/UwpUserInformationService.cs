using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using LibraProgramming.ChatRoom.Client.Services;

namespace LibraProgramming.ChatRoom.Client.UWP.Services
{
    public class UwpUserInformationService : IUserInformation
    {
        public async Task<string> GetUserNameAsync()
        {
            var username = String.Empty;
            var users = await User.FindAllAsync();
            var localUser = users.FirstOrDefault(p =>
                p.AuthenticationStatus == UserAuthenticationStatus.LocallyAuthenticated &&
                p.Type == UserType.LocalUser);

            if (null != localUser)
            {
                Debug.WriteLine($"account: {await localUser.GetPropertyAsync(KnownUserProperties.AccountName)}");
                Debug.WriteLine($"display: {await localUser.GetPropertyAsync(KnownUserProperties.DisplayName)}");
                Debug.WriteLine($"first: {await localUser.GetPropertyAsync(KnownUserProperties.FirstName)}");
                Debug.WriteLine($"principal: {await localUser.GetPropertyAsync(KnownUserProperties.PrincipalName)}");

                username = (string) await localUser.GetPropertyAsync(KnownUserProperties.AccountName);

                if (String.IsNullOrEmpty(username))
                {
                    username = (string) await localUser.GetPropertyAsync(KnownUserProperties.DisplayName);
                }
            }

            return username ?? String.Empty;
        }
    }
}