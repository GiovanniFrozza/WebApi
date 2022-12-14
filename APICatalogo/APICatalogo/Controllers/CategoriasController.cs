using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork context, IConfiguration configuration, ILogger<CategoriasController> logger, IMapper mapper)
        {
            _uof = context;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("autor")]
        public string getAutor()
        {
            try
            {
                var autor = _configuration["autor"];
                var conexao = _configuration["ConnectionStrings:DefaultConnection"];

                return $"Autor: {autor}\nConexao: {conexao}";
            }
            catch (Exception)
            {
                return "Ocorreu um problema ao tratar a sua solicitação.";
            }
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            try
            {
                _logger.LogInformation("Iniciando busca de categorias por produtos");

                var categorias = await _uof.CategoriaRepository.GetCategoriasProdutos();
                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);

                _logger.LogInformation("Finalizando busca de categorias por produtos");
                return categoriasDTO;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
                _logger.LogInformation("Iniciando metodo que busca categorias");

                var categorias = await _uof.CategoriaRepository.GetCategorias(categoriasParameters);

                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);

                _logger.LogInformation("Finalizando metodo que busca categorias");

                return categoriasDTO;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                _logger.LogInformation("Iniciando metodo que busca categoria");

                var categoria = await _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

                if (categoria == null)
                    return NotFound($"Categoria com id={id} não encontrada.");

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                _logger.LogInformation("Finalizando metodo que busca categoria");

                return Ok(categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                            "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(CategoriaDTO categoriaDto)
        {
            try
            {
                _logger.LogInformation("Iniciando metodo que adiciona categoria");

                if (categoriaDto is null)
                    return BadRequest("Categoria informada com erro.");

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Add(categoria);
                await _uof.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                _logger.LogInformation("Finalizando metodo que adiciona categoria");

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }            
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> Put(int id, CategoriaDTO categoriaDto)
        {
            try
            {
                _logger.LogInformation("Iniciando metodo que altera categoria");

                if (id != categoriaDto.CategoriaId)
                    return BadRequest("Dados Inválidos.");

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Update(categoria);
                await _uof.Commit();

                _logger.LogInformation("Finalizando metodo que altera categoria");

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Iniciando metodo que deleta categoria");

                var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

                if(categoria == null)
                    return NotFound($"Categoria com id={id} não encontrada.");

                _uof.CategoriaRepository.Delete(categoria);
                await _uof.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                _logger.LogInformation("Finalizando metodo que deleta categoria");

                return Ok(categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

    }
}
