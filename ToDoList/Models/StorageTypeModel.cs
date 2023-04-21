using System.ComponentModel.DataAnnotations;
using ToDoList.EnumData;

namespace ToDoList.Models
{
    public class StorageTypeModel
    {
        [Required(ErrorMessage ="StorageType is required")]
        public StorageType StorageType { get; set; }
    }
}
