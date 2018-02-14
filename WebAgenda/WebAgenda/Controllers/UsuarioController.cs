using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAgenda.Models;

namespace WebAgenda.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            Usuario usu = new Usuario();
            return View(usu.GetListaUsuarios(""));
        }

        // GET: Usuario/Details/5
        public ActionResult Detalhes(string id)
        {
            int iId = 0;
            try
            {
                iId = Convert.ToInt32(id);
            }
            catch 
            {
            }

            if (iId == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Usuario usu = new Usuario(iId);
                return View(usu);
            }
        }

        public static List<SelectListItem> GetLstComboMedicos(int pMed_Id = 0)
        {
            Medico med = new Medico();
            return med.GetListaMedicosCombo(pMed_Id);
        }

        // GET: Usuario/Create
        public ActionResult Incluir()
        {
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        public ActionResult Incluir(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
