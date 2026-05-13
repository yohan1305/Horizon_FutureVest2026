using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities;
using Persistence.Repositories.Interfaz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class PaisRepository : IRepositorio<Pais>
    {
        private readonly FutureVestContext _context;

        public PaisRepository(FutureVestContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Pais entity)
        {
            await _context.Set<Pais>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<Pais>().FindAsync(id);
            if(entity != null)
            {
                _context.Set<Pais>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Pais>> GetAllAsync()
        {
            return await _context.Set<Pais>().ToListAsync();
        }

        public async Task<Pais?> GetByIdAsync(int id)
        {
            return await _context.Set<Pais>().FindAsync(id);
        }

        public async Task UpdateAsync(Pais entity)
        {
            var entry = await _context.Set<Pais>().FindAsync(entity.Id);
            if(entry != null)
            {
                _context.Entry(entry).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
