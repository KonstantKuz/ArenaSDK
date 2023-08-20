
namespace Response.Fail
{
    public class InvalidFormFail : IFailResponse
    {
        private readonly string _description;
        public InvalidFormFail(string description) => _description = description;

        public string Message => _description;
    }
}