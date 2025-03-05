
namespace MyBoards.Entities
{
    public class User
    {
        // User properties
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        // Reference do Address entity
        public Address Address { get; set; }
    }
}
