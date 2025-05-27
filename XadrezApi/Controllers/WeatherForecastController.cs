using Microsoft.AspNetCore.Mvc;
using Xadrez.API.Services;
using Xadrez.API.Domain.Exceptions;
using XadrezApi.Dtos;

namespace Xadrez.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class XadrezController : ControllerBase
{
    private readonly JogoDeXadrezService _xadrezService;

    public XadrezController(JogoDeXadrezService xadrezService)
    {
        _xadrezService = xadrezService;
    }

    [HttpGet("estado")]
    public IActionResult ObterEstadoPartida()
    {
        var partida = _xadrezService.ObterPartida();
        return Ok(partida); // Em produção, use um DTO para não expor tudo
    }

    [HttpPost("jogada")]
    public IActionResult RealizarJogada([FromBody] PosicaoXadrezDto jogada)
    {
        try
        {
            _xadrezService.RealizarJogada(jogada.Origem, jogada.Destino);
            return Ok("Jogada realizada com sucesso.");
        }
        catch (TabuleiroException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { erro = "Erro interno.", detalhes = ex.Message });
        }
    }

    [HttpGet("movimentos-possiveis/{origem}")]
    public IActionResult ObterMovimentosPossiveis(string origem)
    {
        try
        {
            var movimentos = _xadrezService.ObterMovimentosPossiveis(origem);
            return Ok(movimentos);
        }
        catch (TabuleiroException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet("tabuleiro/texto")]
    public IActionResult GetTabuleiroTexto()
    {
        var texto = _xadrezService.ObterTabuleiroFormatado();
        return Content(texto, "text/plain");
    }



}
