using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Task.Model;

namespace Task.Services
{
    public class CommentService
    {
        public CommentService(BaseDbContext context)
        {
            _context = context;
        }
        private readonly BaseDbContext _context;

        private DbSet<Comment> Comments => _context.Comments;
        public List<Comment> GetAll()
        {
            return Comments.ToList();
        }

        public Comment GetCommentById(int id)
        {
            Comment comment = Comments.SingleOrDefault((Comment comment) => comment.Id == id);
            if (comment == null)
            {
                return null;
            }
            return comment;
        }
        

        public Comment AddNewComment(Comment value)
        {
            var comment = ToEntity(value);
            if (comment == null)
                return null;
            Comments.Add(comment);
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return null;
            }
            return comment;
        }

        public (Comment comment, Exception exception) UpdateComment(Comment value)
        {
            Comment comment = Comments.SingleOrDefault((Comment comment) => comment.Id == value.Id);

            if (comment == null)
            {
                return (null, new ArgumentNullException($"comment with id: {value.Id} not found"));
            }

            if (value.Id != 0)
            {
                comment.Text = value.Text;
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return (null, new DbUpdateException($"Cannot save changes: {e.Message}"));
            }

            return (comment, null);
        }

        public (bool result, Exception exception) DeleteComment(int id)
        {
            Comment comment = Comments.SingleOrDefault((Comment comment) => comment.Id == id);

            if (comment == null)
            {
                return (false, new ArgumentNullException($"Comment with id: {id} not found"));
            }

            EntityEntry<Comment> result = Comments.Remove(comment);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return (false, new DbUpdateException($"Cannot save changes: {e.Message}"));
            }

            return (result.State == EntityState.Deleted, null);
        }

        public Comment ToEntity(Comment value)
        {
            return new Comment
            {
                Id = value.Id,
                Text = value.Text,
            };
        }

        
    }
}
