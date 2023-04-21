using ToDoList.DBmodels;
using ToDoList.EnumData;

namespace ToDoList.Models
{
    public class IndexViewModel
    {
        public List<DBmodels.Task>? Tasks { get; set; }

        public List<Category>? Categories{ get; set;}

        public TaskValidationModel? TaskValidation { get; set; }

        public StorageTypeModel? StorageTypeModel { get; set; }

        public StorageType selectedType { get; set; }

    }
}
