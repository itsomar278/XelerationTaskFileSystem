namespace XelerationTask.Core.Models
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get;} = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public int DeletedBy { get; set; }
    }
}
