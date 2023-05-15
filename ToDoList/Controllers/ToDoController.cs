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
        private readonly IMapper _mapper;
        private readonly CookieService _cookieService;
        private readonly StorageProvider _provider;

        public ToDoController(ILogger<ToDoController> logger, IMapper mapper, CookieService cookieService, StorageProvider provider)
        {
            _logger = logger;
            _mapper = mapper;
            _cookieService = cookieService;
            _provider = provider;
        }

        [HttpPost]
        public IActionResult Change(StorageTypeModel storageTypeModel)
        {
            if (ModelState.IsValid)
            {
                _cookieService.SetCookie("Storage", storageTypeModel.StorageType.ToString());
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async  Task<IActionResult> Recover(int id) 
        {
            var currentStorage = _cookieService.GetStorage("Storage");

            await _provider.GetTaskRepository(currentStorage).UpdateStatusAsync(id, false);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> History() 
        {
            var currentStorage = _cookieService.GetStorage("Storage");

            IEnumerable<DBmodels.Task> tasks = await _provider.GetTaskRepository(currentStorage).GetAllByStatusAsync(true);
            IEnumerable<Category> categories = await _provider.GetCategoriesRepository(currentStorage).GetAllAsync();

            return View(CreateHistoryViewModel(tasks, categories));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentStorage = _cookieService.GetStorage("Storage");

            IEnumerable<DBmodels.Task> tasks = await _provider.GetTaskRepository(currentStorage).GetAllByStatusAsync(false);
            IEnumerable<Category> categories = await _provider.GetCategoriesRepository(currentStorage).GetAllAsync();

            return View("Index", CreateIndexViewModel(tasks, categories));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var currentStorage = _cookieService.GetStorage("Storage");

            await _provider.GetTaskRepository(currentStorage).DeleteAsync(id);

            return RedirectToAction("Index"); 
        }

        [HttpGet]
        public async Task<IActionResult> Complete(int id)
        {
            var currentStorage = _cookieService.GetStorage("Storage");

            await _provider.GetTaskRepository(currentStorage).UpdateStatusAsync(id, true);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskValidationModel taskValidation) 
        {
            var currentStorage = _cookieService.GetStorage("Storage");

            if (ModelState.IsValid)
            {
                var task = _mapper.Map<DBmodels.Task>(taskValidation);

                await _provider.GetTaskRepository(currentStorage).CreateAsync(task);

                return RedirectToAction("Index");
            }

            IEnumerable<DBmodels.Task> tasks = await _provider.GetTaskRepository(currentStorage).GetAllByStatusAsync(false);
            IEnumerable<Category> categories = await _provider.GetCategoriesRepository(currentStorage).GetAllAsync();

            return View("Index", CreateIndexViewModel(tasks, categories));
        }

        private IndexViewModel CreateIndexViewModel(IEnumerable<DBmodels.Task> tasks, IEnumerable<Category> categories)
        {
            var indexModel = new IndexViewModel
            {
                Tasks = tasks.OrderBy(c => c.Deadline).ToList(),
                Categories = categories.ToList(),
                selectedType = _cookieService.GetStorage("Storage")
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
                selectedType = _cookieService.GetStorage("Storage")
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