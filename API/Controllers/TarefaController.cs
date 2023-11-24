using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;

namespace API.Controllers;

[Route("api/tarefa")]
[ApiController]
public class TarefaController : ControllerBase
{
    private readonly AppDataContext _context;

    public TarefaController(AppDataContext context) =>
        _context = context;

    // GET: api/tarefa/listar
    [HttpGet]
    [Route("listar")]
    public IActionResult Listar()
    {
        try
        {
            List<Tarefa> tarefas = _context.Tarefas.Include(x => x.Categoria).ToList();
            return Ok(tarefas);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // POST: api/tarefa/cadastrar
    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] Tarefa tarefa)
    {
        try
        {
            Categoria? categoria = _context.Categorias.Find(tarefa.CategoriaId);
            if (categoria == null)
            {
                return NotFound();
            }
            tarefa.Categoria = categoria;
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            return Created("", tarefa);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // POST: api/tarefa/buscar
      [HttpGet]
    [Route("buscar/{id}")]
    public IActionResult Buscar([FromRoute] int id)
    {
        try
        {
            Tarefa? tarefaCadastrado = _context.Tarefas.FirstOrDefault(x => x.TarefaId == id);
            if (tarefaCadastrado != null)
            {
                return Ok(tarefaCadastrado);
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

     [HttpDelete]
    [Route("deletar/{id}")]
    public IActionResult Deletar([FromRoute] int id)
    {
        try
        {
            Tarefa? TarefaCadastrado = _context.Tarefas.Find(id);
            if (TarefaCadastrado != null)
            {
                 _context.Tarefas.Remove(TarefaCadastrado);
                 _context.SaveChanges();
                return Ok(
                     _context.Tarefas.
                    Include(x => x.Categoria).
                    ToList());
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

     [HttpPut]
    [Route("alterar/{id}")]
    public IActionResult Alterar([FromRoute] int id,
        [FromBody] Tarefa Tarefa)
    {
        try
        {
            //Expressões lambda
            Tarefa? TarefaCadastrado =
                _context.Tarefas.FirstOrDefault(x => x.TarefaId == id);
            if (TarefaCadastrado != null)
            {
                Categoria? categoria =
                    _context.Categorias.Find(Tarefa.CategoriaId);
                if (categoria == null)
                {
                    return NotFound();
                }
                TarefaCadastrado.Categoria = categoria;
                TarefaCadastrado.Titulo = Tarefa.Titulo;
                TarefaCadastrado.Descricao = Tarefa.Descricao;
                TarefaCadastrado.CriadoEm = Tarefa.CriadoEm;
                _context.Tarefas.Update(TarefaCadastrado);
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
