using Xunit;
using Moq;
using AutoMapper;
using sunex_task_project.Services;
using sunex_task_project.Models;
using sunex_task_project.Data;
using sunex_task_project.Dtos;
using sunex_task_project.Exceptions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sunex_task_project.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly TaskService _taskService;
        // Following dependencies are mock dependencies to test service class.
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<TaskService>> _mockLogger;

        public TaskServiceTests()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<TaskService>>();
            // init task service with mock dependencies
            _taskService = new TaskService(_mockTaskRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetTasksAsync_ShouldReturnTasks()
        {
            var tasks = new List<TaskItem>
            {
                new TaskItem { TaskId = 1, Title = "Test Task 1" },
                new TaskItem { TaskId = 2, Title = "Test Task 2" }
            };
            // return tasks when call repo.GetTasksAsync()
            _mockTaskRepository.Setup(repo => repo.GetTasksAsync()).ReturnsAsync(tasks);
            _mockMapper.Setup(m => m.Map<IEnumerable<TaskResponseDto>>(It.IsAny<IEnumerable<TaskItem>>()))
                       .Returns(new List<TaskResponseDto> { new TaskResponseDto { TaskId = 1 }, new TaskResponseDto { TaskId = 2 } });

            var result = await _taskService.GetTasksAsync();
            // be sure actual count is correct
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetTaskByIdAsync_TaskExists_ShouldReturnTask()
        {
            var task = new TaskItem { TaskId = 1, Title = "Test Task 1" };
            // return above task object when call repo.GetTaskByIdAsync(1)
            _mockTaskRepository.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(task);
            _mockMapper.Setup(m => m.Map<TaskResponseDto>(It.IsAny<TaskItem>())).Returns(new TaskResponseDto { TaskId = 1 });

            var result = await _taskService.GetTaskByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.TaskId);
        }

        [Fact]
        public async Task GetTaskByIdAsync_TaskDoesNotExist_ShouldThrowTaskNotFoundException()
        {
            // return null when call repo.GetTaskByIdAsync(1)
            _mockTaskRepository.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync((TaskItem)null);
            // be sure about throwing TaskNotFoundException
            await Assert.ThrowsAsync<TaskNotFoundException>(() => _taskService.GetTaskByIdAsync(1));
        }

        [Fact]
        public async Task AddTaskAsync_ValidTask_ShouldReturnCreatedTask()
        {
            var taskRequestDto = new TaskRequestDto { Title = "New Task", Completed = false };
            var taskItem = new TaskItem { TaskId = 1, Title = "New Task", Completed = false };
            var taskResponseDto = new TaskResponseDto { TaskId = 1, Title = "New Task", Completed = false };
            
            // arrange mapping process for request and response
            _mockTaskRepository.Setup(repo => repo.GetTasksAsync()).ReturnsAsync(new List<TaskItem>());
            _mockMapper.Setup(m => m.Map<TaskItem>(taskRequestDto)).Returns(taskItem);
            _mockTaskRepository.Setup(repo => repo.AddTaskAsync(taskItem)).ReturnsAsync(taskItem);
            _mockMapper.Setup(m => m.Map<TaskResponseDto>(taskItem)).Returns(taskResponseDto);

            var result = await _taskService.AddTaskAsync(taskRequestDto);

            Assert.NotNull(result);
            Assert.Equal(1, result.TaskId);
            Assert.Equal(taskRequestDto.Title, result.Title);
        }

        [Fact]
        public async Task UpdateTaskAsync_TaskExists_ShouldReturnUpdatedTask()
        {
            var taskUpdateDto = new TaskUpdateDto { Title = "Updated Task", Completed = true };
            var existingTask = new TaskItem { TaskId = 1, Title = "Old Task", Completed = false };
            var updatedTask = new TaskItem { TaskId = 1, Title = "Updated Task", Completed = true };
            var updatedTaskResponse = new TaskResponseDto { TaskId = 1, Title = "Updated Task", Completed = true };

            _mockTaskRepository.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(existingTask);
            _mockMapper.Setup(m => m.Map(taskUpdateDto, existingTask)).Returns(updatedTask);
            _mockTaskRepository.Setup(repo => repo.UpdateTaskAsync(updatedTask)).ReturnsAsync(updatedTask);
            _mockMapper.Setup(m => m.Map<TaskResponseDto>(updatedTask)).Returns(updatedTaskResponse);

            var result = await _taskService.UpdateTaskAsync(1, taskUpdateDto);
            // check actual and expected values
            Assert.NotNull(result);
            Assert.Equal(1, result.TaskId);
            Assert.Equal("Updated Task", result.Title);
        }

        [Fact]
        public async Task UpdateTaskAsync_TaskDoesNotExist_ShouldThrowTaskNotFoundException()
        {
            var taskUpdateDto = new TaskUpdateDto { Title = "Updated Task", Completed = true };
            // return null when call repo.GetTaskByIdAsync(1)
            _mockTaskRepository.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync((TaskItem)null);
            // be sure about throwing TaskNotFoundException 
            await Assert.ThrowsAsync<TaskNotFoundException>(() => _taskService.UpdateTaskAsync(1, taskUpdateDto));
        }

        [Fact]
        public async Task DeleteTaskAsync_TaskExists_ShouldReturnTrue()
        {
            var existingTask = new TaskItem { TaskId = 1, Title = "Test Task" };
            _mockTaskRepository.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(existingTask);
            _mockTaskRepository.Setup(repo => repo.DeleteTaskAsync(1)).ReturnsAsync(true);
 
            var result = await _taskService.DeleteTaskAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteTaskAsync_TaskDoesNotExist_ShouldThrowTaskNotFoundException()
        {
            // return null object when call repo.GetTaskByIdAsync(1)
            _mockTaskRepository.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync((TaskItem)null);
            // be sure about throwing TaskNotFoundException 
            await Assert.ThrowsAsync<TaskNotFoundException>(() => _taskService.DeleteTaskAsync(1));
        }
    }
}
