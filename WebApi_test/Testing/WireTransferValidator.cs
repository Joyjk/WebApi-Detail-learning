namespace WebApi_test.Testing
{
    public class WireTransferValidator : IValidateWireTransfer
    {
        public OperationResult Validate(Account origin, Account destination, decimal amount)
        {
            if (amount>origin.Funds)
            {
                return new OperationResult(false, "The account does not have enough funds");

            }
            return new OperationResult(true);
        }
    }
}
