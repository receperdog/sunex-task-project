using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using sunex_task_project.Controllers;
using sunex_task_project.Dtos;
using sunex_task_project.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sunex_task_project.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly TasksController _tasksController;
        private readonly Mock<ITaskService> _mockTaskService;

        public TasksControllerTests()
        {
            _mockTaskService = new Mock<ITaskService>();
            _tasksController = new TasksController(_mockTaskService.Object);
        }

        [Fact]
        public async Task GetTasks_ReturnsOkResult_WithListOfTasks()
        {
            var taskList = new List<TaskResponseDto>
            {
                new TaskResponseDto { TaskId = 1, Title = "Task 1", Completed = false },
                new TaskResponseDto { TaskId = 2, Title = "Task 2", Completed = true }
            };

            _mockTaskService.Setup(service => service.GetTasksAsync()).ReturnsAsync(taskList);

            var result = await _tasksController.GetTasks();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TaskResponseDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetTask_ReturnsOkResult_WithSingleTask()
        {
            var taskId = 1;
            var task = new TaskResponseDto { TaskId = taskId, Title = "Task 1", Completed = false };

            _mockTaskService.Setup(service => service.GetTaskByIdAsync(taskId)).ReturnsAsync(task);

            var result = await _tasksController.GetTask(taskId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<TaskResponseDto>(okResult.Value);
            Assert.Equal(taskId, returnValue.TaskId);
        }

        [Fact]
        public async Task CreateTask_ReturnsCreatedAtAction_WithNewTask()
        {
            var taskRequest = new TaskRequestDto { Title = "New Task", Description = "New Task Description", Completed = false };
            var createdTask = new TaskResponseDto { TaskId = 1, Title = "New Task", Completed = false };

            _mockTaskService.Setup(service => service.AddTaskAsync(taskRequest)).ReturnsAsync(createdTask);

            var result = await _tasksController.CreateTask(taskRequest);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<TaskResponseDto>(createdAtActionResult.Value);
            Assert.Equal("New Task", returnValue.Title);
            Assert.Equal(1, returnValue.TaskId);
        }

        [Fact]
        public async Task UpdateTask_ReturnsNoContentResult_WhenTaskIsUpdated()
        {
            var taskId = 1;
            var taskUpdate = new TaskUpdateDto { Title = "Updated Task", Description = "Updated Description", Completed = true };

            _mockTaskService.Setup(service => service.UpdateTaskAsync(taskId, taskUpdate)).ReturnsAsync(new TaskResponseDto { TaskId = taskId, Title = "Updated Task", Completed = true });

            var result = await _tasksController.UpdateTask(taskId, taskUpdate);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNoContentResult_WhenTaskIsDeleted()
        {
            var taskId = 1;
            _mockTaskService.Setup(service => service.DeleteTaskAsync(taskId)).ReturnsAsync(true);

            var result = await _tasksController.DeleteTask(taskId);
            Assert.IsType<NoContentResult>(result);
        }
    }
}