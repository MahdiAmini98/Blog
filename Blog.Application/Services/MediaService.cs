using Blog.Application.Contracts.Medias;
using Blog.Application.DTOs;
using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Services
{
    public class MediaService : IMediaService
    {
        private readonly IRepository<Media> _mediaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepository;

        public MediaService(
            IRepository<Media> mediaRepository,
            IUnitOfWork unitOfWork,
            IRepository<User> userRepository)
        {
            _mediaRepository = mediaRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<MediaDto> UploadMediaAsync(UploadMediaRequest request)
        {
            // واکشی کاربر بر اساس UploadedById
            var user = await _userRepository.GetByIdAsync(request.UploadedBy);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.UploadedBy} not found.");

            // ایجاد یک موجودیت مدیا
            var media = new Media(request.Url, request.Type, user);

            _mediaRepository.Add(media);
            await _unitOfWork.CommitAsync();

            return new MediaDto
            {
                Id = media.Id,
                Url = media.Url,
                Type = media.Type,
                UploadedById = media.UploadedById,
                UploadDate = media.CreatedDate
            };
        }

        public async Task<IEnumerable<MediaDto>> GetAllMediaAsync(int page, int pageSize)
        {

            var mediaList = await _mediaRepository.GetPagedAsync(page, pageSize);

            return mediaList.Select(media => new MediaDto
            {
                Id = media.Id,
                Url = media.Url,
                Type = media.Type,
                UploadedById = media.UploadedById,
                UploadDate = media.CreatedDate
            });
        }

        public async Task<MediaDto> GetMediaByIdAsync(Guid id)
        {
            var media = await _mediaRepository.GetByIdAsync(id);
            if (media == null)
                throw new KeyNotFoundException($"Media with ID {id} not found.");

            return new MediaDto
            {
                Id = media.Id,
                Url = media.Url,
                Type = media.Type,
                UploadedById = media.UploadedById,
                UploadDate = media.CreatedDate
            };
        }

        public async Task DeleteMediaAsync(Guid id)
        {
            var media = await _mediaRepository.GetByIdAsync(id);
            if (media == null)
                throw new KeyNotFoundException($"Media with ID {id} not found.");

            _mediaRepository.Remove(media);
            await _unitOfWork.CommitAsync();
        }
    }
}
