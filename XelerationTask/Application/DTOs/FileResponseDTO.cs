namespace XelerationTask.Application.DTOs
{
    public class FileResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IdNameDto ParentFolder { get; set; }

        public string OriginalName { get; set; }

        public int? CreatedBy { get; set; }

    }
}
