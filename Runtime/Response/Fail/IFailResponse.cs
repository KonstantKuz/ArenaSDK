namespace Response.Fail
{
    public interface IFailResponse : IResponse
    {
        public string Message { get; }
    }
}