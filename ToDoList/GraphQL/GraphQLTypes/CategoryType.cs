using GraphQL.Types;
using ToDoList.DBmodels;

namespace ToDoList.GraphQL.GraphQLTypes
{
    public class CategoryType : ObjectGraphType<Category>
    {
        public CategoryType() 
        {
            Field(x => x.Id, nullable: false, type: typeof(IdGraphType)).Description("Id of a category");
            Field(x => x.Name, nullable: false, type: typeof(StringGraphType)).Description("Name of a category");
        }
    }
}
