using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Task.Model;
using Task.Entities;
using System.Security.Cryptography;

namespace Task.Services
{
    public class ProfileService
    {
        public ProfileService(BaseDbContext context)
        {
            _context = context;
        }
        private readonly BaseDbContext _context;

        private DbSet<Profile> Profiles => _context.Profiles;
        private DbSet<User> Users => _context.Users;
        public List<User> GetAllUsers()
        {
            return Users.ToList();
        }

        public User GetUserByUsername(string username)
        {
            User user = Users.SingleOrDefault((User user) => user.Username == username);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public User GetUserById(int id)
        {
            User user = Users.SingleOrDefault((User user) => user.Id == id);
            if (user == null)
            {
                return null;
            }
            return user;
        }
        public List<Profile> GetAllProfiles()
        {
            return Profiles.ToList();
        }

        public Profile GetProfileById(int id)
        {
            Profile profile = Profiles.SingleOrDefault((Profile profile) => profile.Id == id);
            if (profile == null)
            {
                return null;
            }
            return profile;
        }

        public Profile AddNewProfile(Profile value)
        {
            var profile = ToEntityProfile(value);
            if (profile == null)
                return null;
            Profiles.Add(profile);
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return null;
            }
            var user = ToEntityUser(value);
            if (user == null)
                return null;
            Users.Add(user);
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return null;
            }
            return profile;
        }

        public (Profile profile, Exception exception) UpdateProfile(Profile value)
        {
            Profile profile = Profiles.SingleOrDefault((Profile profile) => profile.Id == value.Id);

            if (profile == null)
            {
                return (null, new ArgumentNullException($"profile with id: {value.Id} not found"));
            }

            if (value.Id != 0)
            {
                profile.FirstName = value.FirstName;
                profile.LastName = value.LastName;
                profile.Email = value.Email;
                profile.Username = value.Username;
                profile.Password = value.Password;
                profile.Password = value.Password;
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return (null, new DbUpdateException($"Cannot save changes: {e.Message}"));
            }

            return (profile, null);
        }

        public (bool result, Exception exception) DeleteProfile(int id)
        {
            Profile profile = Profiles.SingleOrDefault((Profile profile) => profile.Id == id);

            if (profile == null)
            {
                return (false, new ArgumentNullException($"Profile with id: {id} not found"));
            }

            EntityEntry<Profile> result = Profiles.Remove(profile);

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

        public Profile ToEntityProfile(Profile value)
        {
            return new Profile
            {
                Id = value.Id,
                FirstName = value.FirstName,
                LastName = value.LastName,
                Email = value.Email,
                Username = value.Username,
                Password = value.Password,
            };
        }

        public User ToEntityUser(Profile value)
        {
            return new User
            {
                Id = value.Id,
                Username = value.Username,
                Password = value.Password,
                ApiKey = GenerateApiKey(),
            };
        }

        public User ToEntityUserByAuth(User value)
        {
            return new User
            {
                Id = value.Id,
                Username = value.Username,
                Password = value.Password,
                ApiKey = GenerateApiKey(),
            };
        }

        public string GenerateApiKey()
        {
            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);
            string apiKey = Convert.ToBase64String(key);
            return apiKey;
        }

        public User Authorization(User value)
        {
            Profile profile = null;
            var user = Users.SingleOrDefault(x => x.Username == value.Username && x.Password == value.Password);
            // return null if user not found
            if (user == null)
                return null;
            if (user != null)
            {
                var data = Profiles.SingleOrDefault(x => x.Username == value.Username);
                if (data != null)
                    profile = ToEntityProfile(data);
            }
            user = ToEntityUserByAuth(value);
            Users.Add(user);
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return null;
            }
            return user;
        }
    }
}
