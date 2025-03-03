using System.ComponentModel.DataAnnotations;

namespace BlogIT.DataTransferObjects
{
    public record UserRegisterDto(
    [Required] string Name,
    [Required] string LastName,
    [Required] string UserName,
    [Required][EmailAddress] string Email,
    [Required] string Password
    );
}
