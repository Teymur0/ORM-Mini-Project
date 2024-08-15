using ORM_Mini_Project.Models.Common;

namespace ORM_Mini_Project.Models
{
    internal class Payment : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
