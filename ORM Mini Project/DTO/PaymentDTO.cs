namespace ORM_Mini_Project.DTO
{
    internal class PaymentDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
