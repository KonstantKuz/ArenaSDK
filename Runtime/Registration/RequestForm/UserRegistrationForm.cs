using System;
using Response;

namespace Registration.RequestForm
{
    [Serializable]
    public class UserRegistrationForm : IValidatedForm
    {
        public string email;
        public string username;
        public string password;

        public bool IsValid(out string description)
        {
            return password.Length.IsInRange("Password length", out description, 8)
                   && username.Length.IsInRange("Username", out description, 3, 20)
                   && username.IsCharactersValid("Username", out description);
        }
    }
}