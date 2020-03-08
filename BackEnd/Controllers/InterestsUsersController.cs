using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Produces("application/json")]
    [Route("api/IU")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IntereresUsuariosController : ControllerBase
    {

        private readonly ApplicationDbContext context;

        public IntereresUsuariosController(ApplicationDbContext context)
        {
            this.context = context;

            
        }
        // GET: api/IntereresUsuarios
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return View(ListaInterests());
        //}
        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult Get() {
            var interestesUser = ListofInterests();
        
            return Ok(new { results = interestesUser });
            //return ListofInterests();
        }
            
    


        private List<Interests> ListofInterests()

        {

            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var userid = ObtenerIDUser();
            var Interest = new List<InterestsUsers>();
            var inter = new List<Interests>();
            Interest = context.InterestsUsers.Where(x => x.IdUser.Equals(userid)).ToList();
            foreach (InterestsUsers Cont in Interest) {
                inter.Add(context.Interests.FirstOrDefault(x => x.Id == Cont.InterestsId));
            }

            if (inter == null)
            {

                return inter;
            }

            return inter;

        }


        private string ObtenerIDUser()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            user = context.Users.FirstOrDefault(x => x.UserName.Equals(user)).Id;
            return user;
        }


        // POST: api/IntereresUsuarios
        [HttpPost]

        [Authorize(Roles = "User")]
        public IActionResult Post([FromBody] MOdelInterestUser InterestsUser)
        {

            var obj = new InterestsUsers();
     
         obj.IdUser = ObtenerIDUser();
            obj.InterestsId = InterestsUser.Interests_Id;


            if (ModelState.IsValid)
            {
              
               
                context.InterestsUsers.Add(obj);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                  
                        if ((ex.InnerException as SqlException)?.Number == 2627)
                    {
                        return BadRequest("Ya existe el interes asociado al usuario actual.");

                    }
                    return BadRequest();
                }
               
             
                return Ok();
            }

            return BadRequest(ModelState);
        }

        public class MOdelInterestUser {

            public int Interests_Id { get; set; }
        }

        // PUT: api/IntereresUsuarios/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [Authorize(Roles = "User")]
        public IActionResult Delete([FromBody] MOdelInterestUser InterestsUser)
        {

            var obj = new InterestsUsers();

            obj.IdUser = ObtenerIDUser();
            obj.InterestsId = InterestsUser.Interests_Id;


            if (ModelState.IsValid)
            {


                context.InterestsUsers.Remove(obj);
              
                    context.SaveChanges();
               

                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}
