using System.ComponentModel.DataAnnotations;
using ToDoList.ValidationAttributes;

namespace ToDoList.Models
{
    public class TaskValidationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [CategoryValidation(ErrorMessage = "Choose correct category!")]
        public int CategoryId { get; set; }

        [StringLength(30,ErrorMessage = "Title has to be more than 3 symbols", MinimumLength = 3)]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [DateValidation(ErrorMessage = "Choose correct date!")]
        public DateTime? Deadline { get; set; }

        public bool IsCompleted { get; set; }
    }
}
