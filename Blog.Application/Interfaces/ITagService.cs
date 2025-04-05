using Blog.Application.Contracts.Tags;
using Blog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllTagsAsync();
        Task<TagDto> GetTagByIdAsync(Guid id);
        Task<TagDto> CreateTagAsync(CreateTagRequest request);
        Task UpdateTagAsync(Guid id, UpdateTagRequest request);
        Task DeleteTagAsync(Guid id);
    }

}
