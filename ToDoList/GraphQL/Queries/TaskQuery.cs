using GraphQL;
using GraphQL.Types;
using ToDoList.GraphQL.GraphQLTypes;
using ToDoList.Services;

namespace ToDoList.GraphQL.Queries
{
    public class TaskQuery:ObjectGraphType
    {

        public TaskQuery(TaskProvider provider)
        {
            FieldAsync<ListGraphType<TaskType>>(
                "Tasks",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<BooleanGraphType>>
                {
                    Name = "status",
                    Description = "Filter tasks by status"
                }),
                resolve: async context =>
                {
                    bool status = context.GetArgument<bool>("status");
                    var tasks = await provider.GetTaskRepository().GetAllByStatusAsync(status);
                    return tasks;
                });
        }
    }
}
