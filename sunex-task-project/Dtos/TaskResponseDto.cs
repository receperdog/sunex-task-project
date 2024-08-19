namespace sunex_task_project.Dtos
{
    public class TaskResponseDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool Completed { get; set; }
    }
}
