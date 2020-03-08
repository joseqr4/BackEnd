using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReviewController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        public ReviewController(ApplicationDbContext context)
        {
            this.context = context;
        }


        // GET: api/Review
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Review/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Review
        [HttpPost]
        public IActionResult Post([FromBody] ModelPostReview rev)
        {
            if (ModelState.IsValid)
            {
                var userid = ObtenerIDUser();

                var objReview = new Review();
                objReview.CommerceValued = rev.commerce_valued;
                objReview.IdDiscount = rev.id_discount;
                objReview.IDCommerce = rev.id_commerce;
                objReview.Article = rev.article;
                objReview.Value = rev.value;
                objReview.IdUsers = userid;
                objReview.IDCommerce= context.CommerceDiscounts.FirstOrDefault(x => x.DiscountsID==objReview.IdDiscount).CommerceID;
                objReview.DateValoration = DateTime.Today;
                context.Review.Add(objReview);
                context.SaveChanges();

                var qrcod = new QrCode();
                qrcod = context.QrCode.FirstOrDefault(x => x.Id == rev.id_qr);
                qrcod.Valued = true;
                context.Update(qrcod);
                context.SaveChanges();

                return Ok();
            }

            return BadRequest(ModelState);

        }

        private string ObtenerIDUser()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            user = context.Users.FirstOrDefault(x => x.UserName.Equals(user)).Id;
            return user;
        }
        public class ModelPostReview
        {
            public int id_qr { get; set; }

            public int id_discount { get; set; }

            public int id_commerce { get; set; }

            [Column(TypeName = "decimal(2,1)")]
            [ValidationReview(5)]
            [DefaultValue(1)]
            public Decimal value { get; set; }

            [Column(TypeName = "decimal(2,1)")]
            [ValidationReview(5)]
            [DefaultValue(1)]
            public Decimal article { get; set; }

            [Column(TypeName = "decimal(2,1)")]
            [ValidationReview(5)]
            [DefaultValue(1)]
            public Decimal commerce_valued { get; set; }
        }
        // PUT: api/Review/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
