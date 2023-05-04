using GraphQL.Types;

namespace ToDoList.GraphQL.GraphQLTypes
{
    public class TaskUpdateType : ObjectGraphType<DBmodels.Task>
    {
        public TaskUpdateType() {
            Name = "TaskUpdateStatus";

            Field(x => x.Id, nullable: false, typeof(IntGraphType));
            Field(x => x.CategoryId, nullable: false, typeof(IntGraphType));
        }

    }
}
