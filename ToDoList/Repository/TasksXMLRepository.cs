using System.Data;
using System.Xml.Linq;

namespace ToDoList.Repository
{
    public class TasksXMLRepository : ITaskRepository
    {
        private readonly XDocument _document;
        private readonly string _path;
        public TasksXMLRepository(IConfiguration config)
        {
            var filePath = config.GetValue<string>("StorageSettings:XmlFilePath");
            _document = XDocument.Load(filePath);
            _path = filePath;
        }
        public async System.Threading.Tasks.Task<int> CreateAsync(DBmodels.Task task)
        {
            int id = 0;
            bool isExist = true;
            XElement elem = null;

            do
            {
                elem = _document.Root
                .Element("tasks")
                .Elements("task")
                .Where(t => (int)t.Element("id") == id)
                .FirstOrDefault();

                if (elem == null)
                    isExist = false;
                else
                { 
                    Random rnd = new Random();
                    id = rnd.Next();
                }
            } while (isExist);

            XElement taskElement = new XElement("task",
                new XElement("id", id),
                new XElement("categoryId", task.CategoryId),
                new XElement("title", task.Title),
                new XElement("deadline", task.Deadline),
                new XElement("isCompleted", task.IsCompleted)
            );


            _document.Root.Element("tasks").Add(taskElement);
            await Task.Run(() => _document.Save(_path));

            return id;
        }

        public async System.Threading.Tasks.Task<bool> DeleteAsync(int id)
        {
            XElement taskElement = _document.Root
                .Element("tasks")
                .Elements("task")
                .Where(t => (int)t.Element("id") == id)
                .FirstOrDefault();

            if (taskElement != null)
            {
                taskElement.Remove();
                await Task.Run(() => _document.Save(_path));

                return true; 
            }
            else
                return false;
        }

        public async Task<IEnumerable<DBmodels.Task>> GetAllAsync()
        {
            return await Task.Run(() =>
               _document.Root
                   .Element("tasks")
                   .Elements("task")
                   .Select(t => new DBmodels.Task
                   {
                       Id = (int)t.Element("id"),
                       CategoryId = (int)t.Element("categoryId"),
                       Title = (string)t.Element("title"),
                       Deadline = (DateTime?)t.Element("deadline"),
                       IsCompleted = (bool)t.Element("isCompleted")
                   })
           );
        }

        public async Task<IEnumerable<DBmodels.Task>> GetAllByStatusAsync(bool IsCompleted)
        {
            return await Task.Run(() =>
               _document.Root
                   .Element("tasks")
                   .Elements("task").Where(t => (bool)t.Element("isCompleted") == IsCompleted)
                   .Select(t => new DBmodels.Task
                   {
                       Id = (int)t.Element("id"),
                       CategoryId = (int)t.Element("categoryId"),
                       Title = (string)t.Element("title"),
                       Deadline = (DateTime?)t.Element("deadline"),
                       IsCompleted = (bool)t.Element("isCompleted")
                   })
           );
        }

        public async System.Threading.Tasks.Task<bool> UpdateStatusAsync(int id, bool IsCompleted)
        {
            XElement taskElement = _document.Root
                .Element("tasks")
                .Elements("task")
                .Where(t => (int)t.Element("id") == id)
                .FirstOrDefault();

            if (taskElement != null)
            {
                taskElement.Element("isCompleted").Value = IsCompleted.ToString();
                await Task.Run(() => _document.Save(_path));

                return true;
            }
            else 
                return false; 
           
        }
    }
}
