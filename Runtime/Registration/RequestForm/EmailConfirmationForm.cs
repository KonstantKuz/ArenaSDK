using System;

namespace Registration.RequestForm
{
    [Serializable]
    public class EmailConfirmationForm
    {
        public string email;
        public string code;
    }
}