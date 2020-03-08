using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(Roles = "Company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public CompanyController(ApplicationDbContext context)
        {
            this.context = context;
        }


        [HttpPost]

        public IActionResult Post([FromBody] ModelIniCommpany company)
        {
            if (ModelState.IsValid)
            {

                var userid = ObtenerIDUser();
                try
                { 
                    var objCompany = new Company();
                    objCompany.Name = company.Name;
                    objCompany.email = company.email;
                    objCompany.Rut = company.Rut;
                    context.Company.Add(objCompany);
                    context.SaveChanges();
                    return Ok();
                }
                catch (IOException e)
                {
                    return BadRequest(e.Message);
                }


            }

            return BadRequest(ModelState);
        }

        private string ObtenerIDUser()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            user = context.Users.FirstOrDefault(x => x.UserName.Equals(user)).Id;
            return user;
        }

        [HttpGet]

        public IActionResult Get()
        {

            var user = ObtenerIDUser();
            var objCompany = context.Company.FirstOrDefault(x => x.Rut == (context.UsersCompany.FirstOrDefault(x => x.IdUser == user).idCompany));
            var mCompany = new ModelIniCommpany();
            mCompany.Name = objCompany.Name;
            mCompany.Rut = objCompany.Rut;
            mCompany.email = objCompany.email;
            mCompany.Image = objCompany.Image;

            return Ok(mCompany);



        }


   


        //[HttpPut]

        //public IActionResult Put([FromBody]ModelIniCommpanyModifique company)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var userid = ObtenerIDUser();

        //        var objCompani = context.Company.FirstOrDefault(x => x.IdUser == userid);
        //        if (objCompani != null)
        //        {
        //            objCompani.Description = company.Description;
        //            objCompani.Name = company.Name;
        //            objCompani.Image = company.Image;
        //            objCompani.Rut = company.Rut;
        //            objCompani.business_name = company.business_name;
        //            objCompani.Address = company.Address;
        //            objCompani.email = company.email;

        //            context.Entry(objCompani).State = EntityState.Modified;
        //            context.SaveChanges();
        //            return Ok();
        //        }
        //        else
        //        {
        //            return BadRequest("No existe empresa a modificar.");
        //        }
        //    }

        //    return BadRequest(ModelState);
        //}



        public class ModelIniCommpany {


            [Key]
            public Int64 Rut { get; set; }

            [Required]
            public string Name { get; set; }


            [Required]
            public string email { get; set; }

            [Column(TypeName = "nvarchar(Max)")]
            public String? Image { get; set; }

        }

        public class ModelIniCommpanyModifique
        {



            [Required]
            public string Name { get; set; }



            [Required]
            public string email { get; set; }
        }



    }
}