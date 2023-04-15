using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoList.DBmodels;
using ToDoList.Models;
using ToDoList.Repository;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ILogger<ToDoController> _logger;
        private readonly ITasksRepository _tasksRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IMapper _mapper;

        public ToDoController(ILogger<ToDoController> logger, ITasksRepository tasksRepository, ICategoriesRepository categoriesRepository, IMapper mapper)
        {
            _logger = logger;
            _tasksRepository = tasksRepository;
            _categoriesRepository = categoriesRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async  Task<IActionResult> Recover(int id) 
        {
            await _tasksRepository.UpdateStatusAsync(id, false);

            return Redirect("/todo/index");
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
            var tasks = await _tasksRepository.GetAllByStatusAsync(false);
            var categories = await _categoriesRepository.GetAllAsync();

            IndexViewModel indexModel = new IndexViewModel
            {
                Tasks = tasks.OrderBy(c => c.Deadline)
                            .ToList(), 
                Categories = categories.ToList(),
            };
            return View(indexModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _tasksRepository.DeleteAsync(id);

            return Redirect("/todo/index"); 
        }

        [HttpGet]
        public async Task<IActionResult> Complete(int id)
        {
            await _tasksRepository.UpdateStatusAsync(id, true);
            
            return Redirect("/todo/index");
            
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskValidationModel taskValidation) 
        {
            if (ModelState.IsValid)
            {
                var task = _mapper.Map<DBmodels.Task>(taskValidation);

                await _tasksRepository.CreateAsync(task);
            }

            return Redirect("/todo/index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}