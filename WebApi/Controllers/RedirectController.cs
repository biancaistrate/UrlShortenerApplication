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
        private readonly CurrentContext _context;

        public RedirectController(ITinyUrlRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _context = new CurrentContext(httpContextAccessor.HttpContext);

        }
        [Route("/{alias}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status301MovedPermanently)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task RedirectToOriginal(CancellationToken cancellationToken)
        {
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
