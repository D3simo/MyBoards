﻿namespace MyBoards.Entities
{
    public class Comment
    {
        // Comments properties
        public int Id { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // configure relation with WorkItem
        public WorkItem WorkItem { get; set; }
        // configure foreign key, type is based on other table key column
        // EF will create column by default, its called ShadowForeignKeyProperty but its a good practice
        public int WorkItemId { get; set; }
    }
}
