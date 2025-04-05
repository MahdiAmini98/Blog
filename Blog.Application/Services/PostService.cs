using Blog.Application.Contracts.Posts;
using Blog.Application.DTOs;
using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IRepository<Post> postRepository, IUnitOfWork unitOfWork)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PostDto> CreatePostAsync(CreatePostRequest request)
        {
            var post = Post.Create(request.Title, request.Content, request.AuthorId);
            _postRepository.Add(post);
            await _unitOfWork.CommitAsync();

            return new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                PublishedDate = post.PublishedDate
            };
        }


        public async Task<IEnumerable<PostDto>> GetAllPostsAsync(PostQueryParameters queryParameters)
        {
            var spec = new PostFilterSpecification(
                queryParameters.Tag,
                queryParameters.SearchText,
                queryParameters.FromDate,
                queryParameters.ToDate
            );

            var posts = await _postRepository.FindWithSpecificationAsync(spec);

            return posts.Select(post => new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                PublishedDate = post.PublishedDate
            });
        }




        public async Task<PostDto> GetPostByIdAsync(Guid id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
                throw new KeyNotFoundException($"Post with ID {id} not found.");

            return new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                PublishedDate = post.PublishedDate
            };
        }

        public async Task UpdatePostAsync(Guid id, UpdatePostRequest request)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
                throw new KeyNotFoundException($"Post with ID {id} not found.");

            post.SetTitle(request.Title);
            post.SetContent(request.Content);
            _postRepository.Update(post);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeletePostAsync(Guid id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
                throw new KeyNotFoundException($"Post with ID {id} not found.");

            _postRepository.Remove(post);
            await _unitOfWork.CommitAsync();
        }


    }
}
