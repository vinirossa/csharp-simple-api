using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConfimApi.Data;
using ConfimApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ConfimApi.Controllers
{
    [Route("v1/categorias")]
    public class CategoriaController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        // [ResponseCache(VLocation = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
        public async Task<ActionResult<List<Categoria>>> Get([FromServices] DataContext context)
        {
            var categoria = await context.Categorias.AsNoTracking().ToListAsync();
            return Ok(categoria);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Categoria>> GetById(
            int id,
            [FromServices] DataContext context)
        {
            var categoria = await context.Categorias.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(categoria);
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        // [Authorize(Roles="...")]
        public async Task<ActionResult<List<Categoria>>> Post(
            [FromBody] Categoria model,
            [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Categorias.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar a categoria" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        // [Authorize(Roles="...")]
        public async Task<ActionResult<List<Categoria>>> Put(
            int id,
            [FromBody] Categoria model,
            [FromServices] DataContext context)
        {
            if (id != model.Id)
                return NotFound(new { message = "Categoria não encontrada" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Categoria>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Esse registro já foi atualizado" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível atualizar a categoria" });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        // [Authorize(Roles="...")]
        public async Task<ActionResult<List<Categoria>>> Delete(
            int id,
            [FromServices] DataContext context)
        {
            var categoria = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
            if (categoria == null)
            {
                return NotFound(new { message = "Categoria não encontrada" });
            }
            try
            {
                context.Categorias.Remove(categoria);
                await context.SaveChangesAsync();
                return Ok(categoria);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível excluir a categoria" });
            }
        }
    }
}
