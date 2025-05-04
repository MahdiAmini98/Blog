using System.ComponentModel.DataAnnotations;

namespace Blog.PanelAdmin.Models.Posts
{
    public class BasePostRequestDto
    {
        [Required(ErrorMessage = "عنوان پست الزامی است.")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "عنوان باید بین 5 تا 150 کاراکتر باشد.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "محتوای پست نباید خالی باشد.")]
        [MinLength(20, ErrorMessage = "محتوا باید حداقل 20 کاراکتر داشته باشد.")]
        public string Content { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "خلاصه نباید بیش از 300 کاراکتر باشد.")]
        public string? Summary { get; set; }

        [Url(ErrorMessage = "آدرس تصویر معتبر نیست.")]
        public string? ThumbnailUrl { get; set; }

        [Required(ErrorMessage = "وضعیت پست باید مشخص شود.")]
        public PostStatus Status { get; set; } = PostStatus.Draft;

        [Required(ErrorMessage = "متا تگ عنوان الزامی است.")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "متا تگ عنوان باید بین 10 تا 100 کاراکتر باشد.")]
        public string MetaTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "متا تگ توضیحات الزامی است.")]
        [StringLength(300, MinimumLength = 30, ErrorMessage = "متا تگ توضیحات باید بین 30 تا 300 کاراکتر باشد.")]
        public string MetaDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug نباید خالی باشد.")]
        public string Slug { get; set; } = string.Empty;

        [Required(ErrorMessage = "حداقل یک دسته‌بندی باید انتخاب شود.")]
        [MinLength(1, ErrorMessage = "حداقل باید یک دسته‌بندی انتخاب شود.")]
        public List<Guid> CategoryIds { get; set; } = new();

        [Required(ErrorMessage = "حداقل یک برچسب باید انتخاب شود.")]
        [MinLength(1, ErrorMessage = "حداقل باید یک برچسب انتخاب شود.")]
        public List<Guid> TagIds { get; set; } = new();
    }


    public class CreatePostRequestDto : BasePostRequestDto
    {
    }

    public class UpdatePostRequestDto : BasePostRequestDto
    {
        [Required]
        public Guid Id { get; set; }
    }
    public enum PostStatus
    {
        [Display(Name = "پیش‌نویس")]
        Draft = 0,

        [Display(Name = "منتشر شده")]
        Published = 1,

        [Display(Name = "آرشیو شده")]
        Archived = 2
    }
}
