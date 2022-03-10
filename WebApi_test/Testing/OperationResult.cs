namespace WebApi_test.Testing
{
    public class OperationResult
    {
        public OperationResult(bool isSuccessfull, string errorMessage = null)
        {
            IsSuccessfull = isSuccessfull;
            ErrorMessage = errorMessage;
        }

        public bool IsSuccessfull { get; }
        public string ErrorMessage { get; }
    }
}
