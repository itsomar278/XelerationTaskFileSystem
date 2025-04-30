using XelerationTask.Core.Models;

namespace XelerationTask.Application.DTOs
{
    public class FolderResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IdNameDto? ParentFolder { get; set; }

        public ICollection<IdNameDto> ChildFolders { get; set; }

        public ICollection<IdNameDto> ChildFiles { get; set; }

        public int? CreatedBy { get; set; }
    }

}
