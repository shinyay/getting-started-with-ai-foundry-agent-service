using Microsoft.EntityFrameworkCore;
using ReadingExperience.Core.Entities;
using ReadingExperience.Core.Interfaces;
using ReadingExperience.Infrastructure.Data;

namespace ReadingExperience.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ReadingExperienceDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> IsEmailTakenAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email);
    }
}