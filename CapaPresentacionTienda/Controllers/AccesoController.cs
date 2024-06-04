using Antlr.Runtime.Tree;
using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registro()
        {
            return View();
        }

        public ActionResult ReestablecerClave()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registro(Cliente cliente)
        {
            object result;
            string mensaje = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(cliente.Nombres) ? "" : cliente.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(cliente.Apellidos) ? "" : cliente.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(cliente.Correo) ? "" : cliente.Correo;

            if (cliente.Clave != cliente.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }
            else
            {
                result = new CNCliente().Registrar(cliente, out mensaje);
                if (result != null)
                {
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Acceso");
                }
                else
                {
                    ViewBag.Error = mensaje;
                    return View();
                }
            }
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Cliente cliente = new Cliente();
            cliente = new CNCliente().ListarClientes().Where(cli => cli.Correo == correo && cli.Clave == CNRecursos.EncriptarSha256(clave)).FirstOrDefault();
            if (cliente == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }
            else
            {
                if (cliente.Reestablecer)
                {
                    TempData["Id"] = cliente.Id;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(cliente.Correo, false);
                    Session["Cliente"] = cliente;
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }
            }
        }

        [HttpPost]
        public ActionResult ReestablecerClave(string correo)
        {
            Cliente cliente = new Cliente();
            cliente = new CNCliente().ListarClientes().Where(cli => cli.Correo == correo).FirstOrDefault();
            if(cliente == null)
            {
                ViewBag.Error = "No se encontró ningún cliente con el correo indicado.";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CNCliente().ReestablecerClave(cliente.Id, correo, out mensaje);
            if (respuesta)
            {
                TempData["Respuesta"] = "Se acaba de enviar su nueva contraseña de acceso al correo registrado. Puede iniciar sesión nuevamente con esta.";
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idCliente, string claveActual, string nuevaClave, string confirmNuevClave)
        {
            Cliente cliente = new Cliente();
            cliente = new CNCliente().ListarClientes().Where(cli => cli.Id == Guid.Parse(idCliente)).FirstOrDefault();
            if(cliente.Clave != CNRecursos.EncriptarSha256(claveActual))
            {
                TempData["IdCliente"] = idCliente;
                TempData["NombreCliente"] = cliente.Nombres + " " + cliente.Apellidos;
                TempData["ClaveActual"] = "";
                ViewBag.Error = "La contraseña actual no es correcta.";
                return View();
            }
            else if (nuevaClave != confirmNuevClave)
            {
                TempData["IdCliente"] = idCliente;
                TempData["NombreCliente"] = cliente.Nombres + " " + cliente.Apellidos;
                TempData["ClaveActual"] = claveActual;
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }
            ViewData["ClaveActual"] = "";

            nuevaClave = CNRecursos.EncriptarSha256(nuevaClave);
            string mensaje = string.Empty;

            bool respuesta = new CNCliente().CambiarClave(Guid.Parse(idCliente), nuevaClave, out mensaje);
            if (respuesta)
            {
                TempData["Respuesta"] = "Contraseña actualizada. Por favor, inicie sesión nuevamente.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCliente"] = idCliente;
                TempData["NombreCliente"] = cliente.Nombres + " " + cliente.Apellidos;
                ViewBag.Error = mensaje;
                ViewBag.Respuesta = null;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return Json(new { success = true, redirectUrl = Url.Action("Index") }, JsonRequestBehavior.AllowGet);
        }
    }
}