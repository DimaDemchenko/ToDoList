using GraphQL.Types;

namespace ToDoList.GraphQL.GraphQLTypes.TaskTypes
{
    public class TaskType : ObjectGraphType<DBmodels.Task>
    {
        public TaskType()
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Id of a task");
            Field(x => x.CategoryId, type: typeof(IntGraphType)).Description("Category id of a task");
            Field(x => x.Title, type: typeof(StringGraphType)).Description("Title of a task");
            Field(x => x.Deadline, type: typeof(DateTimeGraphType)).Description("Deadline of a task");
            Field(x => x.IsCompleted, type: typeof(BooleanGraphType)).Description("Status of a task");
        }
    }
}
