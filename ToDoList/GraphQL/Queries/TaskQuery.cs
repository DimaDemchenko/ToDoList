using GraphQL;
using GraphQL.Types;
using ToDoList.GraphQL.GraphQLTypes.CategoryTypes;
using ToDoList.GraphQL.GraphQLTypes.TaskTypes;
using ToDoList.Services;

namespace ToDoList.GraphQL.Queries
{
    public class TaskQuery: ObjectGraphType
    {

        public TaskQuery(StorageProvider provider, CookieService cookieService)
        {
            FieldAsync<ListGraphType<TaskType>>(
                "tasks",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<BooleanGraphType>>
                {
                    Name = "status",
                    Description = "Filter tasks by status"
                }),
                resolve: async context =>
                {
                    var storage = cookieService.GetStorageFromHeader();
                    bool status = context.GetArgument<bool>("status");
                    var tasks = await provider.GetTaskRepository(storage).GetAllByStatusAsync(status);

                    return tasks;
                });

            FieldAsync<ListGraphType<TaskType>>(
                "allTasks",
                resolve: async context =>
                {
                    var storage = cookieService.GetStorageFromHeader();
                    var tasks = await provider.GetTaskRepository(storage).GetAllAsync();

                    return tasks;
                });

            FieldAsync<ListGraphType<CategoryType>>(
                "categories", resolve:
                async context =>
                {
                    var storage = cookieService.GetStorageFromHeader();

                    return await provider.GetCategoriesRepository(storage).GetAllAsync();
                });
        }
    }
}
