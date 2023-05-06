using ToDoList.DBmodels;

namespace ToDoList.Repository
{
    public interface ITaskRepository
    {
        System.Threading.Tasks.Task<IEnumerable<DBmodels.Task>> GetAllAsync();

        System.Threading.Tasks.Task<int> CreateAsync(DBmodels.Task task);

        System.Threading.Tasks.Task DeleteAsync(int id);

        System.Threading.Tasks.Task UpdateStatusAsync(int id, bool IsCompleted);

        System.Threading.Tasks.Task<IEnumerable<DBmodels.Task>> GetAllByStatusAsync(bool IsCompleted);
    }
}
