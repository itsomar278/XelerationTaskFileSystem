using System.ComponentModel.DataAnnotations;

namespace XelerationTask.Application.DTOs
{
    public class FolderUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Name { get; set; }

        public int? ParentFolderId { get; set; }
    }
}
