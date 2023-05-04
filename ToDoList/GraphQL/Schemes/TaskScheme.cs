using ToDoList.GraphQL.Queries;
using GraphQL.Types;
using ToDoList.GraphQL.Mutations;

namespace ToDoList.GraphQL.Schemes
{
    public class TaskScheme:Schema
    {
        public TaskScheme(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<TaskQuery>();
            Mutation = provider.GetRequiredService<TaskMutation>();

        }
    }
}
