using Application;
using Application.Repository;
using Domain.Master;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [Route("")]
    [ApiController]
    public class RedirectController : ControllerBase
    {
        private readonly ITinyUrlRepository _repository;
        private readonly ITinyUrlCreator _urlCreator;
        private readonly CurrentContext _context;

        public RedirectController(ITinyUrlRepository repository, ITinyUrlCreator urlCreator, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _urlCreator=urlCreator;

            _context = new CurrentContext(httpContextAccessor.HttpContext);

        }
        [Route("/{alias}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status301MovedPermanently)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task RedirectToOriginal(string alias, CancellationToken cancellationToken)
        {
            //var builder = new UriBuilder();

            //builder.Scheme = HttpContext.Request.Scheme;
            //builder.Host = HttpContext.Request.Host.Host;
            //builder.Port = HttpContext.Request.Host.Port.HasValue ? HttpContext.Request.Host.Port.Value : HttpContext.Request.IsHttps ? 443 : 80;
            var context = new CurrentContext(HttpContext);

            //var tinyUrl = await _repository.FindByShortForm(_urlCreator.Create(context.GetDefaultDomain(), alias), cancellationToken);
            var tinyUrl = await _repository.FindByShortForm(_context.GetDisplayUrl(), cancellationToken);

            if (tinyUrl== null)
            {
                NotFound();
                return;
            }
            Response.Redirect(tinyUrl.OriginalUrl);
        }
    }
}
