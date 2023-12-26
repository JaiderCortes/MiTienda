using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Server;
using System.Reflection;

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

        public Guid Registrar(Usuario obj, out string Mensaje)
        {
            string id = "";
            Guid nuevoId;
            Mensaje = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarUsuario", con);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Id", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.ExecuteNonQuery();

                    id = Convert.ToString(cmd.Parameters["Id"].Value);
                    nuevoId = new Guid(id);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception e)
            {
                id = "";
                Mensaje = e.Message;
                Console.WriteLine($"Ocurrió un error durante la ejecución del procedimiento: {e.Message}");
                throw;
            }
            return nuevoId;
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("SP_EditarUsuario", con);
                    cmd.Parameters.AddWithValue("Id", obj.Id);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception e)
            {
                resultado = false;
                Mensaje = e.Message;
                Console.WriteLine($"Ocurrió un error durante la ejecución del procedimiento: {e.Message}");
                throw;
            }
            return resultado;
        }

        public bool Eliminar(Guid id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM USUARIO WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                resultado = false;
                Mensaje = e.Message;
                Console.WriteLine($"Ocurrió un error durante la ejecución del procedimiento: {e.Message}");
                throw;
            }
            return resultado;
        }

    }
}
