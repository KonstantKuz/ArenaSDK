namespace Response.Fail
{
    public class UnexpectedFail : IFailResponse
    {
        public string Message => "Unexpected request fail.";
    }
}