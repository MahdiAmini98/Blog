namespace Blog.PanelAdmin.Models.Medias
{
    public class MediaDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public Guid UploadedById { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
