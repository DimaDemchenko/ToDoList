using GraphQL.Types;

namespace ToDoList.GraphQL.GraphQLTypes
{
    public class TaskInputType : InputObjectGraphType<DBmodels.Task>
    {
        public TaskInputType()
        {
            Name = "TaskInput";

            Field(x => x.Title, nullable: false, typeof(StringGraphType));
            Field(x => x.Deadline, nullable: false, typeof(DateTimeGraphType));
            Field(x => x.CategoryId, nullable: false, typeof(IntGraphType));
            Field(x => x.IsCompleted, nullable: false, typeof(BooleanGraphType));
        }
    }
}
