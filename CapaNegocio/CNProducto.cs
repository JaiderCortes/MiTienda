using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNProducto
    {
        private CDProducto cdp = new CDProducto();
        public List<Producto> ListarProductos()
        {
            return cdp.ListarProductos();
        }

        public Guid Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (obj.IdMarca.Id == Guid.Empty)
            {
                Mensaje = "Debe seleccionar una marca.";
            }
            else if (obj.IdCategoria.Id == Guid.Empty)
            {
                Mensaje = "Debe seleccionar una categoría.";
            }
            else if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre no puede ser vacío.";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción no puede ser vacía.";
            }
            else if (obj.Precio == 0)
            {
                Mensaje = "Ingrese el precio del producto.";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Ingrese el stock del producto.";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                return cdp.Registrar(obj, out Mensaje);
            }
            else
            {
                return Guid.Empty;
            }
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (obj.IdMarca.Id == Guid.Empty)
            {
                Mensaje = "Debe seleccionar una marca.";
            }
            else if (obj.IdCategoria.Id == Guid.Empty)
            {
                Mensaje = "Debe seleccionar una categoría.";
            }
            else if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre no puede ser vacío.";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción no puede ser vacía.";
            }
            else if (obj.Precio == 0)
            {
                Mensaje = "Ingrese el precio del producto.";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Ingrese el stock del producto.";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return cdp.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(Guid id, out string Mensaje)
        {
            return cdp.Eliminar(id, out Mensaje);
        }

        public bool GuardarImagen(Producto obj, out string Mensaje)
        {
            return cdp.GuardarImagen(obj, out Mensaje);
        }
    }
}
