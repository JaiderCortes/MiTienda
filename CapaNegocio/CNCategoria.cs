using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNCategoria
    {
        private CDCategoria cdc = new CDCategoria();
        public List<Categoria> ListarCategorias()
        {
            return cdc.ListarCategorias();
        }

        public Guid? Registrar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción no puede ser vacía.";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return cdc.Registrar(obj, out Mensaje);
            }
            else
            {
                return null;
            }
        }

        public bool Editar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción no puede ser vacía.";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return cdc.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(Guid id, out string Mensaje)
        {
            return cdc.Eliminar(id, out Mensaje);
        }
    }
}
