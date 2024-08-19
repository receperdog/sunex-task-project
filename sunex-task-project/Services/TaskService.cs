using AutoMapper;
using Microsoft.Extensions.Logging;
using sunex_task_project.Data;
using sunex_task_project.Dtos;
using sunex_task_project.Exceptions;
using sunex_task_project.Hubs;
using sunex_task_project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sunex_task_project.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskService> _logger;
        private readonly ITaskHubClient _taskHubClient;

        public TaskService(ITaskRepository taskRepository, IMapper mapper, ILogger<TaskService> logger, ITaskHubClient taskHubClient)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _logger = logger;
            _taskHubClient = taskHubClient;
        }

        public async Task<IEnumerable<TaskResponseDto>> GetTasksAsync()
        {
            _logger.LogInformation("Fetching all tasks.");
            var tasks = await _taskRepository.GetTasksAsync();
            return _mapper.Map<IEnumerable<TaskResponseDto>>(tasks);
        }

        public async Task<TaskResponseDto?> GetTaskByIdAsync(int taskId)
        {
            _logger.LogInformation("Fetching task with ID {TaskId}.", taskId);
            var task = await _taskRepository.GetTaskByIdAsync(taskId);

            if (task == null)
            {
                _logger.LogWarning("Task with ID {TaskId} was not found.", taskId);
                throw new TaskNotFoundException(taskId);
            }

            return _mapper.Map<TaskResponseDto>(task);
        }

        public async Task<TaskResponseDto> AddTaskAsync(TaskRequestDto taskRequestDto)
        {
            _logger.LogInformation("Adding a new task with title {Title}.", taskRequestDto.Title);

            var task = _mapper.Map<TaskItem>(taskRequestDto);
            var createdTask = await _taskRepository.AddTaskAsync(task);
            var createdTaskResponse = _mapper.Map<TaskResponseDto>(createdTask);

            await _taskHubClient.NotifyTaskAdded(createdTaskResponse);

            return createdTaskResponse;
        }

        public async Task<TaskResponseDto?> UpdateTaskAsync(int id, TaskUpdateDto taskUpdateDto)
        {
            _logger.LogInformation("Updating task with ID {TaskId}.", id);

            var existingTask = await _taskRepository.GetTaskByIdAsync(id);
            if (existingTask == null)
            {
                _logger.LogWarning("Task with ID {TaskId} was not found.", id);
                throw new TaskNotFoundException(id);
            }

            var updatedTask = _mapper.Map(taskUpdateDto, existingTask);
            await _taskRepository.UpdateTaskAsync(updatedTask);
            var updatedTaskResponse = _mapper.Map<TaskResponseDto>(updatedTask);

            await _taskHubClient.NotifyTaskUpdated(updatedTaskResponse);

            return updatedTaskResponse;
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            _logger.LogInformation("Deleting task with ID {TaskId}.", taskId);

            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId);
            if (existingTask == null)
            {
                _logger.LogWarning("Task with ID {TaskId} was not found.", taskId);
                throw new TaskNotFoundException(taskId);
            }

            var result = await _taskRepository.DeleteTaskAsync(taskId);
            if (result)
            {
                await _taskHubClient.NotifyTaskDeleted(taskId);
            }

            return result;
        }
    }
}
