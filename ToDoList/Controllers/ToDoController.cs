using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoList.DBmodels;
using ToDoList.Models;
using ToDoList.Repository;
using ToDoList.Services;
using ToDoList.EnumData;
using Task = System.Threading.Tasks.Task;
using System.Threading.Tasks;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ILogger<ToDoController> _logger;
        private readonly ITasksRepository _tasksRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IMapper _mapper;
        private readonly CookiesService _cookiesService;
        private readonly XMLRepository _xMLRepository;
        private StorageType _storageType;

        public ToDoController(ILogger<ToDoController> logger, ITasksRepository tasksRepository, ICategoriesRepository categoriesRepository, IMapper mapper, CookiesService sessionService, XMLRepository xMLRepository)
        {
            _logger = logger;
            _tasksRepository = tasksRepository;
            _categoriesRepository = categoriesRepository;
            _mapper = mapper;
            _cookiesService = sessionService;
            _xMLRepository = xMLRepository;
        }

        [HttpPost]
        public IActionResult Change(StorageTypeModel storageTypeModel)
        {
            if (ModelState.IsValid)
            {
                _cookiesService.Set("Storage", storageTypeModel.StorageType.ToString());
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async  Task<IActionResult> Recover(int id) 
        {
            _storageType = _cookiesService.Get("Storage");

            if(_storageType == StorageType.SQL)
                await _tasksRepository.UpdateStatusAsync(id, false);
            else if(_storageType == StorageType.XML)
                await _xMLRepository.UpdateTaskAsync(id, false);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> History() 
        {
            _storageType = _cookiesService.Get("Storage");

            IEnumerable<DBmodels.Task> tasks;
            IEnumerable<Category> categories;

            if (_storageType == StorageType.SQL)
            {
                tasks = await _tasksRepository.GetAllByStatusAsync(true);
                categories = await _categoriesRepository.GetAllAsync();
            }
            else if (_storageType == StorageType.XML)
            {
                tasks = await _xMLRepository.GetTasksByStatusAsync(true);
                categories = await _xMLRepository.GetCategoriesAsync();
            }
            else
            {
                throw new Exception("Select storage!");
            }

            HistoryViewModel indexModel = new HistoryViewModel
            {
                Tasks = tasks.OrderBy(c => c.Deadline)
                            .ToList(),
                Categories = categories.ToList(),
                selectedType = _storageType
            };
            return View(indexModel);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _storageType = _cookiesService.Get("Storage");

            IEnumerable<DBmodels.Task> tasks;
            IEnumerable<Category> categories;


            if (_storageType == StorageType.SQL)
            {
                tasks = await _tasksRepository.GetAllByStatusAsync(false);
                categories = await _categoriesRepository.GetAllAsync();
            }
            else if (_storageType == StorageType.XML)
            {
                tasks = await _xMLRepository.GetTasksByStatusAsync(false);
                categories = await _xMLRepository.GetCategoriesAsync();
            }
            else
            {
                throw new Exception("Select storage!");
            }

            return View("Index", CreateIndexViewModel(tasks, categories));

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _storageType = _cookiesService.Get("Storage");

            if(_storageType == StorageType.SQL)
                await _tasksRepository.DeleteAsync(id);
            else if (_storageType == StorageType.XML)
                await _xMLRepository.DeleteTaskByIdAsync(id);

            return RedirectToAction("Index"); 
        }

        [HttpGet]
        public async Task<IActionResult> Complete(int id)
        {
            _storageType = _cookiesService.Get("Storage");

            if(_storageType == StorageType.SQL)
                await _tasksRepository.UpdateStatusAsync(id, true);
            else if(_storageType == StorageType.XML)
                await _xMLRepository.UpdateTaskAsync(id, true);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskValidationModel taskValidation) 
        {
            _storageType = _cookiesService.Get("Storage");

            if (ModelState.IsValid)
            {
                var task = _mapper.Map<DBmodels.Task>(taskValidation);

                if (_storageType == StorageType.SQL)
                    await _tasksRepository.CreateAsync(task);
                else if (_storageType == StorageType.XML)
                { 
                    Random rnd= new Random();
                    do
                    {
                        task.Id = rnd.Next(0, 20000);
                    } while (await _xMLRepository.GetTaskByIdAsync(task.Id) != null);

                    await _xMLRepository.CreateTaskAsync(task);
                }

                return RedirectToAction("Index");
            }

            IEnumerable<DBmodels.Task> tasks = null;
            IEnumerable<Category> categories = null;

            if (_storageType == StorageType.SQL)
            {
                tasks = await _tasksRepository.GetAllByStatusAsync(false);
                categories = await _categoriesRepository.GetAllAsync();
            }
            else if (_storageType == StorageType.XML)
            { 
                tasks = await _xMLRepository.GetTasksByStatusAsync(false);
                categories = await _xMLRepository.GetCategoriesAsync();
            }

            return View("Index", CreateIndexViewModel(tasks, categories));
        }

        private IndexViewModel CreateIndexViewModel(IEnumerable<DBmodels.Task> tasks, IEnumerable<Category> categories)
        {
            var indexModel = new IndexViewModel
            {
                Tasks = tasks.OrderBy(c => c.Deadline).ToList(),
                Categories = categories.ToList(),
                selectedType = _storageType
            };
            return indexModel;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}