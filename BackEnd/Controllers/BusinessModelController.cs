using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Company")]
    public class BusinessModelController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        public BusinessModelController(ApplicationDbContext context)
        {
            this.context = context;
        }

        private string ObtenerIDUser()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            user = context.Users.FirstOrDefault(x => x.UserName.Equals(user)).Id;
            return user;
        }





        // GET: api/BusinessModel
        [HttpGet]
       
        public IActionResult Get()
        {
            var list = new List<ModelBusiness>();
            var user = ObtenerIDUser();
            var listIdBusiness = new List<CompanyModelBussines>();
            var obj = new BusinessModel();
            var temp = new ModelBusiness();

            listIdBusiness = context.CompanyModelBussines.Where(x => x.Rut == context.UsersCompany.FirstOrDefault(y => y.IdUser == ObtenerIDUser()).idCompany).ToList();

            foreach (CompanyModelBussines Cont in listIdBusiness)
            {
                obj = new BusinessModel();
                temp = new ModelBusiness();
                obj = context.BusinessModels.FirstOrDefault(x => x.Id == Cont.Id);
                temp.Id = obj.Id;
                temp.Name = obj.Name;
                temp.Descripcion = obj.Descripcion;
                temp.Image = obj.Image;
                if (obj.CategoriID is null) 
                {
                    temp.CategoryID = null;
                    temp.NameCategory = null;

                }
                else { 
                temp.CategoryID = obj.CategoriID;
                temp.NameCategory = context.Category.FirstOrDefault(x => x.Id == obj.CategoriID).Name;
                }
                list.Add(temp);
            }

            return Ok(new { results = list });
        }


        public class ModelBusiness {

            public int Id { get; set; }
            public string Name { get; set; }
            
            public string Descripcion { get; set; }

            [Column(TypeName = "nvarchar(Max)")]
            public String? Image { get; set; }

            public int? CategoryID { get; set; }

            public string NameCategory { get; set; }
        }

        public class ModelBusinessCreate
        {

 
            public string Name { get; set; }

            public string? Descripcion { get; set; }

            [Column(TypeName = "nvarchar(Max)")]
            public String? Image { get; set; }

            public int? CategoryID { get; set; }

        }

        public class ModelBusinessModify
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string? Descripcion { get; set; }

            [Column(TypeName = "nvarchar(Max)")]
            public String? Image { get; set; }

            public int? CategoryID { get; set; }

        }




        [HttpPost]
        public IActionResult Post([FromBody] ModelBusinessCreate model)
        {
            if (ModelState.IsValid)
            {

                var userid = ObtenerIDUser();
                var idCompany = context.UsersCompany.FirstOrDefault(x => x.IdUser == userid).idCompany;
                BusinessModel objModel = new BusinessModel() ;
       
                objModel.Name = model.Name;
                //objModel.Descripcion = model.Descripcion;
                byte[] bytes = Encoding.Default.GetBytes(model.Descripcion);
                objModel.Descripcion = Encoding.UTF8.GetString(bytes);
                objModel.Image = model.Image;
                if (context.Category.Count(x => x.Id == model.CategoryID) >0)
                {
                    objModel.CategoriID = model.CategoryID;
                }
              
                try
                {
                    
                    
                    context.BusinessModels.Add(objModel);
                    context.SaveChanges();

                    var objBussines = new CompanyModelBussines();
                    objBussines.Rut = idCompany;
                    objBussines.Id = objModel.Id;
                    context.CompanyModelBussines.Add(objBussines);
                    context.SaveChanges();

               


                    return Ok(objModel.Id);
                }
                catch (IOException e)
                {
                    return BadRequest(e.Message);
                }


            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("PostModify")]
  
        public IActionResult PostModify([FromBody] ModelBusinessModify model)
        {
            if (ModelState.IsValid)
            {

                var userid = ObtenerIDUser();
                var idCompany = context.UsersCompany.FirstOrDefault(x => x.IdUser == userid).idCompany;
                BusinessModel objModel = new BusinessModel();
                objModel = context.BusinessModels.FirstOrDefault(x => x.Id == model.Id);

                if (objModel == null)
                {

                    return BadRequest("No exise el modelo a modificar.");
                }

   
                if (model.Name != null)
                {
                    objModel.Name = model.Name;
                }
               
                if (model.Descripcion !=null || model.Descripcion != "")
                {
                    byte[] bytes = Encoding.Default.GetBytes(model.Descripcion);
                    //objModel.Descripcion = model.Descripcion;
                    objModel.Descripcion = Encoding.UTF8.GetString(bytes);

                }

                if (model.Image != null)
                {
                    objModel.Image = model.Image;
                }

                if (model.CategoryID != null)
                {
                            

                if (context.Category.Count(x => x.Id == model.CategoryID) > 0)
                {
                    objModel.CategoriID = model.CategoryID;
                }
                }
                try
                {


                    context.BusinessModels.Update(objModel);
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


        // GET: api/BusinessModel/5
        // [HttpGet("{id}", Name = "Get")]
        // public string Get(int id)
        // {
        //     return "value";
        // }

        // POST: api/BusinessModel
        //[HttpPost]
        // public void Post([FromBody] string value)
        // {
        // }

        // PUT: api/BusinessModel/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody] string value)
        // {
        // }

        // DELETE: api/ApiWithActions/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }
}
