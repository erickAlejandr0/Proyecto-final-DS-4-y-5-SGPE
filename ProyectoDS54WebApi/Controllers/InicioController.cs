using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProyectoDS54WebApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProyectoDS54WebApi.Controllers
{
    [Route("api/[controller]")]
    public class InicioController : Controller
    {
        
        string conexion = "Data Source=localhost;Initial Catalog=PSICOLOGIABD;Integrated Security=True;TrustServerCertificate=True;";
        [HttpGet("ObtenerPacientes")]
        public async Task<IActionResult> ObtenerPacientes([FromBody] PsicologosParametros parametros)
        {
            try
            {

                using (SqlConnection conector = new SqlConnection(conexion))
                {
                    SqlCommand comando = new SqlCommand("ObtenerPacientes_PorPsicologo", conector);
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@ID_psicologo", parametros.id);
                    await conector.OpenAsync();
                    SqlDataReader reader = await comando.ExecuteReaderAsync();
                    

                    var jsonResultado = new List<Dictionary<string, object>>();
                    while (reader.Read()) 
                    { 
                        var filaDicc = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            filaDicc[reader.GetName(i)] = reader.GetValue(i);
                        }
                        jsonResultado.Add(filaDicc);


                    }
                   return Ok(jsonResultado);

                    
                }
            }
            catch (Exception ex) 
            {
                
            }
            return Ok(new
            {
               
                   
            });
        }
    }
}
