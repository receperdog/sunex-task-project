using AutoMapper;
using sunex_task_project.Data;
using sunex_task_project.Dtos;
using sunex_task_project.Exceptions;
using sunex_task_project.Models;
using Microsoft.Extensions.Logging;

namespace sunex_task_project.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository taskRepository, IMapper mapper, ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _logger = logger;
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

            if (string.IsNullOrWhiteSpace(taskRequestDto.Title))
            {
                _logger.LogError("Task Title is required.");
                throw new ArgumentException("Task Title is required.");
            }

            var task = _mapper.Map<TaskItem>(taskRequestDto);

            var createdTask = await _taskRepository.AddTaskAsync(task);
            return _mapper.Map<TaskResponseDto>(createdTask);
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

            // Map the update DTO to the existing task entity
            var updatedTask = _mapper.Map(taskUpdateDto, existingTask);

            await _taskRepository.UpdateTaskAsync(updatedTask);

            return _mapper.Map<TaskResponseDto>(updatedTask);
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

            return await _taskRepository.DeleteTaskAsync(taskId);
        }
    }
}
