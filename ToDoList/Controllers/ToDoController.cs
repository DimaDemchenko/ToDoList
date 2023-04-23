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
        private readonly CookieService _cookiesService;
        private readonly DataService _dataService;

        public ToDoController(ILogger<ToDoController> logger, ITasksRepository tasksRepository, ICategoriesRepository categoriesRepository, IMapper mapper, CookieService sessionService, DataService dataService)
        {
            _logger = logger;
            _tasksRepository = tasksRepository;
            _categoriesRepository = categoriesRepository;
            _mapper = mapper;
            _cookiesService = sessionService;
            _dataService = dataService;
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
            await _dataService.UpdateTaskStatusAsync(id, false);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> History() 
        {
            IEnumerable<DBmodels.Task> tasks = await _dataService.GetAllTasksByStatusAsync(true);
            IEnumerable<Category> categories = await _dataService.GetAllCategoriesAsync();

            return View(CreateHistoryViewModel(tasks, categories));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<DBmodels.Task> tasks = await _dataService.GetAllTasksByStatusAsync(false);
            IEnumerable<Category> categories = await _dataService.GetAllCategoriesAsync();

            return View("Index", CreateIndexViewModel(tasks, categories));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _dataService.DeleteTaskAsync(id);

            return RedirectToAction("Index"); 
        }

        [HttpGet]
        public async Task<IActionResult> Complete(int id)
        {
            await _dataService.UpdateTaskStatusAsync(id, true);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskValidationModel taskValidation) 
        {

            if (ModelState.IsValid)
            {
                var task = _mapper.Map<DBmodels.Task>(taskValidation);

                await _dataService.CreateTaskAsync(task);

                return RedirectToAction("Index");
            }

            IEnumerable<DBmodels.Task> tasks = await _dataService.GetAllTasksByStatusAsync(false);
            IEnumerable<Category> categories = await _dataService.GetAllCategoriesAsync();

            return View("Index", CreateIndexViewModel(tasks, categories));
        }

        private IndexViewModel CreateIndexViewModel(IEnumerable<DBmodels.Task> tasks, IEnumerable<Category> categories)
        {
            var indexModel = new IndexViewModel
            {
                Tasks = tasks.OrderBy(c => c.Deadline).ToList(),
                Categories = categories.ToList(),
                selectedType = _cookiesService.Get("Storage")
            };
            return indexModel;
        }

        private HistoryViewModel CreateHistoryViewModel(IEnumerable<DBmodels.Task> tasks, IEnumerable<Category> categories)
        {
            HistoryViewModel historyModel = new HistoryViewModel
            {
                Tasks = tasks.OrderBy(c => c.Deadline)
                            .ToList(),
                Categories = categories.ToList(),
                selectedType = _cookiesService.Get("Storage")
            };

            return historyModel;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}