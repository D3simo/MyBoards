namespace MyBoards.Entities
{
    public class WorkItemTag
    {
        // WorkItemTag properties
        public WorkItem WorkItem { get; set; }
        // foreign key to WorkItem entity
        public int WorkItemId { get; set; }
        public Tag Tag { get; set; }
        // foreign key to Tag entity
        public int TagId { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
