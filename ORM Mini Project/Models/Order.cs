using ORM_Mini_Project.Models.Common;
using ORM_Mini_Project.Models.Enums;

namespace ORM_Mini_Project.Models
{
    internal class Order : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        public List<OrderProduct> OrderProducts { get; set; }
    }
}
