using Microsoft.AspNetCore.SignalR;
using sunex_task_project.Dtos;
using System.Threading.Tasks;

namespace sunex_task_project.Hubs
{
    public interface ITaskHubClient
    {
        Task NotifyTaskAdded(TaskResponseDto task);
        Task NotifyTaskUpdated(TaskResponseDto task);
        Task NotifyTaskDeleted(int taskId);
    }
}
