using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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

        public async Task<IActionResult> Index()
        {
            var model = await _tasksRepository.GetJoinedTasksAndCategoriesAsync();
            
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}