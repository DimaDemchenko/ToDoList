using ToDoList.GraphQL.Queries;
using GraphQL.Types;
namespace ToDoList.GraphQL.Schemes
{
    public class TaskScheme:Schema
    {
        public TaskScheme(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<TaskQuery>();


        }
    }
}
