using System.Xml.Linq;
using GraphQL.Types;
using ToDoList.EnumData;

namespace ToDoList.GraphQL.GraphQLTypes.StorageTypes
{
    public class StorageTypeEnum : EnumerationGraphType<StorageType>
    {
        public StorageTypeEnum()
        {
            Name = "StorageType";
            Description = "The type of storage to use.";
        }
    }
}
