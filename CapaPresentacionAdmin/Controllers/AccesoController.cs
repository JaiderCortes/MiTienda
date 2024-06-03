using DocumentFormat.OpenXml.Office2016.Presentation.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using DocumentFormat.OpenXml.Bibliography;
using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult ReestablecerClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario usuario = new Usuario();
            string claveDecrypt = CNRecursos.EncriptarSha256(clave);
            usuario = new CNUsuarios().ListarUsuarios().Where(u => u.Correo == correo && u.Clave == claveDecrypt).FirstOrDefault();

            if (usuario == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }
            else if (usuario.Activo == false)
            {
                ViewBag.Error = "Usuario inactivo en el sistema.";
                return View();
            }
            else
            {
                if (usuario.Reestablecer == true)
                {
                    TempData["IdUsuario"] = usuario.Id;
                    TempData["NombreUsuario"] = usuario.Nombres + " " + usuario.Apellidos;
                    return RedirectToAction("CambiarClave");
                }

                FormsAuthentication.SetAuthCookie(usuario.Correo, false);
                ViewBag.Error = null;
                Session["Usuario"] = usuario.Nombres + " " + usuario.Apellidos;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idUsuario, string claveActual, string nuevaClave, string confirmNuevClav)
        {
            Usuario usuario = new Usuario();
            usuario = new CNUsuarios().ListarUsuarios().Where(u => u.Id == Guid.Parse(idUsuario)).FirstOrDefault();

            if (usuario.Clave != CNRecursos.EncriptarSha256(claveActual))
            {
                TempData["IdUsuario"] = idUsuario;
                TempData["NombreUsuario"] = usuario.Nombres + " " + usuario.Apellidos;
                ViewData["ClaveActual"] = "";
                ViewBag.Error = "La contraseña actual no es correcta.";
                return View();
            }
            else if (nuevaClave != confirmNuevClav)
            {
                TempData["IdUsuario"] = idUsuario;
                TempData["NombreUsuario"] = usuario.Nombres + " " + usuario.Apellidos;
                ViewData["ClaveActual"] = claveActual;
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }
            ViewData["ClaveActual"] = "";

            nuevaClave = CNRecursos.EncriptarSha256(nuevaClave);
            string mensaje = string.Empty;

            bool respuesta = new CNUsuarios().CambiarClave(Guid.Parse(idUsuario), nuevaClave, out mensaje);

            if (respuesta)
            {
                TempData["Respuesta"] = "Contraseña actualizada. Por favor, inicie sesión nuevamente.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdUsuario"] = idUsuario;
                TempData["NombreUsuario"] = usuario.Nombres + " " + usuario.Apellidos;
                ViewBag.Error = mensaje;
                ViewBag.Respuesta = null;
                return View();
            }
        }

        [HttpPost]
        public ActionResult ReestablecerClave(string correo)
        {
            Usuario usuario = new Usuario();
            usuario = new CNUsuarios().ListarUsuarios().Where(user => user.Correo == correo).FirstOrDefault();

            if (usuario == null)
            {
                ViewBag.Error = "No se encontró ningún usuario con el correo indicado.";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CNUsuarios().ReestablecerClave(usuario.Id, correo, out mensaje);
            if (respuesta)
            {
                TempData["Respuesta"] = "Se acaba de enviar su nueva contraseña de acceso al correo registrado. Puede iniciar sesión nuevamente con esta.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return Json(new { success = true, redirectUrl = Url.Action("Index") }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index");
        }
    }
}