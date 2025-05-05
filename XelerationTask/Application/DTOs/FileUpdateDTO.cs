using System.ComponentModel.DataAnnotations;

namespace XelerationTask.Application.DTOs
{
    public class FileUpdateDTO
    {

        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public int ParentFolderId { get; set; }
    }
}
