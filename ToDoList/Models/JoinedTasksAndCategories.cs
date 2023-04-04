namespace ToDoList.Models
{
    public class JoinedTasksAndCategories
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public string Name { get; set; }
    }
}
