using ToDoList.DBmodels;

namespace ToDoList.Repository
{
    public interface ITasksRepository
    {
        Task<IEnumerable<Tasks>> GetAllAsync();

        Task<int> CreateAsync(Tasks task);

        Task<bool> DeleteAsync(int id);
    }
}
