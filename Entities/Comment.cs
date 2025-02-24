namespace MyBoards.Entities
{
    public class Comment
    {
        // Comments properties
        public int Id { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
