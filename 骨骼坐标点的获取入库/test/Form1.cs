using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using CCWin;

namespace test
{
    public partial class Form1 : CCSkinMain
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            skinLabel1.Text = "fdsadsa哈哈哈哈，姿势姿势";
            Console.WriteLine(DateTime.Now.Second.ToString());
            System.Console.WriteLine(DateTime.Now.Second);
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(DateTime.Now.Second);
        }
    }
}
