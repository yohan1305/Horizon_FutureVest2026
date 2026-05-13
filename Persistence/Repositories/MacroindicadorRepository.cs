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
    public class MacroindicadorRepository : IRepositorio<Macroindicador> 
    {
        private readonly FutureVestContext _context;

        public MacroindicadorRepository(FutureVestContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Macroindicador entity)
        {
            await _context.Set<Macroindicador>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<Macroindicador>().FindAsync(id);
            if(entity != null)
            {
                _context.Set<Macroindicador>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Macroindicador>> GetAllAsync()
        {
            return await _context.Set<Macroindicador>().ToListAsync();
        }

        public async Task<Macroindicador?> GetByIdAsync(int id)
        {
            return await _context.Set<Macroindicador>().FindAsync(id);
        }

        public async Task UpdateAsync(Macroindicador entity)
        {
            var entry = await _context.Set<Macroindicador>().FindAsync(entity.Id);
            if(entry != null)
            {
                _context.Entry(entry).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
