using System.Xml.Linq;
using ToDoList.DBmodels;

namespace ToDoList.Repository
{
    public class CategoryXMLRepository : ICategoriesRepository
    {
        private readonly XDocument _document;

        public CategoryXMLRepository(IConfiguration config)
        {
            var filePath = config.GetValue<string>("StorageSettings:XmlFilePath");
            _document = XDocument.Load(filePath);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await System.Threading.Tasks.Task.Run(() =>
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

        public Task<Category> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
