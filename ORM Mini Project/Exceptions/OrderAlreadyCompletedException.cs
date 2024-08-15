namespace ORM_Mini_Project.Exceptions
{
    public class OrderAlreadyCompletedException : Exception
    {
        public OrderAlreadyCompletedException(string message) : base(message)
        {
        }
    }
}
