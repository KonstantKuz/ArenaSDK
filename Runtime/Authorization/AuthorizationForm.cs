using System;
using Response;

namespace Authorization
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