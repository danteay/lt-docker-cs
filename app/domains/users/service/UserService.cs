using app.domains.users.repository;

namespace app.domains.users.service
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<List<User>> GetAllAsync()
        {
            return await userRepository.GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await userRepository.GetByIdAsync(id);
        }

        public async Task<User> CreateAsync(User user)
        {
            return await userRepository.CreateAsync(user);
        }

        public async Task<User?> UpdateAsync(int id, User user)
        {
            return await userRepository.UpdateAsync(id, user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await userRepository.DeleteAsync(id);
        }
    }
}
