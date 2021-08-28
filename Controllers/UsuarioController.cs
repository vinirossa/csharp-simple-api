using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConfimApi.Data;
using ConfimApi.Models;
using ConfimApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ConfimApi.Controllers
{
    [Route("v1/usuarios")]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        [Route("anonimo")]
        [AllowAnonymous]
        public string Anonimo() => "Anônimo";

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<List<Usuario>>> Get([FromServices] DataContext context)
        {
            var usuarios = await context.Usuarios.AsNoTracking().ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet]
        [Route("nome")]
        [Authorize(Roles = "Paulo Martim")]
        public string Nome() => "Nome";
        
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        // [Authorize(Roles="...")]
        public async Task<ActionResult<List<Usuario>>> Post(
            [FromBody] Usuario model,
            [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Usuarios.Add(model);
                await context.SaveChangesAsync();
                model.Senha = "";
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário" });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromBody] Usuario model,
            [FromServices] DataContext context)
        {
            var usuario = await context.Usuarios
                .AsNoTracking()
                .Where(x => x.Nome == model.Nome && x.Senha == model.Senha)
                .FirstOrDefaultAsync();
            
            if (usuario == null)
                return NotFound(new {message = "Usuário ou senha inválidos"});

            var token = TokenService.GenerateToken(usuario);
            return new
            {
                // usuario = usuario,
                token = token
            };
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Usuario>>> Put(
            int id,
            [FromBody] Usuario model,
            [FromServices] DataContext context)
        {
            if (id != model.Id)
                return NotFound(new { message = "Usuário não encontrado" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Usuario>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                model.Senha = "";
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Esse registro já foi atualizado" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível atualizar o usuário" });
            }
        }
    }
}