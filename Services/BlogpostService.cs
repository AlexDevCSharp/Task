using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Task.Model;

namespace Task.Services
{
    public class BlogpostService
    {
        public BlogpostService(BaseDbContext context)
        {
            _context = context;
        }
        private readonly BaseDbContext _context;

        private DbSet<Blogpost> Blogposts => _context.Blogposts;
        public List<Blogpost> GetAll()
        {
            return Blogposts.ToList();
        }

        public Blogpost GetBlogpostById(int id)
        {
            Blogpost blogpost = Blogposts.SingleOrDefault((Blogpost blogpost) => blogpost.Id == id);
            if (blogpost == null)
            {
                return null;
            }
            return blogpost;
        }

        public Blogpost AddNewBlogpost(Blogpost value)
        {
            var blogpost = ToEntity(value);
            if (blogpost == null) 
                return null;
            Blogposts.Add(blogpost);
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return null;
            }
            return blogpost;
        }

        public (Blogpost blogpost, Exception exception) UpdateBlogpost(Blogpost value)
        {
            Blogpost blogpost = Blogposts.SingleOrDefault((Blogpost blogpost) => blogpost.Id == value.Id);

            if (blogpost == null)
            {
                return (null, new ArgumentNullException($"blogpost with id: {value.Id} not found"));
            }

            if (value.Id != 0)
            {
                blogpost.Post = value.Post;
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return (null, new DbUpdateException($"Cannot save changes: {e.Message}"));
            }

            return (blogpost, null);
        }

        public (bool result, Exception exception) DeleteBlogpost(int id)
        {
            Blogpost blogpost = Blogposts.SingleOrDefault((Blogpost blogpost) => blogpost.Id == id);

            if (blogpost == null)
            {
                return (false, new ArgumentNullException($"Blogpost with id: {id} not found"));
            }

            EntityEntry<Blogpost> result = Blogposts.Remove(blogpost);

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

        public Blogpost ToEntity(Blogpost value)
        {
            return new Blogpost
            {
                Id=value.Id,
                Post=value.Post,
            };
        }
    }
}
