namespace EPAY.ETC.Core.API.Core.Entities
{
    public class BaseEntity<TId>
    {
        public TId? Id { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
