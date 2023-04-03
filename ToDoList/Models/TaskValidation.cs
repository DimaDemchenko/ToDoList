using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class TaskValidation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [StringLength(55,ErrorMessage = "Title has to be more than 3 symbols", MinimumLength = 3)]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public DateTime? Deadline { get; set; }

        public bool IsCompleted { get; set; }
    }
}
