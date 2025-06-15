namespace Nokas.CashBagManagement.WebAPI.Helpers
{
    public class DuplicateBagNumberException : Exception
    {
        public DuplicateBagNumberException(string bagNumber)
            : base($"Bag number '{bagNumber}' already exists.") { }
    }
}
