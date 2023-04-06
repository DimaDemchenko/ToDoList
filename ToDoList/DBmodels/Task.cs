
namespace ToDoList.DBmodels
{
    public class Task
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public Category Category { get; set; }
    }
}
