using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Globalization;
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
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Company")]

    public class StatisticsController : ControllerBase
    {

       

        private readonly ApplicationDbContext context;
        public StatisticsController(ApplicationDbContext context)
        {
            this.context = context;
        }



     
        private string ObtenerIDUser()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            user = context.Users.FirstOrDefault(x => x.UserName.Equals(user)).Id;
            return user;
        }


        [HttpGet]
        [Route("[action]")]

        public IActionResult Qrdataconversion(int id_model, string fecha_ini, string Fecha_Fin)
        {

            var user = ObtenerIDUser();
            var rut = context.UsersCompany.FirstOrDefault(y => y.IdUser == user).idCompany;
            var listfin = new List<ModelRespuestaEstadisticas>();
            if (context.CompanyModelBussines.Count((x => x.Id == id_model & x.Rut == rut)) > 0)
            {
                SqlParameter parameterS = new SqlParameter("@Id", id_model);
                var resp = new ModelRespuestaEstadisticas();



                DateTime fechaini = Convert.ToDateTime(fecha_ini.Replace('"', ' ').Trim(), new CultureInfo("fr-FR"));
                DateTime fechafin = Convert.ToDateTime(Fecha_Fin.Replace('"', ' ').Trim(), new CultureInfo("fr-FR"));

                if (fechaini > fechafin)
                {

                    return BadRequest("Rango fechas incorrecto");
                }
                else {

                    int mesini = Convert.ToInt16(fechaini.Month);
                    int añoini = Convert.ToInt16(fechaini.Year);
                    int mesfin = Convert.ToInt16(fechafin.Month);
                    int añofin = Convert.ToInt16(fechafin.Year);

                    for (int y = añoini; y <= añofin; y++) {

                        if (y == añofin) {

                            for (int i = mesini; i <= mesfin; i++)
                            {




                                SqlParameter month = new SqlParameter("@Month", i);
                                SqlParameter year = new SqlParameter("@year", y);


                                var Cant_qr_Escaneados = context.QrCode.FromSql("select * from QrCode where idcommerce in (select Commerce from BussinessCommerce where Bussines=@Id ) and consumed=1 and  MONTH(DateConsumed)= @Month and Year(DateConsumed)= @year ", parameterS, month, year).Count();
                                var Cant_descuentos_en = 0;
                                var Can_descarga_qrs = context.QrCode.FromSql("select * from QrCode where idcommerce in (select Commerce from BussinessCommerce where Bussines=@Id ) and MONTH(DateCreate)= @Month and Year(DateCreate)= @year  ", parameterS, month, year).Count();
                                resp = new ModelRespuestaEstadisticas();
                                resp.ano = y;
                                resp.mes = i;
                                resp.cant_descarga_qr = Can_descarga_qrs;
                                resp.cant_escaneados = Cant_qr_Escaneados;
                                resp.cant_descuentos_entraron = Cant_descuentos_en;
                                listfin.Add(resp);



                            }


                        }
                        else { 
                        for (int i = mesini; i <= 12 ; i++)
                    {




                                SqlParameter month = new SqlParameter("@Month", i);
                                SqlParameter year = new SqlParameter("@year", y);


                                var Cant_qr_Escaneados = context.QrCode.FromSql("select * from QrCode where idcommerce in (select Commerce from BussinessCommerce where Bussines=@Id ) and consumed=1 and  MONTH(DateConsumed)= @Month and Year(DateConsumed)= @year ", parameterS, month,year).Count();
                        var Cant_descuentos_en = 0;
                        var Can_descarga_qrs = context.QrCode.FromSql("select * from QrCode where idcommerce in (select Commerce from BussinessCommerce where Bussines=@Id ) and MONTH(DateCreate)= @Month and Year(DateCreate)= @year  ", parameterS, month, year).Count();
                                resp= new ModelRespuestaEstadisticas();
                                resp.ano = y;
                            resp.mes = i;
                        resp.cant_descarga_qr = Can_descarga_qrs;
                        resp.cant_escaneados = Cant_qr_Escaneados;
                        resp.cant_descuentos_entraron = Cant_descuentos_en;
                            listfin.Add(resp);
                    

         
                    }
                        }

                        mesini = 1;

                    }





                }



                return Ok(new { results = listfin });
            }
            else
            {
                return BadRequest("No existe el modelo para la empresa");
            }
        }


        [HttpGet]
        [Route("[action]")]

        public IActionResult QrDataGeneration(int id_model, int mes)
        {

            var user = ObtenerIDUser();
            var rut = context.UsersCompany.FirstOrDefault(y => y.IdUser == user).idCompany;
            if (context.CompanyModelBussines.Count((x => x.Id == id_model & x.Rut == rut)) > 0)
            {
                var mesactual = DateTime.Now.Month;
                var Anoactual = DateTime.Now.Year;
                if (mes > mesactual) {
                    Anoactual = Anoactual - 1;
                }

                SqlParameter parameterS = new SqlParameter("@Id", id_model);
                SqlParameter month = new SqlParameter("@Month", mes);
                SqlParameter year = new SqlParameter("@year", Anoactual);
                var resp = new ModelDiscountModel();
                var lisResult = new List<ModelDiscountModel>();
           
                var Listdiscount = context.Discounts.FromSql("select * from discounts where id in (select t1.iddiscount from (select  top 5 iddiscount,count(*) as cantidad from QrCode where idcommerce in (select Commerce from BussinessCommerce where Bussines = @Id ) and consumed = 1 and MONTH(DateConsumed)= @Month and Year(DateConsumed)= @year group by iddiscount  order by cantidad desc) as t1)", parameterS, month, year).ToList();

                foreach (Discounts Cont in Listdiscount)
                {
                    resp = new ModelDiscountModel();
                    resp.IdDiscount = Cont.Id;
                    resp.Discount_Type = Cont.Discount_Type;
                    resp.Discount_value = Cont.Discount_value;
                    resp.Name = Cont.Name;
                    resp.Description = Cont.Description;
                    var cant = context.Review.Where(x => x.IdDiscount == Cont.Id).Count();
                    if (cant == 0)
                    {
                        cant = 1;
                    }

                    resp.Value = decimal.Round(context.Review.Where(x => x.IdDiscount == Cont.Id).Sum(y => y.Value) / cant, 1);
                    SqlParameter iddisc = new SqlParameter("@IdDiscount", Cont.Id);
                    resp.cantConsumed = context.QrCode.FromSql("select * from QrCode where idcommerce in (select Commerce from BussinessCommerce where Bussines = @Id ) and consumed = 1 and MONTH(DateConsumed)= @Month and Year(DateConsumed)= @year and IdDiscount=@IdDiscount", parameterS, month, year, iddisc).Count();
                    lisResult.Add(resp);

                }




                return Ok(new { results = lisResult });
            }
            else
            {
                return BadRequest("No existe el modelo para la empresa");
            }
        }

        public class ModelDiscountModel
        {

            public int IdDiscount { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }


            public int cantConsumed { get; set; }

            [Column(TypeName = "decimal(18,2)")]

            public decimal Discount_value { get; set; }

            //Value _ Percentage
            public string Discount_Type { get; set; }

            [Column(TypeName = "decimal(2,1)")]
            [DefaultValue(0)]
            public Decimal Value { get; set; }


        }

        public class ModelRespuestaEstadisticas
        {
            public int mes { get; set; }

            public int ano { get; set; }

            public int cant_descuentos_entraron { get; set; }

            public int cant_descarga_qr { get; set; }

            public int cant_escaneados { get; set; }

        }

        public class ModelRespuestaQR
        {
            public int cant_descuentros_entraron { get; set; }

            public int cant_descarga_qr { get; set; }

            public int cant_escaneados { get; set; }

        }

        

        public class ModelRespuestaAnality
        {
            public int valoracion_Actual { get; set; }
            public Decimal valoracion_Anterior_Porcentaje { get; set; }
            public int qr_consumidos_Actual { get; set; }
            public Decimal qr_Consumidos_Anterior_Porcentaje { get; set; }
            public int cliente_incremento_actual { get; set; }
            public Decimal cliente_incremento_porcentaje { get; set; }



        }



        [HttpGet]
        [Route("[action]")]

        public IActionResult DataAnalityc()
        {

            var user = ObtenerIDUser();
            var rut = context.UsersCompany.FirstOrDefault(y => y.IdUser == user).idCompany;
            var respuesta = new ModelRespuestaAnality();

            var mesactual = DateTime.Now.Month;
            var Anoactual = DateTime.Now.Year;

            var mesanterior = DateTime.Now.Month-1;
            var Anoanterior = DateTime.Now.Year;

            if (mesactual == 1)
            {
                mesanterior = 12;
                Anoanterior = Anoactual - 1;

            }

            SqlParameter parameterS = new SqlParameter("@Rut", rut);
            SqlParameter month = new SqlParameter("@Month", mesactual);
            SqlParameter year = new SqlParameter("@year", Anoactual);

            SqlParameter monthAnt = new SqlParameter("@Month", mesanterior);
            SqlParameter yearAnt = new SqlParameter("@year", Anoanterior);

            respuesta.valoracion_Actual = context.Review.FromSql("select * from Review where idcommerce in (select Commerce from BussinessCommerce where Bussines in (select id from CompanyModelBussines where rut=@Rut) )", parameterS).Count();

            var valorantesqr = context.Review.FromSql("select * from Review where idcommerce in (select Commerce from BussinessCommerce where Bussines in (select id from CompanyModelBussines where rut=@Rut) ) and MONTH(DateValoration)= @Month and Year(DateValoration)= @year", parameterS, monthAnt, yearAnt).Count();

            if (valorantesqr == 0)
            {

                valorantesqr = 1;
            }
            respuesta.valoracion_Anterior_Porcentaje = ((respuesta.valoracion_Actual * 100) / valorantesqr) - 100;


            //----------------------------------------------
            respuesta.qr_consumidos_Actual = context.QrCode.FromSql("select * from QrCode where idcommerce in (select Commerce from BussinessCommerce where Bussines in (select id from CompanyModelBussines where rut=@Rut) ) and consumed = 1 ", parameterS).Count();

            var valorantQrConsumido = context.QrCode.FromSql("select * from QrCode where idcommerce in (select Commerce from BussinessCommerce where Bussines in (select id from CompanyModelBussines where rut=@Rut) ) and consumed = 1 and MONTH(DateConsumed)= @Month and Year(DateConsumed)= @year", parameterS, monthAnt, yearAnt).Count();

            if (valorantQrConsumido == 0)
            {

                valorantQrConsumido = 1;
            }
            respuesta.qr_Consumidos_Anterior_Porcentaje = ((respuesta.qr_consumidos_Actual * 100) / valorantQrConsumido) - 100;

            //----------------------------------------------

            respuesta.cliente_incremento_actual = context.QrCode.FromSql("select distinct(IdUser) from QrCode where idcommerce in (select Commerce from BussinessCommerce where Bussines in (select id from CompanyModelBussines where rut=@Rut) ) and consumed = 1", parameterS).Count();

            var valorantClientes = context.QrCode.FromSql("select distinct(IdUser) from QrCode where idcommerce in (select Commerce from BussinessCommerce where Bussines in (select id from CompanyModelBussines where rut=@Rut) ) and consumed = 1 and MONTH(DateConsumed)= @Month and Year(DateConsumed)= @year", parameterS, monthAnt, yearAnt).Count();

            if (valorantClientes == 0)
            {

                valorantClientes = 1;
            }

            respuesta.cliente_incremento_porcentaje = ((respuesta.cliente_incremento_actual * 100) / valorantClientes) - 100;




            return Ok(respuesta);
            
        }


    }
}
