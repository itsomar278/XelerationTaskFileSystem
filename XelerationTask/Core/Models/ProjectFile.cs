using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XelerationTask.Core.Models
{
    [Table("Files")]
    public class ProjectFile : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MinLength(1)]
        [MaxLength(255)]
        [Required]
        public string OriginalName { get; set; }

        [MinLength(1)]
        [MaxLength(255)]
        [Required]

        public string Name { get; set; }

        public int ParentFolderId { get; set; }

        public ProjectFolder ParentFolder { get; set; }


        public ProjectFile(string name){
           
            Name = name;

        }

    }
}
