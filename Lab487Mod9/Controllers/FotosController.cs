using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab487Mod9.Utils;

namespace Lab487Mod9.Controllers
{
    public class FotosController : Controller
    {
        // GET: Fotos
        public ActionResult Index()
        {
            var almacen = ConfigurationManager.AppSettings["container"];
            var cuenta = ConfigurationManager.AppSettings["cuenta"];
            var key = ConfigurationManager.AppSettings["clave"];

            var st = new Storage(cuenta, key);
            var l = st.ListaContenedor(almacen);
            return View(l);
        }

        public ActionResult Nueva()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nueva(HttpPostedFileBase fichero)
        {
            var cuenta = ConfigurationManager.AppSettings["cuenta"];
            var key = ConfigurationManager.AppSettings["clave"];
            var st = new Storage(cuenta, key);
            var almacen = ConfigurationManager.AppSettings["container"];

            if (fichero != null && fichero.ContentLength > 0)
            {
                string nombre = DateTime.Now.Ticks.ToString();
                st.SubirFoto(fichero.InputStream, nombre, almacen);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Borrar(String id)
        {
            var cuenta = ConfigurationManager.AppSettings["cuenta"];
            var key = ConfigurationManager.AppSettings["clave"];
            var st = new Storage(cuenta, key);
            var almacen = ConfigurationManager.AppSettings["container"];
            st.BorrarFoto(id, almacen);
            return RedirectToAction("Index");
        }


    }
}