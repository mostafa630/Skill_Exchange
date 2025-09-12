namespace Skill_Exchange.Application.DTOs.User
{
    public class CreateUserDTO//:ICreate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        //public DateOnly DateOfBirth { get; set; }
    }
}
