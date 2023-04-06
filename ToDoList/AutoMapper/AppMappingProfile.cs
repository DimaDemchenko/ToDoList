using AutoMapper;
using ToDoList.Models;

namespace ToDoList.AutoMapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<TaskValidationModel, DBmodels.Task>();
            CreateMap<TaskValidationModel, DBmodels.Task>().ReverseMap();
        }
    }
}
