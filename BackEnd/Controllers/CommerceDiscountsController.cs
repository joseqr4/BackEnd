using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Produces("application/json")]
    [Route("api/CommerceDiscounts")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommerceDiscountsController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        public CommerceDiscountsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        // GET: api/CommerceDiscounts
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "" };
        //}

 

        //// POST: api/CommerceDiscounts
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}


        //[HttpGet("{id}", Name = "GetDiscountInterest")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        ////private List<Commerce> ListCommerce(int idInterest)

        ////{


        ////    var Commerces = new List<CommerceInterests>();
        ////    Commerces = context.CommerceInterests.Where(x => x.InterestsId == idInterest).ToList();



        ////    var ListIdDiscount = new List<DiscountsInterests>();
        ////    ListIdDiscount = context.DiscountsInterests.Where(x => x.InterestsId == idInterest).ToList();

        ////    foreach (DiscountsInterests Cont in ListIdDiscount)
        ////    {
        ////        Discount.Add(context.Discounts.SingleOrDefault(x => x.Id == Cont.DiscountsID));
        ////    }


        ////    if (Discount == null)
        ////    {

        ////        return Discount;
        ////    }

        ////    return Discount;

        ////}

        //// PUT: api/CommerceDiscounts/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
