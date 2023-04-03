using ToDoList.DBmodels;

namespace ToDoList.Repository
{
    public interface ICategoriesRepository
    {
        Task<Category> GetByIdAsync(int id);

        Task<IEnumerable<Category>> GetAllAsync();
    }
}
