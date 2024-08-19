using AutoMapper;
using sunex_task_project.Dtos;
using sunex_task_project.Models;

namespace sunex_task_project.MappingProfiles
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            // Map TaskRequestDto to TaskItem for creation
            CreateMap<TaskRequestDto, TaskItem>()
                .ForMember(dest => dest.TaskId, opt => opt.Ignore()); // Ignore TaskId on creation

            // Map TaskItem to TaskResponseDto for response
            CreateMap<TaskItem, TaskResponseDto>();

            // Map TaskUpdateDto to TaskItem for updating
            CreateMap<TaskUpdateDto, TaskItem>()
                .ForMember(dest => dest.TaskId, opt => opt.Ignore()); // Ignore TaskId when updating, as it should not change
        }
    }
}
