using Blog.Application.DTOs;
using Blog.Application.DTOs.Posts.BlogUIPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces.BlogUIService
{
    public interface IBlogSidebarService
    {
        Task<List<BlogUIPostListDto>> GetPopularPostsAsync();
        Task<List<CategoryDto>> GetCategoriesAsync();
        Task<List<TagDto>> GetTagsAsync();
    }
}
