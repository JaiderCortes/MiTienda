using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CDCategoria
    {
        public List<Categoria> ListarCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();

            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    string sql = "SELECT Id, Descripcion, Activo FROM CATEGORIA;";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Categoria categoria = new Categoria
                            {
                                Id = rdr.GetGuid(rdr.GetOrdinal("Id")),
                                Descripcion = rdr["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(rdr["Activo"])
                            };
                            categorias.Add(categoria);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                categorias = new List<Categoria>();
                Console.WriteLine($"Ocurrió un error durante la ejecución del procedimiento: {e.Message}");
                throw;
            }

            return categorias;
        }

        public Guid Registrar(Categoria obj, out string Mensaje)
        {
            string id = "";
            Guid nuevoId;
            Mensaje = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarCategoria", con);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
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

        public bool Editar(Categoria obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("SP_EditarCategoria", con);
                    cmd.Parameters.AddWithValue("Id", obj.Id);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
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
                    SqlCommand cmd = new SqlCommand("SP_EliminarCategoria", con);
                    cmd.Parameters.AddWithValue("Id", id);
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
    }
}
