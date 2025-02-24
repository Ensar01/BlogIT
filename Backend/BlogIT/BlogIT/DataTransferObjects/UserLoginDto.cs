using System.ComponentModel.DataAnnotations;

namespace BlogIT.DataTransferObjects
{
    public record UserLoginDto
    {
        [Required]
        public string Username { get; init; }
        
        [Required]
        public string Password { get; init; }
    }
}
