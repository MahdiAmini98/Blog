using Blog.Application.Contracts.Medias;
using Blog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface IMediaService
    {
        Task<MediaDto> UploadMediaAsync(UploadMediaRequest request);
        Task<IEnumerable<MediaDto>> GetAllMediaAsync(int page, int pageSize);
        Task<MediaDto> GetMediaByIdAsync(Guid id);
        Task DeleteMediaAsync(Guid id);
    }
}
