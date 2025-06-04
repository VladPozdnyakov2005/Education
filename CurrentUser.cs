using Education.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Education
{
    internal class CurrentUser
    {
        private static CurrentUser instance;

        private CurrentUser()
        { }

        public static CurrentUser getInstance()
        {
            if (instance == null)
                instance = new CurrentUser();
            return instance;
        }

        public static Users NowUser { get; set; }

        public static void SetUser(Users user)
        {
            NowUser = user;
        }

        public static void ToNull()
        {
            NowUser = null;
        }
        public static bool IsLogin => NowUser != null;

        public static bool IsAdmin()
        {
            if (NowUser != null)
            {
                return NowUser.access == "ADM";
            }
            return false;
        }

    }
}
