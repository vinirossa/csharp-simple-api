using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using ConfimApi.Data;
using ConfimApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ConfimApi.Controllers
{
    [Route("v1")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext context)
        {
            var usuarioRaiz = new Usuario {Id = 1, Nome = "Sistema", Senha = "Citametsys"};
            var categoria = new Categoria {Id = 1, Descricao = "Categoria Inicial"};
            var subcategoria = new Subcategoria {Id =1, Descricao = "Subcategoria Inicial", IdCategoria = 1};
            context.Usuarios.Add(usuarioRaiz);
            context.Categorias.Add(categoria);
            context.Subcategorias.Add(subcategoria);
            await context.SaveChangesAsync();

            return Ok(new { message = "Dados Configurados"});
            
        }

    
    }
}