using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GenProject.Util.Custom;

namespace GenProject
{
    public partial class Graph : Form
    {
        GenAlgorythm a;
        public Graph(GenAlgorythm _a)
        {
            
            InitializeComponent();
            chart1.Update();
            a = _a;
            label1.Text += "(1-" + a.KolFunc+")";
            n1.Maximum = a.KolFunc;
            n2.Maximum = a.KolFunc;


        }

        private void bDraw_Click(object sender, EventArgs e)
        {
            int f1 = (int)n1.Value;
            int f2 = (int)n2.Value;

            chart1.Series[0].Points.Clear();            
            chart1.Update();


            //chart1.Series[0]. = "Функция " + tbF1.Text;
            for (int i = 0; i < ParetoManager.kolvo; i++)
            {
                chart1.Series[0].Points.AddXY(ParetoManager.Param[i, f1 - 1], ParetoManager.Param[i, f2 - 1]);
            }
        }
    }
}
