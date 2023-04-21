using ToDoList.DBmodels;

namespace ToDoList.Models
{
    public class IndexViewModel
    {
        public List<DBmodels.Task>? Tasks { get; set; }

        public List<Category>? Categories{ get; set;}

        public TaskValidationModel? TaskValidation { get; set; }

        public StorageTypeModel? StorageType { get; set; }

    }
}
