namespace webAPI.dal.Entities
{
    public class BaseEntity
    {
        public DateTime? CreationTime { get; set; }
        public bool IsRowActive { get; set; }
    }
}
