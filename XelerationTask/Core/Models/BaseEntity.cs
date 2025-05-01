namespace XelerationTask.Core.Models
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 
        public int? CreatedBy { get; set; } 

        public int? UpdatedBy { get; set; } 

        public bool IsDeleted { get; set; } = false;
        
        public DateTime DeletedAt {  get; set; }

        public int? DeletedBy { get; set; } 
    }
}
