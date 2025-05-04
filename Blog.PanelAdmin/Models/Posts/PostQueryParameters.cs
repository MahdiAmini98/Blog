﻿namespace Blog.PanelAdmin.Models.Posts
{
    public class PostQueryParameters
    {
        public string? Tag { get; set; }
        public string? SearchText { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
