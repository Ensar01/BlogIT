using System.ComponentModel.DataAnnotations;

namespace BlogIT.DataTransferObjects
{
    public record UserRegisterDto(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string UserName,
    [Required][EmailAddress] string Email,
    [Required] string Password
    );
}
