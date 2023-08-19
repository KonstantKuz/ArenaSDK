
namespace Response
{
    public interface IValidatedForm
    {
        public bool IsValid(out string description);
    }
}