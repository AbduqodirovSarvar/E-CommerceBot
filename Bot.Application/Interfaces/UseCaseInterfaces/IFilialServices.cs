using Bot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application.Interfaces.UseCaseInterfaces
{
    public interface IFilialServices
    {
        Task<Filial> CreateFilial(Filial filial, CancellationToken cancellationToken);
        Task<List<Filial>> GetAll(CancellationToken cancellationToken);
        Task<bool> DeleteFilial(int id, CancellationToken cancellationToken);
    }
}
