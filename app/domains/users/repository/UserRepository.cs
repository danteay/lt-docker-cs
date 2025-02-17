using app.infra;
using Microsoft.EntityFrameworkCore;

namespace app.domains.users.repository
{
    public class UserRepository(PostgresDbContext dbContext) : IUserRepository
    {
        public async Task<List<User>> GetAllAsync()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await dbContext.Users.FindAsync(id);
        }

        public async Task<User> CreateAsync(User user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(int id, User user)
        {
            var existingUser = await dbContext.Users.FindAsync(id);
            if (existingUser == null)
                return null;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            await dbContext.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
                return false;

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
