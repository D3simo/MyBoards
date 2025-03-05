using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBoards.Entities
{
    public class WorkItem
    {
        // Workitem properties
        public int Id { get; set; }
        public string State { get; set; }
        public string Area { get; set; }
        public string IterationPath { get; set; }
        public int Priority { get; set; }

        // Epic 
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //Issue
        public decimal Effort { get; set; }

        //Task
        public string Activity { get; set; }
        public decimal RemainingWork { get; set; }

        public string Type { get; set; }

        // configure relation 1:n with Comment entity // add default blank list
        public List<Comment> Comments { get; set; } = new List<Comment>();

        // configure relation n:1 with User entity
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
