using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoList.DBmodels;
using ToDoList.Models;
using ToDoList.Repository;
using ToDoList.Services;
using ToDoList.EnumData;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ILogger<ToDoController> _logger;
        private readonly ITasksRepository _tasksRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IMapper _mapper;
        private readonly SessionService _sessionService;
        private readonly XMLRepository _xMLRepository;
        private StorageType _storageType;
        public ToDoController(ILogger<ToDoController> logger, ITasksRepository tasksRepository, ICategoriesRepository categoriesRepository, IMapper mapper, SessionService sessionService, XMLRepository xMLRepository)
        {
            _logger = logger;
            _tasksRepository = tasksRepository;
            _categoriesRepository = categoriesRepository;
            _mapper = mapper;
            _sessionService = sessionService;
            _xMLRepository = xMLRepository;
        }

        [HttpPost]
        public IActionResult Change(StorageTypeModel storageTypeModel)
        {
            if (ModelState.IsValid)
            {
                _sessionService.Set("Storage", storageTypeModel.StorageType.ToString());
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async  Task<IActionResult> Recover(int id) 
        {
            await _tasksRepository.UpdateStatusAsync(id, false);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> History() 
        {
            var tasks = await _tasksRepository.GetAllByStatusAsync(true);

            HistoryViewModel indexModel = new HistoryViewModel
            {
                Tasks = tasks.OrderBy(c => c.Deadline)
                            .ToList(),
            };
            return View(indexModel);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _storageType = _sessionService.Get("Storage");
            if (_storageType == StorageType.SQL)
            {
                var tasks = _tasksRepository.GetAllByStatusAsync(false);
                var categories = _categoriesRepository.GetAllAsync();

                await Task.WhenAll(tasks, categories);


                IndexViewModel indexModel = new IndexViewModel
                {
                    Tasks = tasks.Result.OrderBy(c => c.Deadline)
                                .ToList(),
                    Categories = categories.Result.ToList(),

                    selectedType = _storageType
                };
                return View(indexModel);
            }
            else if (_storageType == StorageType.XML)
            {
                var tasks = _xMLRepository.GetTasksAsync();
                var categories = _xMLRepository.GetCategoriesAsync();

                await Task.WhenAll(tasks, categories);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _tasksRepository.DeleteAsync(id);

            return RedirectToAction("Index"); 
        }

        [HttpGet]
        public async Task<IActionResult> Complete(int id)
        {
            await _tasksRepository.UpdateStatusAsync(id, true);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskValidationModel taskValidation) 
        {
            if (ModelState.IsValid)
            {
                var task = _mapper.Map<DBmodels.Task>(taskValidation);

                await _tasksRepository.CreateAsync(task);

                return RedirectToAction("Index");
            }

            _storageType = _sessionService.Get("Storage");
            var tasks = _tasksRepository.GetAllByStatusAsync(false);
            var categories = _categoriesRepository.GetAllAsync();

            await Task.WhenAll(tasks, categories);


            IndexViewModel indexModel = new IndexViewModel
            {
                Tasks = tasks.Result.OrderBy(c => c.Deadline)
                            .ToList(),
                Categories = categories.Result.ToList(),
                selectedType = _storageType
            };
            return View("Index",indexModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}