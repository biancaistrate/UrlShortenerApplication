using Application.Common;
using Application.Repository;
using Domain.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class TinyUrlRepository : ITinyUrlRepository
    {
        private readonly ILogger<TinyUrlRepository> _logger;
        private readonly IApplicationDBContext _dbContext;
        public TinyUrlRepository(ILogger<TinyUrlRepository> logger, IApplicationDBContext dBContext)
        {
            _logger = logger;
            _dbContext = dBContext;
        }
        public void Delete(TinyUrl tinyUrl)
        {
            _dbContext.TinyUrls.Remove(tinyUrl);
        }

        public async Task<IEnumerable<TinyUrl>> GetAllAsync(string userIdentifier, CancellationToken cancellationToken)
        {
            return await _dbContext.TinyUrls.Include(x => x.UserIdentity).Where(url => url.UserIdentity.Email == userIdentifier).ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<TinyUrl>> GetTopAsync(int top, string userIdentifier, CancellationToken cancellationToken)
        {
            return await _dbContext.TinyUrls.Include(x => x.UserIdentity).Where(url => url.UserIdentity.Email == userIdentifier).Take(top).ToListAsync(cancellationToken);
        }

        public async Task<TinyUrl?> GetByIdAsync(int tinyUrlId, string userIdentifier, CancellationToken cancellationToken)
        {
            return await _dbContext.TinyUrls.Include(x => x.UserIdentity).Where(url => url.UserIdentity.Email == userIdentifier).FirstOrDefaultAsync(x => x.Id == tinyUrlId, cancellationToken);
        }

        public async Task<TinyUrl?> FindByShortForm(Uri tinyUrl, CancellationToken cancellationToken)
        {
            return await _dbContext.TinyUrls.FirstOrDefaultAsync(x => x.TinyUri == tinyUrl, cancellationToken);
        }

        public TinyUrl Insert(TinyUrl tinyUrl)
        {
            return _dbContext.TinyUrls.Add(tinyUrl).Entity;
        }

        public void Update(TinyUrl tinyUrl)
        {
            _dbContext.TinyUrls.Update(tinyUrl);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveAsync(cancellationToken);
        }
    }
}
