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
    public class ConfiguracionRetornoRepository : IRepositorio<ConfiguracionRetorno>
    {
        private readonly FutureVestContext _context;

        public ConfiguracionRetornoRepository(FutureVestContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ConfiguracionRetorno entity)
        {
            await _context.Set<ConfiguracionRetorno>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<ConfiguracionRetorno>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<ConfiguracionRetorno>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ConfiguracionRetorno>> GetAllAsync()
        {
            return await _context.Set<ConfiguracionRetorno>().ToListAsync();
        }

        public async Task<ConfiguracionRetorno?> GetByIdAsync(int id)
        {
            return await _context.Set<ConfiguracionRetorno>().FindAsync(id);
        }

        public async Task UpdateAsync(ConfiguracionRetorno entity)
        {
            var entry = await _context.Set<ConfiguracionRetorno>().FindAsync(entity.Id);
            if (entry != null)
            {
                _context.Entry(entry).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(ConfiguracionRetorno entity)
        {
            await _context.ConfiguracionRetorno.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

    }
}
