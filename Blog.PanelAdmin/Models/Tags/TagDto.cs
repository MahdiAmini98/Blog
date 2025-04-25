namespace Blog.PanelAdmin.Models.Tags
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CreateTagRequestDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateTagRequestDto
    {
        public string Name { get; set; } 
    }
}
