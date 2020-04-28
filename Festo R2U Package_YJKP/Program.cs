using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Festo_R2U_Package_YJKP
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form_Init mForm_Init = new Form_Init();
            if (mForm_Init.WebLink=="")
            {
                Application.Run(mForm_Init);
            }
            else
            {
                switch (mForm_Init.DefaultWindowIndex)
                {
                    case 0:
                        Application.Run(new Form_Web(mForm_Init.WebLink));
                        break;
                    case 1:
                        Application.Run(new Form_Customized());
                        break;
                    default:
                        Application.Run(new Form_Web(mForm_Init.WebLink));
                        break;
                }
            }            
        }
    }
}
