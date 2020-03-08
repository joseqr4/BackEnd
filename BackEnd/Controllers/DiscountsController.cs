using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    [Route("api/Discounts")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DiscountsController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        public DiscountsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/Discounts
        [HttpGet]

        [Authorize(Roles = "User")]
        public ActionResult Get()
        {



            var Discounts = context.Discounts.FromSql("select * from discounts where getdate()>=Date_Start and Getdate()<=Date_end").ToList();
            var DiscountListComerce = new List<modelDiscounts>();
            var CommerceDisscont = context.CommerceDiscounts.ToList();
            var objModel = new modelDiscounts();
            var cont = 0;
            foreach (CommerceDiscounts Cont in CommerceDisscont)
            {
                foreach (Discounts ContDiscount in Discounts)
                {
                    if (Cont.DiscountsID == ContDiscount.Id) { 
                    objModel = new modelDiscounts();
                        cont = cont + 1;
                        objModel.IdTemp = cont;
                        objModel.Id = Cont.DiscountsID;
                    objModel.IdCommerce = Cont.CommerceID;
                        objModel.Name = ContDiscount.Name;
                        objModel.Discount_value = ContDiscount.Discount_value;
                        objModel.Discount_Type = ContDiscount.Discount_Type;
                        objModel.Description = ContDiscount.Description;
                        objModel.Date_start = ContDiscount.Date_start;
                        objModel.Date_end = ContDiscount.Date_end;
                        var cant = context.Review.Where(x => x.IdDiscount == Cont.DiscountsID).Count();
                        var cantCommerce = context.Review.Where(x => x.IDCommerce == Cont.CommerceID).Count();
                        if (cant == 0)
                        {
                            cant = 1;
                        }
                        if (cantCommerce == 0)
                        {
                            cantCommerce = 1;
                        }
                        objModel.Value = decimal.Round(context.Review.Where(x => x.IdDiscount == Cont.DiscountsID).Sum(y => y.Value) / cant, 1);
                        objModel.Article = decimal.Round(context.Review.Where(x => x.IdDiscount == Cont.DiscountsID).Sum(y => y.Article) / cant, 1);
                        objModel.CommerceValued = decimal.Round(context.Review.Where(x => x.IDCommerce == Cont.CommerceID).Sum(y => y.CommerceValued) / cantCommerce, 1);
                        DiscountListComerce.Add(objModel);
                    }

                }

            }



            return Ok(new { results = DiscountListComerce });

            //return context.Commerce.ToList();
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(Roles = "Company")]

        public IActionResult GetByIdforCompany(int id)
        {
            var Discoun = context.Discounts.FirstOrDefault(x => x.Id == id);
            if (Discoun == null)
            {

                return NotFound("Descuento no encontrado");
            }
            var result = new DiscountsGets();

            result.Id = Discoun.Id;
            result.Name = Discoun.Name;
            result.Description = Discoun.Description;
            result.Discount_Type = Discoun.Discount_Type;
            result.Discount_value = Discoun.Discount_value;
            result.Date_end = Discoun.Date_end;
            result.Date_start = Discoun.Date_start;
            result.ListCommerce = new List<Commerce>();
            result.ListIntereses = new List<Interests>();


            var listaABorrar = context.CommerceDiscounts.Where(z => z.DiscountsID == id).ToList();
            var listaABorrarInterest = context.DiscountsInterests.Where(c => c.DiscountsID == id).ToList();

            foreach (CommerceDiscounts x in listaABorrar)
            {
                result.ListCommerce.Add(context.Commerce.FirstOrDefault(y => y.Id == x.CommerceID));
                //result.IdsCommerce.Add(x.CommerceID);
            }


            foreach (DiscountsInterests y in listaABorrarInterest)
            {
                result.ListIntereses.Add(context.Interests.FirstOrDefault(w => w.Id == y.InterestsId));
                //result.IdsIntereses.Add(y.InterestsId);
            }




            return Ok(result);

        }


        [HttpGet("{id}", Name = "DiscountCreate")]
        [Authorize(Roles = "Company")]

        public IActionResult GetById(int id)
        {
            var Discoun = context.Discounts.FirstOrDefault(x => x.Id == id);

            if (Discoun == null)
            {

                return NotFound("Descuento no encontrado");
            }

            return Ok(Discoun);

        }

        // POST: api/Discounts
        [HttpPost]
        [Authorize(Roles = "Company")]
        public IActionResult Post([FromBody] DiscountsCreate discount)
        {
            if (ModelState.IsValid)
            {
                var check = true;

                foreach (int id in discount.IdsCommerce) {

                    if (context.Commerce.Count(x => x.Id == id)==0) {
                        check = false;
                    }

                }

                if (check == true)
                {
                    var objD = new Discounts();
                    objD.Name = discount.Name;
                    objD.Description = discount.Description;
                    if (discount.Discount_Type.ToUpper() == "PERCENTAGE")
                    {
                        objD.Discount_Type = "Percentage";
                    }
                    else {
                        objD.Discount_Type = "Value";
                    }
                  
                    objD.Discount_value = discount.Discount_value;
                    objD.Date_start = discount.Date_start;
                    objD.Date_end = discount.Date_end;
                    context.Discounts.Add(objD);
                    context.SaveChanges();

                    var objCD = new CommerceDiscounts();
                    objCD.DiscountsID = objD.Id;

                    foreach (int id in discount.IdsCommerce)
                    {
                        objCD.CommerceID = id;
                        context.CommerceDiscounts.Add(objCD);
                        context.SaveChanges();
                    }
                    var objID = new DiscountsInterests();
                    objID.DiscountsID = objD.Id;

                    foreach (int id in discount.IdsIntereses)
                    {
                        objID.InterestsId = id;
                        context.DiscountsInterests.Add(objID);
                        context.SaveChanges();
                    }

                    return Ok();
                    }
                else {

                    return BadRequest("Uno de los comercios asociados no existe.");
                }

              
            }

            return BadRequest(ModelState);
        }


        [HttpPut]
        [Authorize(Roles = "Company")]
        public IActionResult Put([FromBody] DiscountsModify discount)
        {
            if (ModelState.IsValid)
            {
                var check = true;

                foreach (int id in discount.IdsCommerce)
                {

                    if (context.Commerce.Count(x => x.Id == id) == 0)
                    {
                        check = false;
                    }

                }

                if (check == true)
                {
                    var objD = context.Discounts.FirstOrDefault(y => y.Id == discount.Id);
                    objD.Name = discount.Name;
                    objD.Description = discount.Description;
                    //objD.Discount_Type = discount.Discount_Type;
                    //objD.Discount_value = discount.Discount_value;
                    objD.Date_start = discount.Date_start;
                    objD.Date_end = discount.Date_end;


                    context.Entry(objD).State = EntityState.Modified;
                    context.SaveChanges();


                    var objCD = new CommerceDiscounts();
                    objCD.DiscountsID = objD.Id;

                    var listaABorrar = context.CommerceDiscounts.Where(z => z.DiscountsID == objD.Id).ToList();
                    var listaABorrarInterest = context.DiscountsInterests.Where(c =>  c.DiscountsID == objD.Id).ToList();

                    foreach (CommerceDiscounts id in listaABorrar)
                    {
                        objCD.CommerceID = id.CommerceID;
                        context.CommerceDiscounts.Remove(context.CommerceDiscounts.FirstOrDefault(z => z.CommerceID== id.CommerceID && z.DiscountsID== objD.Id));
                        context.SaveChanges();
                    }


                    var objID = new DiscountsInterests();
                    objID.DiscountsID = objD.Id;

                    foreach (DiscountsInterests id2 in listaABorrarInterest)
                    {
                        objID.InterestsId = id2.InterestsId;
                        if (id2.InterestsId != 22) { 
                        context.DiscountsInterests.Remove(context.DiscountsInterests.FirstOrDefault(z => z.DiscountsID == objD.Id && z.InterestsId== id2.InterestsId));
                        context.SaveChanges();
                        }
                    }



                    foreach (int id in discount.IdsCommerce)
                    {
                        objCD.CommerceID = id;
                        context.CommerceDiscounts.Add(objCD);
                        context.SaveChanges();
                    }


                    foreach (int id in discount.IdsIntereses)
                    {
                        objID = new DiscountsInterests();
                        objID.DiscountsID = objD.Id;
                        objID.InterestsId = id;
                        if (id != 22) { 
                        context.DiscountsInterests.Add(objID);
                        context.SaveChanges();
                        }
                    }



                    return Ok();
                }
                else
                {

                    return BadRequest("Uno de los comercios asociados no existe.");
                }


            }

            return BadRequest(ModelState);
        }

      
        public class DiscountsCreate
        {

            public string Name { get; set; }

            public string Description { get; set; }

            [Column(TypeName = "decimal(18,2)")]
            public decimal Discount_value { get; set; }

            //Value _ Percentage
            public string Discount_Type { get; set; }

            public DateTime Date_start { get; set; }

            public DateTime Date_end { get; set; }

            public List<int> IdsCommerce { get; set; }

            public List<int> IdsIntereses { get; set; }


        }

        public class DiscountsModify
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public string Description { get; set; }

            [Column(TypeName = "decimal(18,2)")]
            public decimal Discount_value { get; set; }

            //Value _ Percentage
            public string Discount_Type { get; set; }

            public DateTime Date_start { get; set; }

            public DateTime Date_end { get; set; }

            public List<int> IdsCommerce { get; set; }

            public List<int> IdsIntereses { get; set; }


        }

        public class DiscountsGets
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public string Description { get; set; }

            [Column(TypeName = "decimal(18,2)")]
            public decimal Discount_value { get; set; }

            //Value _ Percentage
            public string Discount_Type { get; set; }

            public DateTime Date_start { get; set; }

            public DateTime Date_end { get; set; }

            public List<Commerce> ListCommerce { get; set; }

            public List<Interests> ListIntereses { get; set; }


        }


        private List<modelDiscounts> ListDescount(int idInterest)

        {


            var Discount = new List<Discounts>();
            var ListIdDiscount = new List<DiscountsInterests>();
            ListIdDiscount = context.DiscountsInterests.Where(x => x.InterestsId == idInterest).ToList();
            var DiscountListComerce = new List<modelDiscounts>();
            var CommerceDisscont = context.CommerceDiscounts.ToList();
            var objModel = new modelDiscounts();
          
            foreach (DiscountsInterests Cont in ListIdDiscount)
            {
                Discount.Add(context.Discounts.SingleOrDefault(x => x.Id == Cont.DiscountsID));
            }

            var cont = 0;
            foreach (CommerceDiscounts Cont in CommerceDisscont)
            {
                foreach (Discounts ContDiscount in Discount)
                {
                    if (Cont.DiscountsID == ContDiscount.Id)
                    {
                        objModel = new modelDiscounts();
                        cont = cont + 1;
                        objModel.IdTemp = cont;
                        objModel.Id = Cont.DiscountsID;
                        objModel.IdCommerce = Cont.CommerceID;
                        objModel.Name = ContDiscount.Name;
                        objModel.Discount_value = ContDiscount.Discount_value;
                        objModel.Discount_Type = ContDiscount.Discount_Type;
                        objModel.Description = ContDiscount.Description;
                        objModel.Date_start = ContDiscount.Date_start;
                        objModel.Date_end = ContDiscount.Date_end;
                        var cant = context.Review.Where(x => x.IdDiscount == Cont.DiscountsID).Count();
                        var cantCommerce = context.Review.Where(x => x.IDCommerce == Cont.CommerceID).Count();
                        if (cant == 0)
                        {
                            cant = 1;
                        }
                        if (cantCommerce == 0)
                        {
                            cantCommerce = 1;
                        }
                        objModel.Value = decimal.Round(context.Review.Where(x => x.IdDiscount == Cont.DiscountsID).Sum(y => y.Value) / cant, 1);
                        objModel.Article = decimal.Round(context.Review.Where(x => x.IdDiscount == Cont.DiscountsID).Sum(y => y.Article) / cant, 1);
                        objModel.CommerceValued = decimal.Round(context.Review.Where(x => x.IDCommerce == Cont.CommerceID).Sum(y => y.CommerceValued) / cantCommerce, 1);


                        DiscountListComerce.Add(objModel);
                    }

                }

            }




            if (DiscountListComerce == null)
            {

                return DiscountListComerce;
            }

            return DiscountListComerce;

        }

        //[HttpGet("{id}")]

        [Route("[action]/{id}")]
        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult GetByInterest(int id)
        {
            var Discounts = ListDescount(id);

            return Ok(new { results = Discounts});
            //return ListofInterests();


        }

        public class modelDiscounts
        {
            public int IdTemp { get; set; }
            public int Id { get; set; }
                       
            public int IdCommerce { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            [Column(TypeName = "decimal(18,2)")]
            public decimal Discount_value { get; set; }

            public string Discount_Type { get; set; }

            public DateTime Date_start { get; set; }

            public DateTime Date_end { get; set; }


            [Column(TypeName = "decimal(2,1)")]

            public Decimal Value { get; set; }
            [Column(TypeName = "decimal(2,1)")]

            public Decimal Article { get; set; }
            [Column(TypeName = "decimal(2,1)")]

            public Decimal CommerceValued { get; set; }


        }


        [Route("[action]/{id_model}")]
        [HttpGet]
        [Authorize(Roles = "Company")]
        public IActionResult GetDIscountModel(int id_model)
        {
            var user = ObtenerIDUser();
            var rut = context.UsersCompany.FirstOrDefault(y => y.IdUser == user).idCompany;
            if (context.CompanyModelBussines.Count((x => x.Id == id_model & x.Rut == rut)) > 0)
            {
                var list = new List<ModelDiscountModel>();


                SqlParameter parameterS = new SqlParameter("@Id", id_model);
                var discounts = context.Discounts.FromSql("select * from Discounts where id in (select DiscountsID from CommerceDiscounts where CommerceId in (select Commerce from BussinessCommerce where bussines =@Id)) ", parameterS).ToList();
                  var commerce = context.Commerce.FromSql("select * from  Commerce where id in (select commerce from BussinessCommerce where bussines=@Id) ", parameterS).ToList();
                var obj = new ModelDiscountModel();

                foreach (Discounts Cont in discounts)
                {
                    obj = new ModelDiscountModel();
                    obj.IdDiscount = Cont.Id;
                    obj.Discount_Type = Cont.Discount_Type;
                    obj.Discount_value = Cont.Discount_value;
                    obj.Name = Cont.Name;
                    var cant = context.Review.Where(x => x.IdDiscount == Cont.Id).Count();
                    if (cant == 0)
                    {
                        cant = 1;
                    }
                    SqlParameter parameterS2 = new SqlParameter("@IdDsicount", Cont.Id);
                    obj.Intereses= context.Interests.FromSql("select * from Interests where id in (select InterestsId from DiscountsInterests where discountsId=@IdDsicount)", parameterS2).ToList();
                    obj.Value =  decimal.Round(context.Review.Where(x => x.IdDiscount == Cont.Id).Sum(y => y.Value) / cant, 1);
                    obj.Activo = Cont.Date_start.ToShortDateString() + '-' + Cont.Date_end.ToShortDateString(); 
                    obj.qrRead= context.QrCode.FromSql("select * from qrcode where consumed=1 and idDiscount=@IdDsicount", parameterS2).Count();
                    var ultimoaagregar="";

                    foreach (Commerce com in commerce)
                    {
                        
                        if (context.CommerceDiscounts.Count(x => x.CommerceID==com.Id & x.DiscountsID==Cont.Id) >0)
                        {
                            if (ultimoaagregar == "")
                            {

                                if (obj.Locales is null)
                                {
                                    obj.Locales = com.Alias;
                                }
                                else
                                {
                                    ultimoaagregar = com.Alias;
                                }

                            }
                            else
                            {


                                obj.Locales = obj.Locales + ", " + ultimoaagregar;
                                ultimoaagregar = com.Alias;
                            }
                            
                        }

                    }
                    if (ultimoaagregar != "") { 
                        obj.Locales = obj.Locales + " y " + ultimoaagregar;
                    }
                    list.Add(obj);
                }
                return Ok(new { results = list });
            }
            else {
                return BadRequest("No existe el model para el commercio.");
            }

           
            //return ListofInterests();


        }

        public class ModelDiscountModel
        {

         public int IdDiscount { get; set; }

            public string Locales { get; set; }

            public string Name { get; set; }

            public List<Interests> Intereses { get; set; }
            //fecha de inicio del descuento
            public string Activo { get; set; }

            public int qrRead { get; set; }

            [Column(TypeName = "decimal(18,2)")]

            public decimal Discount_value { get; set; }

            //Value _ Percentage
            public string Discount_Type { get; set; }

            [Column(TypeName = "decimal(2,1)")]
            [DefaultValue(0)]
            public Decimal Value { get; set; }


        }

        private string ObtenerIDUser()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            user = context.Users.FirstOrDefault(x => x.UserName.Equals(user)).Id;
            return user;
        }

    }
}
