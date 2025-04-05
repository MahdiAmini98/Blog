using Blog.Application.Contracts.Tags;
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
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _tagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IRepository<Tag> tagRepository, IUnitOfWork unitOfWork)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            return tags.Select(tag => new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                Slug = tag.Slug,
                CreatedDate = tag.CreatedDate
            });
        }

        public async Task<TagDto> GetTagByIdAsync(Guid id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
                throw new KeyNotFoundException($"Tag with ID {id} not found.");

            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                Slug = tag.Slug,
                CreatedDate = tag.CreatedDate
            };
        }

        public async Task<TagDto> CreateTagAsync(CreateTagRequest request)
        {
            var tag = new Tag(request.Name);

            _tagRepository.Add(tag);
            await _unitOfWork.CommitAsync();

            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                Slug = tag.Slug,
                CreatedDate = tag.CreatedDate
            };
        }

        public async Task UpdateTagAsync(Guid id, UpdateTagRequest request)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
                throw new KeyNotFoundException($"Tag with ID {id} not found.");

            tag.SetName(request.Name);

            _tagRepository.Update(tag);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteTagAsync(Guid id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
                throw new KeyNotFoundException($"Tag with ID {id} not found.");

            _tagRepository.Remove(tag);
            await _unitOfWork.CommitAsync();
        }
    }
}
