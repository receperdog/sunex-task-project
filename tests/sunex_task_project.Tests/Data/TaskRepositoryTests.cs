using Microsoft.EntityFrameworkCore;
using sunex_task_project.Data;
using sunex_task_project.Models;
using sunex_task_project.AppDataContext;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace sunex_task_project.Tests.Data
{
    public class TaskRepositoryTests
    {
        private readonly TaskRepository _taskRepository;
        private readonly AppDbContext _context;

        public TaskRepositoryTests()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TaskDb_{System.Guid.NewGuid()}")
                .Options;
            
            _context = new AppDbContext(options);
            _taskRepository = new TaskRepository(_context);
        }

        // This method will run before each test to avoid data leak
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose(); 
        }

        [Fact]
        public async Task GetTasksAsync_ShouldReturnAllTasks()
        {
            _context.Tasks.Add(new TaskItem { Title = "Test Task 1" });
            _context.Tasks.Add(new TaskItem { Title = "Test Task 2" });
            await _context.SaveChangesAsync();

            var tasks = await _taskRepository.GetTasksAsync();

            Assert.Equal(2, tasks.Count());
        }

        [Fact]
        public async Task GetTaskByIdAsync_TaskExists_ShouldReturnTask()
        {
            var task = new TaskItem { Title = "Test Task" };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var retrievedTask = await _taskRepository.GetTaskByIdAsync(task.TaskId);

            Assert.NotNull(retrievedTask);
            Assert.Equal(task.TaskId, retrievedTask.TaskId);
        }

        [Fact]
        public async Task GetTaskByIdAsync_TaskDoesNotExist_ShouldReturnNull()
        {
            var task = await _taskRepository.GetTaskByIdAsync(999);

            Assert.Null(task);
        }

        [Fact]
        public async Task AddTaskAsync_ShouldAddTask()
        {
            var task = new TaskItem { Title = "New Task" };

            var addedTask = await _taskRepository.AddTaskAsync(task);

            var retrievedTask = await _context.Tasks.FindAsync(addedTask.TaskId);
            Assert.NotNull(retrievedTask);
            Assert.Equal(task.Title, retrievedTask.Title);
        }

        [Fact]
        public async Task UpdateTaskAsync_TaskExists_ShouldUpdateTask()
        {
            var task = new TaskItem { Title = "Old Task" };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            task.Title = "Updated Task";

            var updatedTask = await _taskRepository.UpdateTaskAsync(task);

            Assert.NotNull(updatedTask);
            Assert.Equal("Updated Task", updatedTask.Title);
        }

        [Fact]
        public async Task UpdateTaskAsync_TaskDoesNotExist_ShouldReturnNull()
        {
            var task = new TaskItem { TaskId = 999, Title = "Non-existing Task" };

            var result = await _taskRepository.UpdateTaskAsync(task);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteTaskAsync_TaskExists_ShouldReturnTrue()
        {
            var task = new TaskItem { Title = "Task to Delete" };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var result = await _taskRepository.DeleteTaskAsync(task.TaskId);

            Assert.True(result);
            Assert.Null(await _context.Tasks.FindAsync(task.TaskId));
        }

        [Fact]
        public async Task DeleteTaskAsync_TaskDoesNotExist_ShouldReturnFalse()
        {
            var result = await _taskRepository.DeleteTaskAsync(999);
            Assert.False(result);
        }
    }
}
