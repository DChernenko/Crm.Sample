using Crm.Sample.Domain.Entities.Base;

namespace Crm.Sample.Domain.Entities.Customers
{
    public class Customer : BaseEntity
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}
