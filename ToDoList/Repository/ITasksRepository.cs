using ToDoList.DBmodels;

namespace ToDoList.Repository
{
    public interface ITasksRepository
    {
        System.Threading.Tasks.Task<IEnumerable<DBmodels.Task>> GetAllAsync();

        System.Threading.Tasks.Task CreateAsync(DBmodels.Task task);

        System.Threading.Tasks.Task DeleteAsync(int id);

        System.Threading.Tasks.Task UpdateStatusAsync(int id, bool IsCompleted);
    }
}
