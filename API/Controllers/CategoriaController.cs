using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Models;

namespace API.Controllers;

[Route("api/categoria")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly AppDataContext _context;

    public CategoriaController(AppDataContext context) =>
        _context = context;

    // GET: api/categoria/listar
    [HttpGet]
    [Route("listar")]
    public IActionResult Listar()
    {
        try
        {
            List<Categoria> categorias = _context.Categorias.ToList();
            return Ok(categorias);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] Categoria categoria)
    {
        try
        {
            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return Created("", categoria);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

     // GET: api/categoria/buscar/{nome}
    [HttpGet]
    [Route("buscar/{nome}")]
    public ActionResult Buscar([FromRoute] string nome)
    {
        try
        {
            Categoria? categoriaCadastrada = _context.Categorias.FirstOrDefault(x => x.Nome == nome);
            if (categoriaCadastrada != null)
            {
                return Ok(categoriaCadastrada);
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    // PUT: api/categoria/alterar/5
    [HttpPut]
    [Route("alterar/{id}")]
    public IActionResult Alterar([FromRoute] int id,
        [FromBody] Categoria categoria)
    {
        try
        {
            Categoria? categoriaCadastrada =
                _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);
            if (categoriaCadastrada != null)
            {
                categoriaCadastrada.Nome = categoria.Nome;
                _context.SaveChanges();
                return Ok(categoria);
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

     // DELETE: api/categoria/deletar/5
    [HttpDelete]
    [Route("deletar/{id}")]
    public IActionResult Deletar([FromRoute] int id)
    {
        try
        {
            Categoria? categoriaCadastrada = _context.Categorias.Find(id);
            if (categoriaCadastrada != null)
            {
                _context.Categorias.Remove(categoriaCadastrada);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
