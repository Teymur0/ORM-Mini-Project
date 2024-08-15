using ORM_Mini_Project.Models.Common;

namespace ORM_Mini_Project.Models
{
    internal class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
