using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAgenda.Models;

namespace WebAgenda.Controllers
{
    public class MedicoController : Controller
    {
        // GET: Medico
        public ActionResult Index()
        {
            Medico med = new Medico(); 
            return View(med.GetListaMedicos(""));
        }

        // GET: Medico/Detalhes/5
        public ActionResult Detalhes(int id)
        {
            return View();
        }

        // POST: Medico/Detalhes/5
        [HttpPost]
        public ActionResult Detalhes(int id, FormCollection collection)
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

        // GET: Medico/Create
        public ActionResult Create()
        {
            Medico med = new Medico();
            return View(med);
        }

        // POST: Medico/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                Medico med = new Medico
                {
                    Med_Nome = collection.GetValue("Med_Nome").AttemptedValue.ToString(),
                    Med_Email = collection.GetValue("Med_Email").AttemptedValue.ToString(),
                    Med_Telefones = collection.GetValue("Med_Telefones").AttemptedValue.ToString(),
                    Med_Obs = collection.GetValue("Med_Obs").AttemptedValue.ToString()
                };

                try
                {
                    med.Gravar();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: Medico/Delete/5
        public ActionResult Delete(string id)
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
                try
                {
                    Medico med = new Medico(iId);
                    med.Excluir();
                    return View(med);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        // POST: Medico/Delete/5
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
