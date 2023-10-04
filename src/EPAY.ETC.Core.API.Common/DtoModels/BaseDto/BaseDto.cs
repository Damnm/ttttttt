namespace EPAY.ETC.Core.API.Core.DtoModels.BaseDto
{
    public class BaseDto<TId>
    {
        public TId? Id { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
