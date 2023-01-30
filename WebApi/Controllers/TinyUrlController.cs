using Application;
using Application.Common;
using Application.Repository;
using AutoMapper;
using Domain.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize()]
    [ApiController]
    [Route("[controller]")]
    public class TinyUrlController : ControllerBase
    {
        private readonly ILogger<TinyUrlController> _logger;
        private readonly ITinyUrlRepository _urlsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITinyUrlCreator _tinyUrlCreator;
        private readonly CurrentContext _context;

        public TinyUrlController(ILogger<TinyUrlController> logger, IMapper mapper, ITinyUrlRepository urlsRepository, IUserRepository userRepository, ITinyUrlCreator tinyUrlCreator, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _urlsRepository = urlsRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _tinyUrlCreator = tinyUrlCreator;
            _context = new CurrentContext(httpContextAccessor.HttpContext);
        }

        [AllowAnonymous]
        [HttpPut(Name = "create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TinyUrl))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] TinyUrlDTO tinyUrl, CancellationToken cancellationToken)
        {
            var computedTinyUrl = _mapper.Map<TinyUrl>(tinyUrl);
            computedTinyUrl.TinyUri = _tinyUrlCreator.Create(_context.GetDisplayUrl(), tinyUrl.Alias);
            if (_context.IsUserAuthenticated())
            {
                var user = await _userRepository.GetByEmail(_context.GetUserClaim(), cancellationToken);
                if (user != null)
                {
                    computedTinyUrl.UserRefId = user.Id;
                }
            }
            var url = _urlsRepository.Insert(computedTinyUrl);
            await _urlsRepository.SaveAsync(cancellationToken);


            return new CreatedAtActionResult(nameof(GetById),
                                "TinyUrl",
                                new { id = url.Id },
                                url);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TinyUrl))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var url = await _urlsRepository.GetByIdAsync(id, string.Empty, cancellationToken);
            return url == null ? NotFound() : Ok(url);
        }

        [AllowAnonymous]
        [HttpGet("get-by-short-form")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TinyUrl))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByShortForm([FromQuery] string tinyUrl, CancellationToken cancellationToken)
        {
            var url = await _urlsRepository.FindByShortForm(new Uri(tinyUrl), cancellationToken);
            return url == null ? NotFound() : Ok(url);
        }

        [Authorize()]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TinyUrl>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            if (!_context.IsUserAuthenticated())
                return Unauthorized();

            if (!_context.HasClaims())
                return Unauthorized();

            var urls = await _urlsRepository.GetAllAsync(_context.GetUserClaim(), cancellationToken);
            return Ok(urls);
        }

        [Authorize()]
        [HttpGet("get-recent/{top:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TinyUrl>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTop(int top, CancellationToken cancellationToken)
        {
            if (!_context.IsUserAuthenticated())
                return Unauthorized();

            if (!_context.HasClaims())
                return Unauthorized();

            var urls = await _urlsRepository.GetTopAsync( top, _context.GetUserClaim(), cancellationToken);
            return Ok(urls);
        }

        [Authorize()]
        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TinyUrl))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] UpdateTinyUrlDTO tinyUrl, CancellationToken cancellationToken)
        {
            if (!_context.IsUserAuthenticated())
                return Unauthorized();

            if (!_context.HasClaims())
                return Unauthorized();

            var url = await _urlsRepository.GetByIdAsync(tinyUrl.Id, _context.GetUserClaim(), cancellationToken);
            if(url == null)
                return new NotFoundObjectResult(StatusCodes.Status404NotFound);

            url.OriginalUrl=tinyUrl.NewOriginalUrl;
            url.Alias = tinyUrl.NewAlias;
            url.TinyUri = _tinyUrlCreator.Create(_context.GetDisplayUrl(), tinyUrl.NewAlias);

            _urlsRepository.Update(url);
            await _urlsRepository.SaveAsync(cancellationToken);
            return Ok(url);
        }

        [Authorize()]
        [HttpDelete("{alias}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteTinyUrl([FromQuery] string alias, CancellationToken cancellationToken)
        {
            if (!_context.IsUserAuthenticated())
                return Unauthorized();

            if (!_context.HasClaims())
                return Unauthorized();
            
            var tinyUrl = await _urlsRepository.FindByShortForm(_context.GetDisplayUrl(), cancellationToken);

            if (tinyUrl == null)
            {
                return NoContent();
            }
            _urlsRepository.Delete(tinyUrl);
            await _urlsRepository.SaveAsync(cancellationToken);
            return NoContent();
        }
    }
}