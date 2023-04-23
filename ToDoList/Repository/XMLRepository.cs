using Microsoft.Extensions.Options;
using System.Xml.Linq;
using ToDoList.DBmodels;
using TaskDB = ToDoList.DBmodels.Task;
using TaskThread = System.Threading.Tasks.Task;

namespace ToDoList.Repository
{
    public class XMLRepository
    {
        private readonly XDocument _document;
        private readonly string _path;

        public XMLRepository(IConfiguration config)
        {
            var filePath = config.GetValue<string>("StorageSettings:XmlFilePath");
            _document = XDocument.Load(filePath);
            _path = filePath;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await TaskThread.Run(() =>
                _document.Root
                    .Element("categories")
                    .Elements("category")
                    .Select(c => new Category
                    {
                        Id = (int?)c.Element("id"),
                        Name = (string?)c.Element("name")
                    })
            );
        }

        public async Task<IEnumerable<TaskDB>> GetTasksAsync()
        {
            return await TaskThread.Run(() =>
                _document.Root
                    .Element("tasks")
                    .Elements("task")
                    .Select(t => new TaskDB
                    {
                        Id = (int)t.Element("id"),
                        CategoryId = (int)t.Element("categoryId"),
                        Title = (string)t.Element("title"),
                        Deadline = (DateTime?)t.Element("deadline"),
                        IsCompleted = (bool)t.Element("isCompleted")
                    })
            );
        }

        public async Task<IEnumerable<TaskDB>> GetTasksByStatusAsync(bool status)
        {
            return await TaskThread.Run(() =>
                _document.Root
                    .Element("tasks")
                    .Elements("task").Where(t => (bool)t.Element("isCompleted") == status)
                    .Select(t => new TaskDB
                    {
                        Id = (int)t.Element("id"),
                        CategoryId = (int)t.Element("categoryId"),
                        Title = (string)t.Element("title"),
                        Deadline = (DateTime?)t.Element("deadline"),
                        IsCompleted = (bool)t.Element("isCompleted")
                    })
            );
        }

        public async Task<TaskDB> GetTaskByIdAsync(int id)
        {
            return await TaskThread.Run(() =>
                _document.Root
                    .Element("tasks")
                    .Elements("task")
                    .Where(t => (int)t.Element("id") == id)
                    .Select(t => new TaskDB
                    {
                        Id = (int)t.Element("id"),
                        CategoryId = (int)t.Element("categoryId"),
                        Title = (string)t.Element("title"),
                        Deadline = (DateTime?)t.Element("deadline"),
                        IsCompleted = (bool)t.Element("isCompleted")
                    })
                    .FirstOrDefault()
            );
        }

        public async TaskThread CreateTaskAsync(TaskDB task)
        {
            XElement taskElement = new XElement("task",
                new XElement("id", task.Id),
                new XElement("categoryId", task.CategoryId),
                new XElement("title", task.Title),
                new XElement("deadline", task.Deadline),
                new XElement("isCompleted", task.IsCompleted)
            );

            _document.Root.Element("tasks").Add(taskElement);
            await TaskThread.Run(()=> _document.Save(_path));
        }

        public async TaskThread UpdateTaskStatusAsync(int id, bool status)
        {
            XElement taskElement = _document.Root
                .Element("tasks")
                .Elements("task")
                .Where(t => (int)t.Element("id") == id)
                .FirstOrDefault();

            if (taskElement != null)
            {
                taskElement.Element("isCompleted").Value = status.ToString();
                await TaskThread.Run(() => _document.Save(_path));
            }
        }

        public async TaskThread DeleteTaskByIdAsync(int id)
        {
            XElement taskElement = _document.Root
                .Element("tasks")
                .Elements("task")
                .Where(t => (int)t.Element("id") == id)
                .FirstOrDefault();

            if (taskElement != null)
            {
                taskElement.Remove();
                await  TaskThread.Run(() => _document.Save(_path));
            }
        }

    }
}
