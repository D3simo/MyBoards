using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBoards.Entities
{
    public class Epic : WorkItem
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class Issue : WorkItem
    {
        public decimal Efford { get; set; }
    }

    public class Task : WorkItem
    {
        public string Activity { get; set; }
        public decimal RemainingWork { get; set; }
    }

    public abstract class WorkItem
    {
        // Workitem properties
        public int Id { get; set; }

        // configure relation n:1 with WorkItemState entity
        public virtual WorkItemState State { get; set; }
        public int StateId { get; set; }

        public string Area { get; set; }
        public string IterationPath { get; set; }
        public int Priority { get; set; }

        // configure relation 1:n with Comment entity // add default blank list
        public virtual List<Comment> Comments { get; set; } = [];

        // configure relation n:1 with User entity
        public virtual User Author { get; set; }
        public Guid AuthorId { get; set; }

        public virtual List<Tag> Tags { get; set; }

        // configure relation n:1 with WorkItemTag entity
        //for older .NET version
        // public List<WorkItemTag> WorkItemTags { get; set; } = new List<WorkItemTag>();
    }
}
