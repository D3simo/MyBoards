
namespace MyBoards.Entities
{
    public class User
    {
        // User properties
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        // Reference do Address entity
        public virtual Address Address { get; set; }

        // configure relation 1:n with WorkItem entity // with blank list
        public virtual List<WorkItem> WorkItem { get; set; } = [];

        // configure relation 1:n with Comment entity // with blank list
        public virtual List<Comment> Comments { get; set; } = [];
    }
}
