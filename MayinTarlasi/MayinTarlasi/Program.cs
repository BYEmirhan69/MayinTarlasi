using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MayinTarlasi
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form2 form2 = new Form2();
            if (form2.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new Form1(form2.MineCount));
            }
        }
    }
    
}
