namespace WebApi_test.Testing
{
    public interface IValidateWireTransfer
    {
        OperationResult Validate(Account origin, Account destination, decimal amount);
    }
}
