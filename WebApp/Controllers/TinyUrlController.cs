using Application.Common;
using AutoMapper;
using Domain.Master;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TinyUrlController : ControllerBase
    {
        private readonly ILogger<TinyUrlController> _logger;
        private readonly IApplicationDBContext _dbContext;
        private readonly IMapper _mapper;

        public TinyUrlController(ILogger<TinyUrlController> logger, IMapper mapper, IApplicationDBContext dBContext)
        {
            _logger = logger;
            _dbContext = dBContext;
            _mapper = mapper;
        }

        [HttpPut(Name = "create")]
        public async Task<IActionResult> Create(TinyUrlDTO tinyUrl)
        {
            var url = await _dbContext.TinyUrls.AddAsync(_mapper.Map<TinyUrl>(tinyUrl));
            await _dbContext.SaveChangesAsync();

            return new CreatedAtActionResult(nameof(GetById),
                                "Urls",
                                new { id = url.Entity.Id },
                                url);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TinyUrl))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var url = _dbContext.TinyUrls.Find(id);
            return url == null ? NotFound() : Ok(url);
        }

    }
}