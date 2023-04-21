using System.ComponentModel.DataAnnotations;
using ToDoList.Enum;

namespace ToDoList.Models
{
    public class StorageTypeModel
    {
        [Required(ErrorMessage ="StorageType is required")]
        public StorageType StorageType { get; set; }
    }
}
