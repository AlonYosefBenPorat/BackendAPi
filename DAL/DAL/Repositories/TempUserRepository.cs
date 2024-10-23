using System.Threading.Tasks;
using DAL.Models.UsersModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using DAL.Data;

namespace DAL.Repositories
{
    public class TempUserRepository : ITempUserRepository
    {
        private readonly WebAppContext _context;

        public TempUserRepository(WebAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppUsersTemp>> GetTempUsersAsync()
        {
            return await _context.AppUsersTemps.ToListAsync();
        }

        public async Task<AppUsersTemp?> GetTempUserByIdAsync(Guid id)
        {
            return await _context.AppUsersTemps.FindAsync(id);
        }

        public async Task<bool> CreateTempUserAsync(AppUsersTemp tempUser, string password)
        {
            if (tempUser == null) throw new ArgumentNullException(nameof(tempUser));
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Password cannot be null or empty", nameof(password));

            await _context.AppUsersTemps.AddAsync(tempUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTempUserIsImportAsync(Guid id, bool isImport)
        {
            var tempUser = await _context.AppUsersTemps.FindAsync(id);
            if (tempUser is null)
            {
                return false;
            }

            tempUser.IsImport = isImport;
            _context.AppUsersTemps.Update(tempUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTempUserAsync(Guid id)
        {
            var tempUser = await _context.AppUsersTemps.FindAsync(id);
            if (tempUser is null)
            {
                return false;
            }

            _context.AppUsersTemps.Remove(tempUser);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
