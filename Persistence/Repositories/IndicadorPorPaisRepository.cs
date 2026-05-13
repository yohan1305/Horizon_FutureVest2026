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
    public class IndicadorPorPaisRepository : IRepositorio<IndicadorPorPais> 
    {
        private readonly FutureVestContext _context;

        public IndicadorPorPaisRepository(FutureVestContext context)
        {
            _context = context;
        }

        public async Task AddAsync(IndicadorPorPais entity)
        {
            await _context.Set<IndicadorPorPais>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<IndicadorPorPais>().FindAsync(id);
            if(entity != null)
            {
                _context.Set<IndicadorPorPais>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<IndicadorPorPais>> GetAllAsync()
        {
            return await _context.Set<IndicadorPorPais>().ToListAsync();
        }

        public async Task<IndicadorPorPais?> GetByIdAsync(int id)
        {
            return await _context.Set<IndicadorPorPais>().FindAsync(id);
        }

        public async Task UpdateAsync(IndicadorPorPais entity)
        {
            var entry = await _context.Set<IndicadorPorPais>().FindAsync(entity.Id);
            if(entry != null)
            {
                _context.Entry(entry).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
