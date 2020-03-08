using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    [Produces("application/json")]
    [Route("api/Commerce")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[ApiController]

    public class CommerceController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public CommerceController(ApplicationDbContext context) {
            this.context = context;
        }

        [HttpGet]
        //[Authorize(Roles = "Company")]
        //[Authorize(Roles = "User")]
        public ActionResult Get() {

            var comemer = context.Commerce.ToList();
            var listCommerceResult = new List<ModelCommerce>();
            var objModel = new ModelCommerce();
            var cont = 0;
            foreach (Commerce com in comemer)
            {
                objModel = new ModelCommerce();
                objModel.Id = com.Id;
                objModel.Name = context.BusinessModels.FirstOrDefault((x => x.Id == context.BussinessCommerce.FirstOrDefault(x => x.Commerce == com.Id).Bussines)).Name;
                objModel.Image = context.BusinessModels.FirstOrDefault((x => x.Id == context.BussinessCommerce.FirstOrDefault(x => x.Commerce == com.Id).Bussines)).Image;
                objModel.Latitude = com.Latitude;
                objModel.Longitude = com.Longitude;
                objModel.Phone = com.Phone;
                objModel.Address = com.Address;
                objModel.Discounts = ListDiscountCommerce(com.Id, ref cont);
                //com.Discounts = ListDiscountCommerce(com.Id);
                listCommerceResult.Add(objModel);

            }

            return Ok(new { results = listCommerceResult });

            //return context.Commerce.ToList();
        }


        [HttpGet("{id}",Name = "CommerceCreate")]
        [Authorize(Roles = "Company")]
        public IActionResult GetById(int id)
        {
            var Commerc = context.Commerce.FirstOrDefault(x => x.Id == id);

            if (Commerc == null) {

                return NotFound();
            }

            return Ok(Commerc);
             
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        public IActionResult Post([FromBody] ModelCommerceCreate Comer) {

            if (ModelState.IsValid) {
                if (context.BusinessModels.Count(x => x.Id == Comer.Id_Model) > 0)
                {
                    var com = new Commerce();
                    com.Alias = Comer.Alias;
                    com.Address = Comer.Address;
                    com.Latitude = Comer.Latitude;
                    com.Longitude = Comer.Longitude;
                    com.Phone = Comer.Phone;

                    context.Commerce.Add(com);
                    context.SaveChanges();

                    var modelcomer = new BussinessCommerce();
                    modelcomer.Commerce = com.Id;
                    modelcomer.Bussines = Comer.Id_Model;
                    context.BussinessCommerce.Add(modelcomer);
                    context.SaveChanges();

                }
                else
                {
                    return BadRequest("No existe el modelo.");
                }
                
                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Company")]
        public IActionResult Put([FromBody]Commerce comer, int id) {

            if (comer.Id != id) {

                return BadRequest();
            }

            context.Entry(comer).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }
        public class ModelDiscount
        {
            public int IdTemp { get; set; }
            public int Id { get; set; }

            public int IdCommerce { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public decimal Discount_value { get; set; }

            public string Discount_Type { get; set; }

            public String? Imagen { get; set; }

            [Column(TypeName = "decimal(2,1)")]
        
            public Decimal Value { get; set; }
            [Column(TypeName = "decimal(2,1)")]
           
            public Decimal Article { get; set; }
            [Column(TypeName = "decimal(2,1)")]
          
            public Decimal CommerceValued { get; set; }

        }

        public class ModelCommerce
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Address { get; set; }

            public double Latitude { get; set; }

            public double Longitude { get; set; }

            public int Phone { get; set; }

            public String? Image { get; set; }

            public List<ModelDiscount>? Discounts { get; set; }
        }

        public class ModelCommerceCreate
        {

            public int Id_Model { get; set; }

            [StringLength(40)]
            [Required]
            public string Alias { get; set; }
            [Required]
            public string Address { get; set; }

            public double Latitude { get; set; }

            public double Longitude { get; set; }

            public int Phone { get; set; }

        }

        


        private List<ModelCommerce> ListOfCommerce(int idInterest)

        {


            var Discount = new List<Discounts>();
            var ListIdDiscount = new List<DiscountsInterests>();
            ListIdDiscount = context.DiscountsInterests.Where(x => x.InterestsId == idInterest).ToList();
            var cont = 0;
            foreach (DiscountsInterests Cont in ListIdDiscount)
            {
                Discount.Add(context.Discounts.SingleOrDefault(x => x.Id == Cont.DiscountsID));
            }

            var prueba = new List<int>();

            foreach (Discounts y in Discount)
            {
                prueba.Add(context.CommerceDiscounts.FirstOrDefault(x => x.DiscountsID == y.Id).CommerceID);
            }



            var listofcommerce = new List<Commerce>();
            foreach (int contador in prueba.Distinct())
            {
                listofcommerce.Add(context.Commerce.FirstOrDefault(x => x.Id == contador));
            }

            var listCommerceResult = new List<ModelCommerce>();
            var objModel =  new ModelCommerce();
            foreach (Commerce com in listofcommerce)
            {
                objModel = new ModelCommerce();
                objModel.Id = com.Id;
                objModel.Name = context.BusinessModels.FirstOrDefault((x => x.Id == context.BussinessCommerce.FirstOrDefault(x => x.Commerce == com.Id).Bussines)).Name;
                objModel.Image = context.BusinessModels.FirstOrDefault((x => x.Id == context.BussinessCommerce.FirstOrDefault(x => x.Commerce == com.Id).Bussines)).Image;
                objModel.Latitude = com.Latitude;
                objModel.Longitude = com.Longitude;
                objModel.Phone = com.Phone;
                objModel.Address = com.Address;
                objModel.Discounts = ListDiscountCommerce(com.Id, ref cont);
                //com.Discounts = ListDiscountCommerce(com.Id);
                listCommerceResult.Add(objModel);

            }

           

            if (listCommerceResult == null)
            {

                return listCommerceResult;
            }

            return listCommerceResult;

        }
      
        //[HttpGet("{id}")]
 
        [Route("[action]/{id}")]
        [HttpGet]
        //[Authorize(Roles = "Company")]
        //[Authorize(Roles = "User")]
        public IActionResult GetByInterest(int id)
        {
            var Commerce = ListOfCommerce(id);

            return Ok(new { results = Commerce });
            //return ListofInterests();


        }




        private List<ModelDiscount> ListDiscountCommerce(int idCommerce, ref int contador)

        {

            var discountModel = new ModelDiscount();
            var discountObject = new Discounts();
            var Discount = new List<ModelDiscount>();
            var IdDiscounts = new List<CommerceDiscounts>();
        
       
            IdDiscounts = context.CommerceDiscounts.Where(x => x.CommerceID == idCommerce).ToList();



            foreach (CommerceDiscounts Cont in IdDiscounts)
            {
                discountObject = new Discounts();
                discountModel = new ModelDiscount();
                discountObject = context.Discounts.SingleOrDefault(x => x.Id == Cont.DiscountsID);

                contador = contador + 1;
                discountModel.IdTemp = contador;
                discountModel.Id = discountObject.Id;
                discountModel.IdCommerce = Cont.CommerceID;
                discountModel.Name = discountObject.Name;
                discountModel.Description = discountObject.Description;
                discountModel.Discount_Type = discountObject.Discount_Type;
                discountModel.Discount_value = discountObject.Discount_value;
                discountModel.Imagen = context.BusinessModels.FirstOrDefault((x => x.Id == context.BussinessCommerce.FirstOrDefault(x => x.Commerce == idCommerce).Bussines)).Image;
                var cant = context.Review.Where(x => x.IdDiscount == Cont.DiscountsID).Count();
                var cantCommerce = context.Review.Where(x => x.IDCommerce == Cont.CommerceID).Count();
                if (cant ==0)
                {
                    cant = 1;
                }
                if (cantCommerce == 0)
                {
                    cantCommerce = 1;
                }
                discountModel.Value = decimal.Round(context.Review.Where(x => x.IdDiscount == Cont.DiscountsID).Sum(y => y.Value)/ cant,1); 
                discountModel.Article = decimal.Round(context.Review.Where(x => x.IdDiscount == Cont.DiscountsID).Sum(y => y.Article) / cant,1) ;
                discountModel.CommerceValued = decimal.Round(context.Review.Where(x => x.IDCommerce == Cont.CommerceID).Sum(y => y.CommerceValued) / cantCommerce ,1);


                Discount.Add(discountModel);
            }

            if (Discount == null)
            {

                return Discount;
            }

            return Discount;

        }



        private string ObtenerIDUser()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            user = context.Users.FirstOrDefault(x => x.UserName.Equals(user)).Id;
            return user;
        }

        [HttpGet]
        [Route("[action]/{id_model}")]
        [Authorize(Roles = "Company")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult GetCommerceByModel(int id_model)
        {
            var user = ObtenerIDUser();
            var list = context.CompanyModelBussines.Where(y => y.Rut == (context.UsersCompany.FirstOrDefault(x => x.IdUser == user).idCompany)).ToList();
            var exist = false;

            foreach (CompanyModelBussines Cont in list)
            {
           if (id_model== Cont.Id)
                {

                    exist = true;
                }
            }
            if (exist) {
                
                SqlParameter parameterS = new SqlParameter("@Id", id_model);
            var comemer = context.Commerce.FromSql("SELECT * FROM COMMERCE WHERE ID IN (SELECT Commerce FROM BussinessCommerce where bussines=@Id)", parameterS).ToList();
            
            return Ok(new { results = comemer });
            }
            else
            {
                return BadRequest("El modelo no pertenece a la empresa.");

            }
            //return context.Commerce.ToList();
        }

    }
}