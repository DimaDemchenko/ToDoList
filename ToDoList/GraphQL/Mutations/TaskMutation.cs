using GraphQL;
using GraphQL.Types;
using ToDoList.EnumData;
using ToDoList.GraphQL.GraphQLTypes.StorageTypes;
using ToDoList.GraphQL.GraphQLTypes.TaskTypes;
using ToDoList.Services;

namespace ToDoList.GraphQL.Mutations
{
    public class TaskMutation : ObjectGraphType
    {
        public TaskMutation(TaskProvider provider, CookieService service)
        {
            FieldAsync<IntGraphType>("createTask",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<TaskInputType>>()
                    { Name = "task" }),
                resolve: async context =>
                {
                    var task = context.GetArgument<DBmodels.Task>("task");

                    return await provider.GetTaskRepository().CreateAsync(task);
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

            FieldAsync<BooleanGraphType>("deleteTask", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>>()
                {
                    Name = "Id"
                }
                ), resolve: async context =>
                {
                    int id = context.GetArgument<int>("Id");

                    await provider.GetTaskRepository().DeleteAsync(id);

                    return true;
                }
                );

            FieldAsync<BooleanGraphType>("changeStorageType",
                    arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StorageTypeEnum>>()
                    {
                        Name = "storageType"
                        }
                    ),
                resolve: async context =>
                {
                    StorageType storageType = context.GetArgument<StorageType>("storageType");

                    if (storageType == StorageType.XML)
                    {
                        service.Set("Storage", StorageType.XML.ToString());
                    }
                    else
                    {
                        service.Set("Storage", StorageType.SQL.ToString());
                    }

                    return true;
                });
        }


    }
}
