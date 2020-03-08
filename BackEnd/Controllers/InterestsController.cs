using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[ApiController]   

    public class InterestsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public InterestsController(ApplicationDbContext context)
        {
            this.context = context;
        }

     

        [HttpGet]
        //public IEnumerable<Interests> Get()
        //[Authorize(Roles = "Company")]
        //[Authorize(Roles = "User")]
        public ActionResult Get()
        {
            var interestess = context.Interests.ToList();
            return Ok(new { results = interestess });
            //return context.Interests.ToList() ;
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "User")]
        public ActionResult GetInterestforUser()
        {
            var user = ObtenerIDUser();
            SqlParameter parameterS = new SqlParameter("@Id", user);
            var listIntUser = context.Interests.FromSql("select * from Interests where id  not in (select InterestsId from InterestsUsers where IdUser=@Id ) ", parameterS).ToList();
            return Ok(new { results = listIntUser });
            //return context.Interests.ToList() ;
        }


        private string ObtenerIDUser()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            user = context.Users.FirstOrDefault(x => x.UserName.Equals(user)).Id;
            return user;
        }


        [HttpPost]
        [Authorize(Roles = "Company")]
        public IActionResult Post([FromBody] Interests Interes)
        {

            if (ModelState.IsValid)
            {

                context.Interests.Add(Interes);
                context.SaveChanges();
                return new CreatedAtRouteResult("InterestsCreated", new { id = Interes.Id }, Interes);
            }

            return BadRequest(ModelState);
        }


        [HttpGet("{id}", Name = "InterestsCreated")]
        [Authorize(Roles = "Company")]
        public IActionResult GetById(int id)
        {
            var Interest = context.Interests.FirstOrDefault(x => x.Id == id);

            if (Interest == null)
            {

                return NotFound();
            }

            return Ok(Interest);

        }



        [HttpPut("{id}")]
        [Authorize(Roles = "Company")]

        public IActionResult Put([FromBody]Interests inter, int id)
        {

            if (inter.Id != id)
            {

                return BadRequest();
            }

            context.Entry(inter).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }


    }
}