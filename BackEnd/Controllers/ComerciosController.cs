using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Produces("application/json")]
    [Route("api/Comercios")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[ApiController]
    public class ComerciosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public ComerciosController(ApplicationDbContext context) {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Comercios> Get() {
            return context.Comercios.ToList();
        }

        [HttpGet("{id}",Name = "comercioCreado")]

        public IActionResult GetById(int id)
        {
            var Comercio = context.Comercios.FirstOrDefault(x => x.Id == id);

            if (Comercio == null) {

                return NotFound();
            }

            return Ok(Comercio);
             
        }

        [HttpPost]
        public IActionResult Post([FromBody] Comercios Comer) {

            if (ModelState.IsValid) {

                context.Comercios.Add(Comer);
                context.SaveChanges();
                return new CreatedAtRouteResult("comercioCreado", new { id = Comer.Id }, Comer);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]

        public IActionResult Put([FromBody]Comercios comer, int id) {

            if (comer.Id != id) {

                return BadRequest();
            }

            context.Entry(comer).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }
    }
}