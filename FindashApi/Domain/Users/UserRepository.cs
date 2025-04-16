using Findash.Users.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Findash.Users;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task <User?> GetById(Guid id)
    {
        return await dbContext.Users.FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<IEnumerable<User>> GetAll(int? page = 1, int? numberOfRecords = 100, string? contains = "")
    {
        var pageNumber = page ?? 1;
        var recordsPerPage = numberOfRecords ?? 10;
        
        var query = dbContext.Users
            .Skip(((pageNumber - 1) * recordsPerPage))
            .Take(recordsPerPage);
        
        if (!string.IsNullOrWhiteSpace(contains))
        {
            query = query.Where(e => e.FirstName.ToLower().Contains(contains.ToLower()));
        }
        
        return await query.ToArrayAsync();
    }

    public async Task Create(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(User existingUser)
    {
        dbContext.Entry(existingUser).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(User user)
    {
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
    }
}