using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConfimApi.Data;
using ConfimApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ConfimApi.Controllers
{
    [Route("v1/subcategorias")]
    public class SubcategoriaController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Subcategoria>>> Get([FromServices] DataContext context)
        {
            var subcategoria = await context.Subcategorias.AsNoTracking().ToListAsync();
            return Ok(subcategoria);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Subcategoria>> GetById(
            int id,
            [FromServices] DataContext context)
        {
            var subcategoria = await context.Subcategorias.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(subcategoria);
        }

        [HttpGet]
        [Route("categorias/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Subcategoria>> GetByCategoria(
            int id,
            [FromServices] DataContext context)
        {
            var subcategoria = await context.Subcategorias //.Include(x=> x.IdCategoria)
                               .AsNoTracking().Where(x => x.Id == id).ToListAsync();
            return Ok(subcategoria);
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        // [Authorize(Roles="...")]
        public async Task<ActionResult<List<Subcategoria>>> Post(
            [FromBody] Subcategoria model,
            [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Subcategorias.Add(model);
                await context.SaveChangesAsync();

                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar a subcategoria" });
            }
        }
    }
}
