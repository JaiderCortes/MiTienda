using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Server;

namespace CapaDatos
{
    public class CDUsuarios
    {
        public List<Usuario> ListarUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    string sql = "SELECT Id, Nombres, Apellidos, Correo, Clave, Reestablecer, Activo FROM Usuario;";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Usuario usuario = new Usuario
                            {
                                Id = rdr.GetGuid(rdr.GetOrdinal("Id")),
                                Nombres = rdr["Nombres"].ToString(),
                                Apellidos = rdr["Apellidos"].ToString(),
                                Correo = rdr["Correo"].ToString(),
                                Clave = rdr["Clave"].ToString(),
                                Reestablecer = Convert.ToBoolean(rdr["Reestablecer"]),
                                Activo = Convert.ToBoolean(rdr["Activo"])
                            };
                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                usuarios = new List<Usuario>();
                Console.WriteLine($"Ocurrió un error durante la ejecución del procedimiento: {e.Message}");
                throw;
            }

            return usuarios;
        }
    }
}
