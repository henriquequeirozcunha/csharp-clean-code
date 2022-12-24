namespace Domain.Entities
{
    public class BaseDomainEntity
    {
        public int Id { get; set; }
        public DateTime DataCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
    }
}