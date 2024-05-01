using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class GestionController : Controller
    {
        // GET: Gestion
        #region Pages
        public ActionResult Categorias()
        {
            return View();
        }
        public ActionResult Marcas()
        {
            return View();
        }
        public ActionResult Productos()
        {
            return View();
        }
        #endregion Pages

        #region GestionCategorías
        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> categorias;
            categorias = new CNCategoria().ListarCategorias();
            return Json(new { data = categorias }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria categoria)
        {
            object result;
            string mensaje = string.Empty;

            if (categoria.Id == Guid.Empty)
            {
                result = new CNCategoria().Registrar(categoria, out mensaje);
            }
            else
            {
                result = new CNCategoria().Editar(categoria, out mensaje);

            }
            return Json(new { result = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(Guid idCategoria)
        {
            bool result = false;
            string mensaje = string.Empty;

            result = new CNCategoria().Eliminar(idCategoria, out mensaje);

            return Json(new { result = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion GestionCategorías

        #region GestionMarcas
        [HttpGet]
        public JsonResult ListarMarcas()
        {
            List<Marca> marcas;
            marcas = new CNMarca().ListarMarcas();
            return Json(new { data = marcas }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca marca)
        {
            object result;
            string mensaje = string.Empty;

            if (marca.Id == Guid.Empty)
            {
                result = new CNMarca().Registrar(marca, out mensaje);
            }
            else
            {
                result = new CNMarca().Editar(marca, out mensaje);

            }
            return Json(new { result = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(Guid idMarca)
        {
            bool result = false;
            string mensaje = string.Empty;

            result = new CNMarca().Eliminar(idMarca, out mensaje);

            return Json(new { result = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion GestionMarcas

        #region GestionProductos
        [HttpGet]
        public JsonResult ListarProductos()
        {
            List<Producto> productos;
            productos = new CNProducto().ListarProductos();
            return Json(new { data = productos }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase imagen)
        {
            string mensaje = string.Empty;
            bool exito = true;
            bool guardarImagenExito = true;

            Producto producto = new Producto();
            producto = JsonConvert.DeserializeObject<Producto>(objeto);

            //producto.Id = Guid.Empty;

            if (producto.PrecioTexto == "")
            {
                producto.PrecioTexto = "0";
            }

            decimal precio = Convert.ToDecimal(producto.PrecioTexto);
            if (decimal.TryParse(producto.PrecioTexto, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("es-CO"), out precio))
            {
                producto.Precio = precio;
            }
            else
            {
                return Json(new { exito = false, mensaje = "Formato de precio incorrecto." }, JsonRequestBehavior.AllowGet);
            }

            if (producto.Id == Guid.Empty)
            {
                Guid productoCreado = new CNProducto().Registrar(producto, out mensaje);
                if(productoCreado != Guid.Empty)
                {
                    producto.Id = productoCreado;
                }
                else
                {
                    exito = false;
                }
            }
            else
            {
                exito = new CNProducto().Editar(producto, out mensaje);

            }

            if(exito)
            {
                //string rutaFisica = Server.MapPath("~/Imagenes/");
                if(imagen != null)
                {
                    //string rutaImagen = ConfigurationManager.AppSettings["ServidorFotos"];
                    string rutaImagen = Server.MapPath(ConfigurationManager.AppSettings["ServidorFotos"]);
                    //string rutaImagen = rutaFisica; //Pruebas
                    string extensionImagen = Path.GetExtension(imagen.FileName);
                    string nombreImagen = string.Concat(producto.Id.ToString(), extensionImagen);

                    try
                    {
                        imagen.SaveAs(Path.Combine(rutaImagen, nombreImagen));
                    }
                    catch (Exception e)
                    {
                        string msg = e.Message;
                        guardarImagenExito = false;
                        throw;
                    }

                    if (guardarImagenExito)
                    {
                        producto.RutaImagen = rutaImagen;
                        producto.NombreImagen = nombreImagen;
                        bool respuesta = new CNProducto().GuardarImagen(producto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardó el producto pero no se pudo guardar la imagen.";
                    }
                }

            }

            return Json(new { result = exito, idGenerado = producto.Id, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImagenProducto(Guid idProducto)
        {
            bool conversion;
            Producto producto = new CNProducto().ListarProductos().Where(p => p.Id == idProducto).FirstOrDefault();
            string textBase64 = CNRecursos.ConvertirBase64(Path.Combine(producto.RutaImagen, producto.NombreImagen), out conversion);

            return Json(new {
                conversion = conversion,
                textBase64 = textBase64,
                extension = Path.GetExtension(producto.NombreImagen)
            },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarProducto(Guid idProducto)
        {
            bool result = false;
            string mensaje = string.Empty;

            result = new CNProducto().Eliminar(idProducto, out mensaje);

            return Json(new { result = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion GestionProductos
    }
}