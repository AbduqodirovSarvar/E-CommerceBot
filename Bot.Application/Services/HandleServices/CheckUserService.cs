using Bot.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application.Services.HandleServices
{
    public class CheckUserService : ICheckUserService
    {
        private readonly IAppDbContext _context;
        private readonly IRedisService _redisService;
        public CheckUserService(
            IAppDbContext context, 
            IRedisService redisService)
        {
            _context = context;
            _redisService = redisService;
        }

        public async Task<bool> CheckUserRegistered(long id, CancellationToken cancellationToken)
        {
            var result = await _redisService.GetAsync(id.ToString());
            if(result != null)
            {
                return true;
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if(user == null)
            {
                return false;
            }
            await _redisService.SetAsync(user.Id.ToString(), user.FullName);
            return true;
        }
    }
}
