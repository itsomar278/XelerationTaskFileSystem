using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XelerationTask.Core.Models
{  
    [Table("Folders")]
    public class ProjectFolder : BaseEntity
    {
      
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MinLength(1)]
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }

        public int? ParentFolderId { get; set; }

        public ProjectFolder? ParentFolder { get; set; }

        public ICollection<ProjectFile> Files { get; set; }

        public ProjectFolder() {
        }

    }
}
