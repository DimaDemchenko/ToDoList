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
        public TaskMutation(StorageProvider provider, CookieService service)
        {
            FieldAsync<IntGraphType>("createTask",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<TaskInputType>>()
                    { Name = "task" }),
                resolve: async context =>
                {
                    var storage = service.GetStorageFromHeader();
                    var task = context.GetArgument<DBmodels.Task>("task");

                    return await provider.GetTaskRepository(storage).CreateAsync(task);
                }

                );

            FieldAsync<BooleanGraphType>("updateStatusTask", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>>()
                {
                    Name = "Id"
                }, new QueryArgument<NonNullGraphType<BooleanGraphType>>()
                {
                    Name = "Status"
                }
                ), resolve: async context =>
                {
                    var storage = service.GetStorageFromHeader();
                    int id = context.GetArgument<int>("Id");
                    bool status = context.GetArgument<bool>("status");

                    return await provider.GetTaskRepository(storage).UpdateStatusAsync(id, status);
                }
                );

            FieldAsync<BooleanGraphType>("deleteTask", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>>()
                {
                    Name = "Id"
                }
                ), resolve: async context =>
                {
                    var storage = service.GetStorageFromHeader();
                    int id = context.GetArgument<int>("Id");

                    return await provider.GetTaskRepository(storage).DeleteAsync(id);

                }
                );

           /* FieldAsync<StringGraphType>("changeStorageType",
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
                        service.SetCookie("Storage", StorageType.XML.ToString());
                        return StorageType.XML.ToString();
                    }
                    else
                    {
                        service.SetCookie("Storage", StorageType.SQL.ToString());
                        return StorageType.SQL.ToString();
                    }

                });*/
        }


    }
}
