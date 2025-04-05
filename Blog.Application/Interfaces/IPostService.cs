using Blog.Application.Contracts.Posts;
using Blog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface IPostService
    {
        Task<PostDto> CreatePostAsync(CreatePostRequest request);
        Task<IEnumerable<PostDto>> GetAllPostsAsync(PostQueryParameters queryParameters);
        Task<PostDto> GetPostByIdAsync(Guid id);
        Task UpdatePostAsync(Guid id, UpdatePostRequest request);
        Task DeletePostAsync(Guid id);
    }
}
