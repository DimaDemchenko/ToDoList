using ToDoList.DBmodels;
using ToDoList.Models;

namespace ToDoList.Repository
{
    public interface ITasksRepository
    {
        Task<IEnumerable<Tasks>> GetAllAsync();

        Task<int> CreateAsync(Tasks task);

        Task<bool> DeleteAsync(int id);

        Task<bool> UpdateStatusAsync(int id, bool IsCompleted);

        Task<IEnumerable<JoinedTasksAndCategories>> GetJoinedTasksAndCategoriesAsync();
    }
}
