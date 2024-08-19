using Microsoft.AspNetCore.SignalR;
using sunex_task_project.Dtos;
using System.Threading.Tasks;

namespace sunex_task_project.Hubs
{
    public class TaskHubClient : ITaskHubClient
    {
        private readonly IHubContext<TaskHub> _hubContext;

        public TaskHubClient(IHubContext<TaskHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyTaskAdded(TaskResponseDto task)
        {
            await _hubContext.Clients.All.SendAsync("TaskAdded", task);
        }

        public async Task NotifyTaskUpdated(TaskResponseDto task)
        {
            await _hubContext.Clients.All.SendAsync("TaskUpdated", task);
        }

        public async Task NotifyTaskDeleted(int taskId)
        {
            await _hubContext.Clients.All.SendAsync("TaskDeleted", taskId);
        }
    }
}
