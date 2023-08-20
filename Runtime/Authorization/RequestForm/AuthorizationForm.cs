using System;
using Response;
using Util;

namespace Authorization.RequestForm
{
    [Serializable]
    public class AuthorizationForm : IValidatedForm
    {
        public string login;
        public string password;
        
        public bool IsValid(out string description)
        {
            return login.Length.IsInRange("Login length", out description, 1)
                   && password.Length.IsInRange("Password length", out description, 8, 100);
        }
    }
}