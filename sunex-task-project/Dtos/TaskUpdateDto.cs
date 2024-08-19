using System.ComponentModel.DataAnnotations;

namespace sunex_task_project.Dtos
{
    public class TaskUpdateDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool Completed { get; set; }
    }
}
