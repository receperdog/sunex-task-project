namespace sunex_task_project.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(int taskId)
            : base($"Task with ID {taskId} was not found.")
        {
        }

        public TaskNotFoundException(string message) : base(message)
        {
        }

        public TaskNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
