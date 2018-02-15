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
            { }

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

        [HttpPost]
        public ActionResult Detalhes(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                string[] Str = collection.AllKeys;
                string str = collection.GetValue("Usu_Id").AttemptedValue.ToString();
                Usuario usu = new Usuario();
                usu.Usu_Id = Convert.ToInt32(collection.GetValue("Usu_Id").AttemptedValue);
                usu.Med_Id = Convert.ToInt32(collection.GetValue("Médico").AttemptedValue);
                usu.Usu_Nome = collection.GetValue("Usu_Nome").AttemptedValue.ToString();
                usu.Usu_Login = collection.GetValue("Usu_Login").AttemptedValue.ToString();
                usu.Usu_Senha = collection.GetValue("Usu_Senha").AttemptedValue.ToString();
                usu.Gravar();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public static List<SelectListItem> GetLstComboMedicos(int pMed_Id = 0)
        {
            Medico med = new Medico();
            return med.GetListaMedicosCombo(pMed_Id);
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
