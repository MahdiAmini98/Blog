using Blog.Domain.Base;
using Blog.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
    public class Post : EntityBase
    {
        public string Title { get; private set; }
        public string Slug { get; private set; }
        public string Content { get; private set; }
        public string? Summary { get; private set; }
        public string? ThumbnailUrl { get; private set; }
        public DateTime PublishedDate { get; private set; }
        public int ViewCount { get; private set; }
        public Guid AuthorId { get; private set; }
        public PostStatus Status { get; private set; }
        public string? MetaTitle { get; private set; }
        public string? MetaDescription { get; private set; }


        private readonly List<Category> _categories = new();
        public IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();

        private readonly List<Tag> _tags = new();
        public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();

        private readonly List<Comment> _comments = new();
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

        private readonly List<Reaction> _reactions = new();
        public IReadOnlyCollection<Reaction> Reactions => _reactions.AsReadOnly();

        public User Author { get; private set; }


        private Post(string title, string slug, string content, string metaTitle, string metaDescription, Guid authorId)
        {
            SetTitle(title);
            SetContent(content);
            SetMetaTitle(metaTitle);
            SetMetaDescription(metaDescription);
            Slug = slug;
            AuthorId = authorId;
            Status = PostStatus.Draft;
            PublishedDate = DateTime.UtcNow;
        }

        public static Post Create(string title, string content, string metaTitle, string metaDescription, Guid authorId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be empty.", nameof(content));
            }

            if (string.IsNullOrWhiteSpace(metaTitle))
            {
                throw new ArgumentException("Meta title cannot be empty.", nameof(metaTitle));
            }

            if (metaTitle.Length > 100)
            {
                throw new ArgumentException("Meta title cannot exceed 100 characters.", nameof(metaTitle));
            }

            if (string.IsNullOrWhiteSpace(metaDescription))
            {
                throw new ArgumentException("Meta description cannot be empty.", nameof(metaDescription));
            }

            if (metaDescription.Length > 300)
            {
                throw new ArgumentException("Meta description cannot exceed 300 characters.", nameof(metaDescription));
            }

            var slug = GenerateSlug(title);
            var post = new Post(title, slug, content, metaTitle, metaDescription, authorId);

            return post;
        }

        public void SetMetaTitle(string metaTitle)
        {
            if (string.IsNullOrWhiteSpace(metaTitle))
            {
                throw new ArgumentException("Meta title cannot be empty.", nameof(metaTitle));
            }

            if (metaTitle.Length > 100)
            {
                throw new ArgumentException("Meta title cannot exceed 100 characters.", nameof(metaTitle));
            }

            MetaTitle = metaTitle;
        }

        public void SetMetaDescription(string metaDescription)
        {
            if (string.IsNullOrWhiteSpace(metaDescription))
            {
                throw new ArgumentException("Meta description cannot be empty.", nameof(metaDescription));
            }

            if (metaDescription.Length > 300)
            {
                throw new ArgumentException("Meta description cannot exceed 300 characters.", nameof(metaDescription));
            }

            MetaDescription = metaDescription;
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            }

            Title = title;
        }

        public void SetSlug(string slug)
        {
            Slug = GenerateSlug(slug);
        }


        private static string GenerateSlug(string input)
        {
            input = input.ToLower().Trim();
            input = Regex.Replace(input, @"[^a-z0-9\s-]", "");
            input = Regex.Replace(input, @"\s+", "-");
            return input;
        }



        public void SetContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be empty.", nameof(content));
            }

            Content = content;
        }


        public void SetSummary(string summary)
        {
            Summary = summary?.Length > 500
                ? throw new ArgumentException("Summary cannot exceed 500 characters.", nameof(summary))
                : summary;
        }


        public void SetThumbnailUrl(string thumbnailUrl)
        {
            if (!string.IsNullOrWhiteSpace(thumbnailUrl) && !Uri.IsWellFormedUriString(thumbnailUrl, UriKind.Absolute))
            {
                throw new ArgumentException("Invalid thumbnail URL.", nameof(thumbnailUrl));
            }

            ThumbnailUrl = thumbnailUrl;
        }


        public void ChangeStatus(PostStatus newStatus)
        {
            Status = newStatus;
        }


        public void IncrementViewCount()
        {
            ViewCount++;
        }


        public void AddCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");
            }

            if (_categories.Contains(category))
            {
                throw new InvalidOperationException("Category is already assigned to this post.");
            }

            _categories.Add(category);
        }

        public void RemoveCategory(Category category)
        {
            if (category == null || !_categories.Contains(category))
            {
                throw new InvalidOperationException("Category is not assigned to this post.");
            }

            _categories.Remove(category);
        }

        public void AddTag(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag), "Tag cannot be null.");
            }

            if (_tags.Contains(tag))
            {
                throw new InvalidOperationException("Tag is already assigned to this post.");
            }

            _tags.Add(tag);
        }

        public void RemoveTag(Tag tag)
        {
            if (tag == null || !_tags.Contains(tag))
            {
                throw new InvalidOperationException("Tag is not assigned to this post.");
            }
            _tags.Remove(tag);
        }


        public void AddComment(Comment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment), "Comment cannot be null.");
            }

            _comments.Add(comment);
        }



        public void UpdateCategories(IEnumerable<Category> newCategories)
        {
            _categories.Clear();
            foreach (var category in newCategories)
            {
                AddCategory(category);
            }
        }
        public void UpdateTags(IEnumerable<Tag> newTags)
        {
            _tags.Clear();
            foreach (var tag in newTags)
            {
                AddTag(tag);
            }
        }


    }



}
