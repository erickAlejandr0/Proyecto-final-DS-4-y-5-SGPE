using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.SqlClient;
using ProyectoDS54WebApi.Models;
using System.Data;

namespace ProyectoDS54WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        string conexion = "Data Source=localhost;Initial Catalog=PSICOLOGIABD;Integrated Security=True;TrustServerCertificate=True;";
       
        [HttpPost("Autenticar")]
        async public Task<IActionResult> Autenticar([FromBody] LoginParametros parametros)
        {
           
            try
            {
                using (SqlConnection conector = new SqlConnection(conexion))
                {


                    SqlCommand comando = new SqlCommand("AutenticarPsicologo", conector);
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@mail",parametros.mail);
                    comando.Parameters.AddWithValue("@pass",parametros.pass);
                    await conector.OpenAsync();

                    SqlDataReader reader = await comando.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                       
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

                        
                        return Ok(new
                        {
                            autenticado = true,
                            mensaje = "Autenticado",
                            data = jsonResultado 
                        });
                    }
                    else
                    {
                        return NotFound(new
                        {
                            autenticado = false,
                            mensaje = " NO autenticado",

                        });
                    }

                   
                }
            }
            catch (SqlException ex) 
            {
                Console.Write($"Error: {ex.Message}");
                return StatusCode(500, "Error interno del servidor." + ex.Message);
            }

        }


    }
}
