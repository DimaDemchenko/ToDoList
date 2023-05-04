using GraphQL;
using GraphQL.Types;
using ToDoList.GraphQL.GraphQLTypes;
using ToDoList.Services;

namespace ToDoList.GraphQL.Mutations
{
    public class TaskMutation : ObjectGraphType
    {
        //refactor this method
        public TaskMutation(TaskProvider provider)
        {
            FieldAsync<TaskType>("createTask",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<TaskInputType>>()
                    { Name = "task" }),
                resolve: async context =>
                {
                    var task = context.GetArgument<DBmodels.Task>("task");

                    await provider.GetTaskRepository().CreateAsync(task);

                    return task;
                }

                );

            FieldAsync<IntGraphType>("updateStatusTask", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>>()
                {
                    Name = "Id"
                }, new QueryArgument<NonNullGraphType<BooleanGraphType>>()
                { 
                    Name = "Status"
                }
                ), resolve: async context =>
                {
                    int id = context.GetArgument<int>("Id");
                    bool status = context.GetArgument<bool>("status");

                    await provider.GetTaskRepository().UpdateStatusAsync(id, status);

                    return id;
                }
                );
        }


    }
}
