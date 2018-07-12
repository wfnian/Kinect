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

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string strInput = "F:\\kinect\\application.py 409.947969373939,614.289002160591,1069.99326556048,729.388634304389,284.803383755373,392.435675163498,534.111825220838,1073.69154107929,1487.13061041836,233.262367480703,609.100048386539,451.452729308413";
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(strInput + "&exit");
            p.StandardInput.AutoFlush = true;
            string strOuput = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();

            Console.WriteLine(strOuput.Split('\n')[strOuput.Split('\n').Length-2]);


            Console.ReadKey();
        }
    }
}
