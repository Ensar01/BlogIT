using System.ComponentModel.DataAnnotations;

namespace BlogIT.DataTransferObjects
{
    public record UserRegisterDto(
    [Required] string Name,
    [Required] string LastName,
    [Required] DateOnly BirthDate,
    [Required] string UserName,
    [Required] string Email,
    [Required] string Password,
    [Required] string PhoneNumber);
}
