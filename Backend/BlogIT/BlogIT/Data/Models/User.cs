using Microsoft.AspNetCore.Identity;

namespace BlogIT.Data.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        public DateOnly RegistrationDate { get; set; }

    }
}
