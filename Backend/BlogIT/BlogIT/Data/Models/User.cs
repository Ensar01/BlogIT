namespace BlogIT.Data.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDate { get; set; }
        public DateOnly RegistrationDate { get; set; }

    }
}
