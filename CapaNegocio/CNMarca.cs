using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNMarca
    {
        private CDMarca cdm = new CDMarca();
        public List<Marca> ListarMarcas()
        {
            return cdm.ListarMarcas();
        }

        public Guid? Registrar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción no puede ser vacía.";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return cdm.Registrar(obj, out Mensaje);
            }
            else
            {
                return null;
            }
        }

        public bool Editar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción no puede ser vacía.";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return cdm.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(Guid id, out string Mensaje)
        {
            return cdm.Eliminar(id, out Mensaje);
        }
    }
}
