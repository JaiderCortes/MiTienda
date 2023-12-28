using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class GestionController : Controller
    {
        // GET: Gestion
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
    }
}