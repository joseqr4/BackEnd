using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace BackEnd.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    //[ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext context;
        private readonly RoleManager<ApplicationUser> _RoleManager;
        private readonly IEmailSender _emailSender;
        private readonly IHostingEnvironment _env;
        public AccountController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration, IEmailSender emailSender, IHostingEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._configuration = configuration;
            this.context = context;
            _emailSender = emailSender;
            _env = env;
        }

        
 



        [Route("Create")]
        [HttpPost]
        //[ApiExplorerSettings(IgnoreApi  = true)]
        public async Task<IActionResult> CreateUser([FromBody] UserInfo model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email,Name= model.Name, Last_Name= model.Last_name, Email = model.Email, password_confirmation= model.Password_Confirmation};
                if (user.password_confirmation!= model.Password)
                {
                    return BadRequest(new { message = "Fallo en Confirmacion de Password" });
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var userr = context.Users.FirstOrDefault(x => x.Email == model.Email);
                    await _userManager.AddToRoleAsync(userr, "User");
                 
                    return BuildToken(userr, "User");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

      

        [Route("CreateCompany")]
        [HttpPost]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> CreateCompany([FromBody] ModelUserCompany model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.email, Name = model.Name,  Email = model.email ,Last_Name= model.Last_name};
                Microsoft.AspNetCore.Identity.SignInResult resultUser = await _signInManager.PasswordSignInAsync(model.email, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (resultUser.Succeeded) {
                    if (context.UsersCompany.Count(x=> x.IdUser== (context.Users.FirstOrDefault(y=> y.Email== model.email).Id))>0) {
                        return BadRequest("El Usuario ya posee una Empresa asignada");
                    }
                    else { 
                    await _userManager.AddToRoleAsync(context.Users.FirstOrDefault(x => x.Email == model.email), "Company");
                    if (context.Company.Count(x => x.Rut == model.Rut) > 0)
                    {
                      
                        var UserCompany = new UsersCompany();
                        UserCompany.idCompany = model.Rut;
                        UserCompany.IdUser = user.Id;
                        context.UsersCompany.Add(UserCompany);
                        context.SaveChanges();
                        //await _emailSender.SendEmailAsync(model.email, "Bienvenidos a la aplicación Aprovechapp!-.", CreateBodyWelcome(model)).ConfigureAwait(false);
                        return Ok();
                    }
                    else
                    {
                        var company = new Company();
                        company.email = model.email;
                        company.Name = model.Name;
                        company.Rut = model.Rut;
                        company.Name = model.NameBussines;
                        context.Company.Add(company);
                        context.SaveChanges();
                        var UserCompany = new UsersCompany();
                        UserCompany.idCompany = company.Rut;
                        UserCompany.IdUser = user.Id;
                        context.UsersCompany.Add(UserCompany);
                        context.SaveChanges();
                        //await _emailSender.SendEmailAsync(model.email, "Bienvenidos a la aplicación Aprovechapp!-.", CreateBodyWelcome(model)).ConfigureAwait(false);
                        return Ok();

                    }
                    }
                }
                else { 
                var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(context.Users.FirstOrDefault(x => x.Email == model.email), "Company");

                        if (context.Company.Count(x => x.Rut == model.Rut) >0)
                        {

                            var UserCompany = new UsersCompany();
                            UserCompany.idCompany = model.Rut;
                            UserCompany.IdUser = user.Id;
                            context.UsersCompany.Add(UserCompany);
                            context.SaveChanges();
                            //await _emailSender.SendEmailAsync(model.email, "Bienvenidos a la aplicación Aprovechapp!-.", CreateBodyWelcome(model)).ConfigureAwait(false);
                            return Ok();
                        }
                        else { 
                            
                        var company = new Company();
                        company.email = model.email;
                        company.Name = model.Name;
                        company.Rut = model.Rut;
                        company.Name = model.NameBussines;
                        context.Company.Add(company);
                        context.SaveChanges();
                        var UserCompany = new UsersCompany();
                        UserCompany.idCompany = company.Rut;
                        UserCompany.IdUser = user.Id;
                        context.UsersCompany.Add(UserCompany);
                        context.SaveChanges();
                        //await _emailSender.SendEmailAsync(model.email, "Bienvenidos a la aplicación Aprovechapp!-.", CreateBodyWelcome(model)).ConfigureAwait(false);
                        return Ok();
                        }
                    }
                    else
                    {
                        return BadRequest(result.Errors);
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        [Route("LoginCompany")]
     
        //public async Task<IActionResult> Login([FromBody] UserInfo userInfo)
        public async Task<IActionResult> LoginCompany([FromBody] ProfileViewModel userinfo)
        {
            if (ModelState.IsValid)
            {

                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(userinfo.email, userinfo.password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    
                    if (context.UsersCompany.FirstOrDefault(x => x.IdUser== context.Users.FirstOrDefault(y =>y.Email==userinfo.email).Id).Enable == true)
                    {
                        var userr = context.Users.FirstOrDefault(x => x.Email == userinfo.email);
                        return BuildToken(userr, "Company");
                    }
                    else
                    {
                        return BadRequest("El usuario aun no esta autorizado." );

                    }
                    
                }
                else
                {
                    ModelState.AddModelError("Code: 300",result.ToString() + "- Contraseña o Correo Incorrecto, reintente.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private string ObtenerIDUser()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            user = context.Users.FirstOrDefault(x => x.UserName.Equals(user)).Id;
            return user;
        }

        private string DestroyToken()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
           

            user = context.Users.FirstOrDefault(x => x.UserName.Equals(user)).Id;
            return user;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult GetDataUser()
        {
            var user = new ModelUserGet();
            var iduser = ObtenerIDUser();
            var userc = context.Users.FirstOrDefault(x => x.Id == iduser);
            user.Date_birth = userc.Date_birth;
            user.Last_Name = userc.Last_Name;
            user.Name = userc.Name;
            user.Photo = userc.Photo;
            user.Phone = userc.PhoneNumber;
            user.email = userc.Email;

            return Ok(new { results = user });
            //return context.Interests.ToList() ;
        }


        public class ModelUser {

            [StringLength(120)]
            public string Name { get; set; }
            [PersonalData]
            public string Last_Name { get; set; }

            [Column(TypeName = "nvarchar(Max)")]
            public String? Photo { get; set; }

            public DateTime? Date_birth { get; set; }

            public string? Phone { get; set; }



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



        public class ChangePassword
        {
            [StringLength(30, MinimumLength = 5)]
            public string Password_old { get; set; }

            [StringLength(30, MinimumLength = 5)]
            public string Password { get; set; }

            [StringLength(30, MinimumLength = 5)]
            public string Password_Confirmation { get; set; }

        }

        [HttpPut]
        [Route("Modify")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> Login([FromBody] UserInfo userInfo)
        [RequestSizeLimit(5242880)]
        public IActionResult Modify([FromBody] ModelUser userinfo)
        {
            if (ModelState.IsValid)
            {
                var iduser = ObtenerIDUser();
                var userc = context.Users.FirstOrDefault(x => x.Id == iduser);
                if (userinfo.Name != null)
                {
                    userc.Name = userinfo.Name;
                }
                if (userinfo.Last_Name != null)
                {
                    userc.Last_Name = userinfo.Last_Name;
                }
                if (userinfo.Phone != null)
                {
                    userc.PhoneNumber = userinfo.Phone;
                }
                if (userinfo.Photo != null)
                {
                   
                        userc.Photo = userinfo.Photo;
                                                        
                  
                }
                if (userinfo.Date_birth != null)
                {
                    userc.Date_birth = userinfo.Date_birth;
                }



                context.Entry(userc).State = EntityState.Modified;
                context.SaveChanges();

                var respuesta = new ModelUserGet();
                respuesta.Name = userc.Name;
                respuesta.Last_Name = userc.Last_Name;
                respuesta.Phone = userc.PhoneNumber;
                respuesta.Photo = userc.Photo;
                respuesta.Date_birth = userc.Date_birth;
                respuesta.email = userc.Email;
                return Ok(respuesta);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //public Image Base64ToImage(string base64String)
        //{
        //    // Convert base 64 string to byte[]
        //    byte[] imageBytes = Convert.FromBase64String(base64String);
        //    // Convert byte[] to Image
        //    using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
        //    {
        //        Image image = Image.FromStream(ms, true);
        //        return image;
        //    }
        //}



        [HttpPut]
        [Route("PasswordModify")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> Login([FromBody] UserInfo userInfo)
        public async Task<IActionResult> PasswordModify([FromBody] ChangePassword userinfo)
        {
            if (ModelState.IsValid)
            {
                if (userinfo.Password== userinfo.Password_Confirmation) { 
                var iduser = ObtenerIDUser();
                var userc = context.Users.FirstOrDefault(x => x.Id == iduser);

                var changePasswordResult = await _userManager.ChangePasswordAsync(userc, userinfo.Password_old, userinfo.Password);
                if (!changePasswordResult.Succeeded)
                {

                        return BadRequest(new { message = "Fallo en actualizar, contraseña antigua incorrecta o no valida contraseña nueva." });
                    }
                return Ok();
                }
                else
                {
                    return BadRequest(new { message = "Fallo en Confirmacion de Password" });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

  //      public string ImageToBase64(Image image,
  //System.Drawing.Imaging.ImageFormat format)
  //      {
  //          using (MemoryStream ms = new MemoryStream())
  //          {
  //              // Convert Image to byte[]
  //              image.Save(ms, format);
  //              byte[] imageBytes = ms.ToArray();

  //              // Convert byte[] to Base64 String
  //              string base64String = Convert.ToBase64String(imageBytes);
  //              return base64String;
  //          }
  //      }

        //private string RezizeImage(Image img, int maxWidth, int maxHeight)
        //{
           
        //    if (img.Height <= maxHeight && img.Width <= maxWidth) return "1";
        //    using (img)
        //    {
        //        Double xRatio = (double)img.Width / maxWidth;
        //        Double yRatio = (double)img.Height / maxHeight;
        //        Double ratio = Math.Max(xRatio, yRatio);
        //        int nnx = (int)Math.Floor(img.Width / ratio);
        //        int nny = (int)Math.Floor(img.Height / ratio);
        //        Bitmap cpy = new Bitmap(nnx, nny, PixelFormat.Format32bppArgb);
        //        using (Graphics gr = Graphics.FromImage(cpy))
        //        {
        //            gr.Clear(Color.Transparent);

        //            // This is said to give best quality when resizing images
        //            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;

        //            gr.DrawImage(img,
        //                new Rectangle(0, 0, nnx, nny),
        //                new Rectangle(0, 0, img.Width, img.Height),
        //                GraphicsUnit.Pixel);
        //        }



        //        return ImageToBase64(cpy,ImageFormat.Bmp);
        //    }

        //}

        //private MemoryStream BytearrayToStream(byte[] arr)
        //{
        //    return new MemoryStream(arr, 0, arr.Length);
        //}


        //private  String ResizeImageFile(string imageFile)
        //{
        //   int size = 640;
        //     int quality = 65;

        //    using (var image = new Bitmap(Base64ToImage(imageFile)))
        //    {
        //        int width, height;
        //        if (image.Width > image.Height)
        //        {
        //            width = size;
        //            height = Convert.ToInt32(image.Height * size / (double)image.Width);
        //        }
        //        else
        //        {
        //            width = Convert.ToInt32(image.Width * size / (double)image.Height);
        //            height = size;
        //        }
        //        var resized = new Bitmap(width, height);
        //        //Image result;
        //        using (var graphics = Graphics.FromImage(resized))
        //        {
        //            graphics.CompositingQuality = CompositingQuality.HighSpeed;
        //            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            graphics.CompositingMode = CompositingMode.SourceCopy;
        //            graphics.DrawImage(image, 0, 0, width, height);
                   
        //                var qualityParamId = System.Drawing.Imaging.Encoder.Quality;
        //                var encoderParameters = new EncoderParameters(1);
        //                encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
        //                var codec = ImageCodecInfo.GetImageDecoders()
        //                    .FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
        //            //resized.Save(result, codec, encoderParameters);
        //            return ImageToBase64(resized, ImageFormat.Bmp);
        //        }
        //    }
        //}
        //private static Size CalculateDimensions(Size oldSize, int targetSize)
        //{
        //    Size newSize = new Size();

        //    if (oldSize.Height > oldSize.Width)
        //    {
        //        newSize.Width = (int)(oldSize.Width * ((float)targetSize / (float)oldSize.Height));
        //        newSize.Height = targetSize;
        //    }

        //    else
        //    {
        //        newSize.Width = targetSize;
        //        newSize.Height = (int)(oldSize.Height * ((float)targetSize / (float)oldSize.Width));
        //    }

        //    return newSize;
        //}





        private string CreateBodyWelcome(ModelUserCompany model)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "TemplateEmail", "TemplateCreateCompany.html")))
            {

                body = reader.ReadToEnd();
            }

            body = body.Replace("{name}", model.Name);
            body = body.Replace("{namecomercial}", model.NameBussines);

            return body;

        }
    

        public class ModelUserCompany
        {
            public string Name { get; set; }

            public string Last_name { get; set; }
            [Required]
            public string email { get; set; }

            [StringLength(30, MinimumLength = 5)]
            public string Password { get; set; }

            [Compare("Password")]
            public string Password_Confirmation { get; set; }

            [Column(TypeName = "nvarchar(Max)")]


            public string? NameBussines { get; set; }

            [Required]

            public string emailBussines { get; set; }

             [Required]
            public Int64 Rut { get; set; }

            [Column(TypeName = "nvarchar(Max)")]
            public String? Image { get; set; }




        }


        [HttpDelete("Logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logged out");
        }




        // creacion de la asignacion de rol dado un usuario, endpoint interno, no se va a usar en ninguna aplicacion.
        [Route("AsignateRole")]
        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> AddRoleToUser(ModelRole userrole)
        {
            if (ModelState.IsValid)
            {

                if (userrole.Role == 1)
                {

                    Task<ApplicationUser> testUser = _userManager.FindByEmailAsync(userrole.email);
                    testUser.Wait();

                    if (testUser.Result == null)
                    {
                        return BadRequest("No existe el usuario con el email proporcionado");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(context.Users.FirstOrDefault(x => x.Email == userrole.email), "Company");
                        return Ok();
                    }


                        
                }
                else {
                    return Ok();
                }
                
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        public class ModelRole {

            public string email { get; set; }

            //Id 1 = Company
            //Id 2 = Member
            public int Role { get; set; }

        }


        [HttpPost]
        [Route("Login")]

        //public async Task<IActionResult> Login([FromBody] UserInfo userInfo)
        public async Task<IActionResult> Login([FromBody] ProfileViewModel userinfo)
        {
            if (ModelState.IsValid)
            {
           
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(userinfo.email, userinfo.password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var userr = context.Users.FirstOrDefault(x => x.Email == userinfo.email);
                    return BuildToken(userr, "User");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.ToString() +  "Invalid login attempt.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        public class ProfileViewModel
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        //private IActionResult BuildToken(UserInfo userInfo)
        private IActionResult BuildToken(ApplicationUser user, string role)
        {
            

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("somethingyouwantwhichissecurewillworkk"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName,user.Email),
                 new Claim(ClaimTypes.Name, user.Email),
                 //new Claim(ClaimTypes.NameIdentifier, t),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,role)

            };
           


            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "aprovechapp.com",
               audience: "aprovechapp.com",
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            string xtoken = new JwtSecurityTokenHandler().WriteToken(token);
            //token.Header.Add("Access-Token", xtoken);

            Response.Headers["Access-Token"] = xtoken;


            //    expiration = expiration



            //_configuration.app.Use(async (context, next) =>
            //{
            //    context.Response.OnStarting(() =>
            //    {
            //        context.Response.Headers.Add("Access-Token", xt);
            //        return Task.FromResult(0);
            //    });
            //    await next();
            //});


            //return asd;
            
            OkObjectResult Resp = Ok(Response);

            var res = new ModelUserGet();
            res.email = user.Email;
            res.Date_birth = user.Date_birth;
            res.Last_Name = user.Last_Name;
            res.Name = user.Name;
            res.Phone = user.PhoneNumber;
            res.Photo = user.Photo;

            Resp.Value = res;
            
            
            return Resp;

        }


private void AddRolesToClaims(List<Claim> claims, IEnumerable<string> roles)
{
    foreach (var role in roles)
    {
        var roleClaim = new Claim(ClaimTypes.Role, role);
        claims.Add(roleClaim);
    }
}


        
        //var user = await GetCurrentUserAsync();
        //var userId = user?.Id;

        //[HttpGet("{id}", Name = "InteresUsuario")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public IEnumerable<Interests> GetInterestsUser(string id)
        //{
        //    IEnumerable<Interests> Interests = context.InterestsUsuario.Where(x => x.UserNameID.Equals(id)).;

        //    if (Interests == null)
        //    {

        //        return NotFound();
        //    }

        //    return Ok(Interests);
        //}



    }

}
