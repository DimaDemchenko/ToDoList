using ToDoList.GraphQL.Queries;
using GraphQL.Types;
using ToDoList.GraphQL.Mutations;

namespace ToDoList.GraphQL.Schemes
{
    public class AppScheme:Schema
    {
        public AppScheme(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<TaskQuery>();
            Mutation = provider.GetRequiredService<TaskMutation>();

        }
    }
}
