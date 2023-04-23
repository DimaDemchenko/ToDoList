using ToDoList.DBmodels;
using ToDoList.EnumData;
using ToDoList.Repository;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Services
{
    public class DataService
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly CookieService _cookieService;
        private readonly XMLRepository _xMLRepository;
        private readonly StorageType _storageType;
        public DataService(ITasksRepository tasksRepository, ICategoriesRepository categoriesRepository, CookieService cookieService, XMLRepository xMLRepository)
        {
            _tasksRepository = tasksRepository;
            _categoriesRepository = categoriesRepository;
            _cookieService = cookieService;
            _xMLRepository = xMLRepository;
            _storageType = _cookieService.Get("Storage");
        }

        public async Task<IEnumerable<DBmodels.Task>> GetAllTasksByStatusAsync(bool IsCompleted)
        {
            if (_storageType == StorageType.SQL)
            {
                return await _tasksRepository.GetAllByStatusAsync(IsCompleted); 
            }
            else if(_storageType == StorageType.XML)
            {
                return await _xMLRepository.GetTasksByStatusAsync(IsCompleted);
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            if (_storageType == StorageType.SQL)
            {
                return await _categoriesRepository.GetAllAsync();
            }
            else if (_storageType == StorageType.XML)
            {
                return await _xMLRepository.GetCategoriesAsync();
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task UpdateTaskStatusAsync(int id, bool status)
        {
            if (_storageType == StorageType.SQL)
            {
                await _tasksRepository.UpdateStatusAsync(id, status);
            }
            else if (_storageType == StorageType.XML)
            {
                await _xMLRepository.UpdateTaskStatusAsync(id, status);
            }
        }

        public async Task DeleteTaskAsync(int id)
        {
            if (_storageType == StorageType.SQL)
            {
                await _tasksRepository.DeleteAsync(id);
            }
            else if (_storageType == StorageType.XML)
            {
                await _xMLRepository.DeleteTaskByIdAsync(id);
            }
        }

        public async Task CreateTaskAsync(DBmodels.Task task)
        {
            if (_storageType == StorageType.SQL)
            {
                await _tasksRepository.CreateAsync(task);
            }
            else if (_storageType == StorageType.XML)
            {
                Random rnd = new Random();

                do
                {
                    task.Id = rnd.Next(0, 20000);
                } while (await _xMLRepository.GetTaskByIdAsync(task.Id) != null);

                await _xMLRepository.CreateTaskAsync(task);
            }
        }
    }
}
