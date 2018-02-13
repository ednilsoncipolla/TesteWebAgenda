using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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