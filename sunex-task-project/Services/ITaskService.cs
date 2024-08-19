using sunex_task_project.Dtos;
using sunex_task_project.Models;

namespace sunex_task_project.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDto>> GetTasksAsync();
        Task<TaskResponseDto?> GetTaskByIdAsync(int taskId);
        Task<TaskResponseDto> AddTaskAsync(TaskRequestDto task);
        Task<TaskResponseDto?> UpdateTaskAsync(int id, TaskUpdateDto task);
        Task<bool> DeleteTaskAsync(int taskId);
    }
}
