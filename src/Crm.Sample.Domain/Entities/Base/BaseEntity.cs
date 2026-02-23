namespace Crm.Sample.Domain.Entities.Base
{
    public class BaseEntity : BaseEntity<int> { }

    public abstract class BaseEntity<TId>
    {
        public TId Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreatorId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifierId { get; set; }
    }
}
