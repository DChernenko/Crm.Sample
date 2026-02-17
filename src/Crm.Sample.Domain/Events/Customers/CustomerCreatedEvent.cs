namespace Crm.Sample.Domain.Events.Customers
{
    public class CustomerCreatedEvent
    {
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.Now;
    }
}
