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
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

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
            ScriptEngine pyEngine = Python.CreateEngine();//创建Python解释器对象
            dynamic py = pyEngine.ExecuteFile(@"F:\\kinect\\骨骼坐标点的获取入库\\骨骼坐标点的获取入库\\bin\\Debug\\test.py");//读取脚本文件
            string dd = py.main("调用");//调用脚本文件中对应的函数
            skinLabel1.Text += dd ;
        }
    }
}
