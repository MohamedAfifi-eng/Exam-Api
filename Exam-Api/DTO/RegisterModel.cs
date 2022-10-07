using System.ComponentModel.DataAnnotations;

namespace Exam_Api.DTO
{
    public class RegisterModel
    {
        [Required]

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]

        public string FullName { get; set; }
    }
}