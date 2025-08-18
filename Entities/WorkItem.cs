using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBoards.Entities
{
    public class WorkItem
    {
        // Workitem properties
        public int Id { get; set; }

        // configure relation n:1 with WorkItemState entity
        public WorkItemState State { get; set; }
        public int StateId { get; set; }

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
        public List<Comment> Comments { get; set; } = [];

        // configure relation n:1 with User entity
        public User Author { get; set; }
        public Guid AuthorId { get; set; }

        public List<Tag> Tags { get; set; }

        // configure relation n:1 with WorkItemTag entity
        //for older .NET version
        // public List<WorkItemTag> WorkItemTags { get; set; } = new List<WorkItemTag>();
    }
}
