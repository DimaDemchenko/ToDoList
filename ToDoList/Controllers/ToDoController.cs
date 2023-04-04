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

        public ToDoController(ILogger<ToDoController> logger, ITasksRepository tasksRepository, ICategoriesRepository categoriesRepository)
        {
            _logger = logger;
            _tasksRepository = tasksRepository;
            _categoriesRepository = categoriesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var joinedCategoriesAndTasks = await _tasksRepository.GetJoinedTasksAndCategoriesAsync();
            var categories = await _categoriesRepository.GetAllAsync();

            IndexModel indexModel = new IndexModel
            {
                JoinedTasksAndCategories = joinedCategoriesAndTasks.ToList(),
                Categories = categories.ToList(),
            };
            return View(indexModel);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {

        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskValidation taskValidation) 
        {
            var joinedCategoriesAndTasks = await _tasksRepository.GetJoinedTasksAndCategoriesAsync();
            var categories = await _categoriesRepository.GetAllAsync();
            IndexModel indexModel = new IndexModel
            {
                JoinedTasksAndCategories = joinedCategoriesAndTasks.ToList(),
                Categories = categories.ToList(),
            };

            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TaskValidation, Tasks>();
                });
                var mapper = new Mapper(config);
                var task = mapper.Map<Tasks>(taskValidation);

                int cretead = await _tasksRepository.CreateAsync(task);
                Console.WriteLine(cretead);

                return Redirect("/todo/index");
            }

            return View("Index", indexModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}