using Bot.Application.Interfaces.DbInterfaces;
using Bot.Application.Interfaces.UseCaseInterfaces;
using Bot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application.Services.UseCases
{
    public class FilialServices : IFilialServices
    {
        private readonly IAppDbContext _context;
        public FilialServices(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Filial> CreateFilial(Filial filial, CancellationToken cancellationToken)
        {
            var result = await _context.Filials.AnyAsync(x => x.NameEN == filial.NameEN, cancellationToken);
            if(result)
            {
                throw new Exception();
            }
            
            await _context.Filials.AddAsync(filial, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return filial;
        }

        public async Task<bool> DeleteFilial(int id, CancellationToken cancellationToken)
        {
            var filial = await _context.Filials.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if(filial == null)
            {
                throw new Exception();
            }

            _context.Filials.Remove(filial);
            return (await _context.SaveChangesAsync(cancellationToken)) > 0;
        }

        public async Task<List<Filial>> GetAll(CancellationToken cancellationToken)
        {
            var filials = await _context.Filials.ToListAsync(cancellationToken);
            return filials;
        }
    }
}
