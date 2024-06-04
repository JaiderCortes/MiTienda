using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaDatos
{
    public class CDCliente
    {
        public List<Cliente> ListarClientes()
        {
            List<Cliente> clientes = new List<Cliente>();

            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    string sql = "SELECT Id, Nombres, Apellidos, Correo, Clave, Reestablecer FROM CLIENTE;";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.CommandType = System.Data.CommandType.Text;

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Cliente cliente = new Cliente
                            {
                                Id = rdr.GetGuid(rdr.GetOrdinal("Id")),
                                Nombres = rdr["Nombres"].ToString(),
                                Apellidos = rdr["Apellidos"].ToString(),
                                Correo = rdr["Correo"].ToString(),
                                Clave = rdr["Clave"].ToString(),
                                Reestablecer = Convert.ToBoolean(rdr["Reestablecer"])
                            };
                            clientes.Add(cliente);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                clientes = new List<Cliente>();
                Console.WriteLine($"Ocurrió un error durante la ejecución del procedimiento: {e.Message}");
                throw;
            }

            return clientes;
        }

        public Guid Registrar(Cliente cliente, out string Mensaje)
        {
            string id = "";
            Guid nuevoId;
            Mensaje = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarCliente", con);
                    cmd.Parameters.AddWithValue("Nombres", cliente.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", cliente.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", cliente.Correo);
                    cmd.Parameters.AddWithValue("Clave", cliente.Clave);
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

        public bool CambiarClave(Guid idCliente, string nuevaClave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("UPDATE CLIENTE SET Clave = @NuevaClave, Reestablecer = 0 WHERE Id = @IdCliente;", con);
                    cmd.Parameters.AddWithValue("@NuevaClave", nuevaClave);
                    cmd.Parameters.AddWithValue("@IdCliente", idCliente);
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

        public bool ReestablecerClave(Guid idCliente, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("UPDATE CLIENTE SET Clave = @Clave, Reestablecer = 1 WHERE Id = @IdCliente;", con);
                    cmd.Parameters.AddWithValue("@Clave", clave);
                    cmd.Parameters.AddWithValue("@IdCliente", idCliente);
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
