namespace MyBoards.Entities
{
    public class Tag
    {
        // Tags properties
        public int Id { get; set; }
        public string Value { get; set; }
        public List<WorkItem> WorkItems { get; set; }
    }
}
