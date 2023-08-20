namespace Response.Fail
{
    public class UserInfoLoadFail : IFailResponse
    {
        public string Message => "Could not load user info. Try sign in again. ";
    }
}