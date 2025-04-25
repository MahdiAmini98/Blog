namespace Blog.PanelAdmin.Models.Categories
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
    public class CreateCategoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class UpdateCategoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
