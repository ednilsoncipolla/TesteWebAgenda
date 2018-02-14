using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAgenda.Models
{
    public static class Sistema
    {
        public static Usuario Usuario = new Usuario();

        public static bool StAutenticado()
        {
            return Usuario.Usu_Id > 0;
        }
    }
}