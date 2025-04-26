using Blog.Application.Contracts.Medias;
using Blog.Application.DTOs;
using Blog.Application.Interfaces;
using Blog.Application.Interfaces.FileStorage;
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
        private readonly IRepository<User> _userRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public MediaService(IRepository<Media> mediaRepository,
            IRepository<User> userRepository,
            IFileStorageService fileStorageService, IUnitOfWork unitOfWork)
        {
            _mediaRepository = mediaRepository;
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<MediaDto> UploadMediaAsync(UploadMediaRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Url))
            {
                throw new ArgumentException("File URL cannot be empty.", nameof(request.Url));
            }

            if (string.IsNullOrWhiteSpace(request.Type))
            {
                throw new ArgumentException("File type cannot be empty.", nameof(request.Type));
            }

            // ذخیره فایل و دریافت URL جدید (در صورت نیاز)
            string fileUrl = request.Url;
            if (!(await _fileStorageService.FileExistsAsync(request.Url)))
            {
                throw new FileNotFoundException("File not found on storage.", request.Url);
            }

            // جستجوی کاربر آپلودکننده در دیتابیس
            var user = await _userRepository.GetByIdAsync(request.UploadedBy);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // ذخیره در دیتابیس
            var media = new Media(fileUrl, request.Type, user);
            _mediaRepository.Add(media);
            await _unitOfWork.CommitAsync();

            return new MediaDto
            {
                Id = media.Id,
                Url = $"{_fileStorageService.GetStoragePath()}{media.Url}",
                Type = media.Type,
                UploadedById = media.UploadedById,
                UploadDate = media.CreatedDate
            };
        }

        public async Task<PaginatedList<MediaDto>> GetAllMediaAsync(int page, int pageSize, string? fileType)
        {
            var mediaList = await _mediaRepository.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(fileType))
            {
                mediaList = mediaList.Where(p => p.Type == fileType);
            }
            int totalCount = mediaList.Count();

            var paginatedItems = mediaList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MediaDto
                {
                    Id = m.Id,
                    Url = $"{_fileStorageService.GetStoragePath()}{m.Url}",
                    Type = m.Type,
                    UploadedById = m.UploadedById,
                    UploadDate = m.CreatedDate
                })
                .ToList();

            return new PaginatedList<MediaDto>(paginatedItems, totalCount, page, pageSize);
        }


        public async Task<MediaDto> GetMediaByIdAsync(Guid id)
        {
            var media = await _mediaRepository.GetByIdAsync(id);
            if (media == null)
            {
                throw new KeyNotFoundException($"Media with ID {id} not found.");
            }

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
            {
                throw new KeyNotFoundException($"Media with ID {id} not found.");
            }

            await _fileStorageService.DeleteFileAsync(media.Url);
            _mediaRepository.Remove(media);
            await _unitOfWork.CommitAsync();
        }
    }
}
