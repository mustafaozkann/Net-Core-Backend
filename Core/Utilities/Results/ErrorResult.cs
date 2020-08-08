namespace Core.Utilities.Results
{
    public class ErrorResult : Result
    {
        public ErrorResult(string message):base(success:false, message)
        {

        }

        public ErrorResult() : base(success:false)
        {

        }
    }
}
