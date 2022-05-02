using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Ucondo_Financeiro_Api.Contratos;
using Ucondo_Financeiro_Dominio.Entities;
using Ucondo_Financeiro_Dominio.Interfaces.Services;

namespace Ucondo_Financeiro_Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class ContaRateioController : ControllerBase
    {
        private readonly IContaRateioService _service;
        private readonly IMapper _mapper;
        public ContaRateioController(IContaRateioService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                LogRequest();
                return Ok(_service.ListarContasRaiz());
            }
            catch (Exception ex)
            {
                return RetornarBadRequest(ex.Message);
            }
        }

        [HttpGet("ByContaPai/{idContaPai}")]
        public IActionResult Get(Guid idContaPai)
        {
            try
            {
                LogRequest();
                return Ok(_service.ListarByContaPai(idContaPai));
            }
            catch (Exception ex)
            {
                return RetornarBadRequest(ex.Message);
            }
        }

        [HttpGet("{pesquisa}")]
        public IActionResult GetPesquisa(string pesquisa)
        {
            try
            {
                LogRequest();
                return Ok(_service.Listar(pesquisa));
            }
            catch (Exception ex)
            {
                return RetornarBadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]ContaRateioContrato contrato)
        {
            try
            {
                LogRequest();
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState); //Padrão de mensagem diferente

                _service.Inserir(_mapper.Map<ContaRateio>(contrato));

                return Ok();
            }
            catch (Exception ex)
            {
                return RetornarBadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] ContaRateioContrato contrato)
        {
            try
            {
                LogRequest();
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState); //Padrão de mensagem diferente

                _service.Alterar(_mapper.Map<ContaRateio>(contrato));

                return Ok();
            }
            catch (Exception ex)
            {
                return RetornarBadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                LogRequest();
                _service.Deletar(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return RetornarBadRequest(ex.Message);
            }
        }

        //Quando conta raiz, enviar 00000000-0000-0000-0000-000000000000
        [HttpGet("proximo-codigo/{idContaPai}")]
        public IActionResult ProximoCodigo(Guid? idContaPai)
        {
            try
            {
                LogRequest();
                return Ok(_service.ProximoCodigo(idContaPai));
            }
            catch (Exception ex)
            {
                return RetornarBadRequest(ex.Message);
            }
        }

        private ObjectResult RetornarBadRequest(string message)
        {
            Log.Error("{method} - {controller} - User-Agent:{userAgent}; IP:{ip}; {message}"
                , HttpContext.Request.Method, typeof(ContaRateioController).Name, HttpContext.Request.Headers["User-Agent"], HttpContext.Connection.RemoteIpAddress.MapToIPv4(),
                message);

            return StatusCode(StatusCodes.Status400BadRequest, message);
        }

        protected void LogRequest()
        {
            Log.Information("{method} - {controller} - User-Agent:{userAgent}; IP:{ip}", HttpContext.Request.Method, typeof(ContaRateioController).Name, HttpContext.Request.Headers["User-Agent"], HttpContext.Connection.RemoteIpAddress.MapToIPv4());
        }
    }
}
