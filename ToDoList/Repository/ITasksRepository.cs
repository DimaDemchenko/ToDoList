using ToDoList.DBmodels;
using ToDoList.Models;

namespace ToDoList.Repository
{
    public interface ITasksRepository
    {
        Task<IEnumerable<Tasks>> GetAllAsync();

        Task CreateAsync(Tasks task);

        Task DeleteAsync(int id);

        Task UpdateStatusAsync(int id, bool IsCompleted);
    }
}
