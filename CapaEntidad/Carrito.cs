using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Carrito
    {
        public Guid Id { get; set; }
        public Cliente Idcliente { get; set; }
        public Producto IdProducto { get; set; }
        public int Cantidad { get; set; }
    }
}
