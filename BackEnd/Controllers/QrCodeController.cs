using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRCoder;


namespace BackEnd.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize(Roles = "User")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class QrCodeController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        public QrCodeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/QrCode
        [HttpGet("CreateQR")]
        public IActionResult Get(int idDiscount, int idCommerce)
        {
            ModelQrCreate xqr = new ModelQrCreate();
            xqr= Post(idDiscount, idCommerce);
           if (xqr == null){

                return BadRequest();
            }
            return Ok(new { results = xqr });
        }

  
        [HttpGet]
        public ActionResult Get()
        {
            var userid = ObtenerIDUser();
            var qrCodes = context.QrCode.Where(x => x.IdUser == userid && (x.Consumed == false || x.Valued == false)).ToList();

            //return Ok(new { results = qrCodes });
            var ListQR = new List<ModelQrDiscount>();
            var obj= new ModelQrDiscount();
            var objD = new Discounts();
            var objC = new Commerce();
            foreach (QrCode Cont in qrCodes)
            {
                obj = new ModelQrDiscount();
                objD= context.Discounts.FirstOrDefault(x => x.Id == Cont.IdDiscount);
                objC= context.Commerce.FirstOrDefault(x => x.Id == (context.CommerceDiscounts.FirstOrDefault(y =>y.DiscountsID==objD.Id).CommerceID));
                obj.IdDiscount = Cont.IdDiscount;
                obj.IdCommerce = Cont.IdCommerce;
                obj.qrId = Cont.Id;
                //obj.IdCommerce = objC.Id;
                obj.CommerceName = context.BusinessModels.FirstOrDefault(x => x.Id == context.BussinessCommerce.FirstOrDefault(y => y.Commerce == Cont.IdCommerce).Bussines).Name;
                obj.CommerceAddres = objC.Address;
                obj.Discount_value = objD.Discount_value;
                obj.Discount_Type = objD.Discount_Type;
                obj.InterestDescription = context.Interests.FirstOrDefault(x => x.Id == context.DiscountsInterests.FirstOrDefault(y =>y.DiscountsID==objD.Id).InterestsId).Name; ;
                obj.CommerceImage = context.BusinessModels.FirstOrDefault(x => x.Id == context.BussinessCommerce.FirstOrDefault(y => y.Commerce == Cont.IdCommerce).Bussines).Image;
                obj.Consumed = Cont.Consumed;
                ListQR.Add(obj);
            }

            return Ok(new { results = ListQR });


            //return context.Commerce.ToList();
        }

   


        public class ModelQrDiscount
        {
            public int qrId { get; set; }
            public int IdDiscount { get; set; }
            public int IdCommerce { get; set; }
            
            public string CommerceName{ get; set; }
            public string CommerceAddres { get; set; }

            public string CommerceImage { get; set; }

            public decimal Discount_value { get; set; }
            public string Discount_Type { get; set; }
            public string InterestDescription { get; set; }
            public Boolean Consumed { get; set; }


        }

        public class ModelQrCreate
        {

            public int Id { get; set; }

            public int IdDiscount { get; set; }

            public int IdCommerce { get; set; }

            public string IdUser { get; set; }

         
            [Column(TypeName = "nvarchar(Max)")]
            public String Image { get; set; }

       
            public Boolean Consumed { get; set; }

            public Boolean Valued { get; set; }



        }


        public class ModelQRSearch
        {
            public int Id { get; set; }

            public string Description { get; set; }

            public String Image { get; set; }


        }


        public class Consumedqr
        {
            public string qr_Info { get; set; }

        }

        public class ModelUserGet
        {

            [StringLength(120)]
            public string Name { get; set; }
            [PersonalData]
            public string Last_Name { get; set; }

            [Column(TypeName = "nvarchar(Max)")]
            public String? Photo { get; set; }

            public DateTime? Date_birth { get; set; }

            public string? Phone { get; set; }

            public string email { get; set; }

        }


        [Route("[action]")]
        [HttpPost]
        public IActionResult QrConsumed([FromBody] Consumedqr qrConsumed)
        {
            if (qrConsumed.qr_Info is null)
            {
                return BadRequest("qr_info es Null");
            }
            string cadenaresultante = qrConsumed.qr_Info;

            string[] ressultado = cadenaresultante.Split(',');
            int idqr = Convert.ToInt16(ressultado[0]);

            QrCode qr = context.QrCode.FirstOrDefault(x => x.Id == idqr);

            var user2 = ObtenerIDUser();

            if (context.UserCommerce.FirstOrDefault(x => x.IdUser == user2).CommerceID == qr.IdCommerce) { 


            if (qr.Consumed==true)
            {

                return BadRequest("Qr ya Consumido");
            }
            var user = context.Users.FirstOrDefault(y => y.Id == qr.IdUser);

            var UserResp = new ModelUserGet();

            UserResp.email = user.Email;
            UserResp.Date_birth = user.Date_birth;
            UserResp.Last_Name = user.Last_Name;
            UserResp.Name = user.Name;
            UserResp.Photo = user.Photo;
            UserResp.Phone = user.PhoneNumber;


            if (qr == null)
            {

                return BadRequest();
            }
            qr.Consumed = true;
            qr.DateConsumed = DateTime.Today;

            context.QrCode.Update(qr);
            context.SaveChanges();
                       
            return Ok(UserResp);
            }
            else
            {
                return BadRequest("El QR no pertenece al local.");

            }
        }


        [Route("[action]/{id}")]
        [HttpGet]
        public IActionResult GetQRId(int id)
        {
            QrCode qr = new QrCode();
            ModelQRSearch xqr = new ModelQRSearch();
            qr = context.QrCode.FirstOrDefault(x => x.Id==id);
            xqr.Id = qr.Id;
            xqr.Image = qr.Image;
            xqr.Description= context.Discounts.FirstOrDefault(x => x.Id==qr.IdDiscount).Description;

            if (xqr == null)
            {

                return BadRequest();
            }
            return Ok(new { results = xqr });
        }





        // POST: api/QrCode
        [HttpPost]
        [Produces("application/json")]
        private ModelQrCreate Post( [FromBody] int idDiscount, int idCommerce)
        {
            QrCode xqr = new QrCode();
            ModelQrCreate xqrResp = new ModelQrCreate();
            xqr.IdDiscount = idDiscount;
            xqr.IdCommerce = idCommerce;
            var userid = ObtenerIDUser();
            xqr.IdUser = userid;
            xqr.TimeValidation = DateTime.Today.AddDays(1);
            xqr.DateCreate = DateTime.Today;


            if (xqr.IdDiscount > 0) { 
                context.QrCode.Add(xqr);
                context.SaveChanges();

                Put(xqr);
                xqrResp.Id = xqr.Id;
                xqrResp.IdCommerce = xqr.IdCommerce;
                xqrResp.IdDiscount = xqr.IdDiscount;
                xqrResp.IdUser = xqr.IdUser;
                xqrResp.Image = xqr.Image;
                xqrResp.Valued = xqr.Valued;



                return xqrResp;
            }

            return null;

        }

        private string ObtenerIDUser()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            user = context.Users.FirstOrDefault(x => x.UserName.Equals(user)).Id;
            return user;
        }

        // PUT: api/QrCode/5
        [HttpPut]
        private void Put( [FromBody] QrCode value)
        {
            context.Update(QrGeneration(value));
            context.SaveChanges();
                    
        }


        private QrCode QrGeneration(QrCode xobj)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            var text = xobj.Id + "," + xobj.IdDiscount + "," + xobj.IdUser;
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text,
            QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            System.IO.MemoryStream ms = new MemoryStream();
            qrCodeImage.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            xobj.Image = Convert.ToBase64String(byteImage);

            //xobj.Imagen = ImageToBase64(qrCodeImage, System.Drawing.Imaging.ImageFormat.Bmp);

            return xobj;
        }

        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        private string ImageToBase64(Image image,
  System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
