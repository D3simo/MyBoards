using Microsoft.AspNetCore.Http.HttpResults;

namespace MyBoards.Entities.ViewModels
{
    public class TopAuthor
    {
        public string FullName { get; set; }

        //namespace the same as Created in the SQL View
        public int WorkItemsCreated { get; set; }
    }
}
