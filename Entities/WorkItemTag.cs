namespace MyBoards.Entities
{
    public class WorkItemTag
    {
        // WorkItemTag properties
        public virtual WorkItem WorkItem { get; set; }
        // foreign key to WorkItem entity
        public int WorkItemId { get; set; }
        public virtual Tag Tag { get; set; }
        // foreign key to Tag entity
        public int TagId { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
