using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace XelerationTask.Application.DTOs
{
    public class UserCreateDTO
    {
        [Required]
        [EmailAddress]
        public String Email { get; set; }

        [MinLength(8)]
        public String Password { get; set; }

    }
}
