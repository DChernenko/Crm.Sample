namespace Crm.Sample.Domain.Exceptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException(string entityName, int id)
            : base($"{entityName} with id '{id}' was not found.")
        {
        }
    }
}
