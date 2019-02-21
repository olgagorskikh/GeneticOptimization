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


    public partial class MainForm : Form
    {
        //поля класса MainForm
        #region GenAlgorythm fields

            private int KolFunc;
            private int KolKoord;
            private int KolOgr;
            private uslov[] uslovia;
            private functional[] funct;
            private int KolBit;
            private int KolOsob;
            private int KolPokol;
            public double Tochn;
            private int MaxValue;
            public GenAlgorythm a;
            public int kolOsobParet;//количество особей в Парето-фронте

        #endregion

        public MainForm()
        {
            InitializeComponent();
        }        

        private void bReady_Click(object sender, EventArgs e)
        {
            KolFunc = int.Parse(tbKolFunc.Text);
            KolKoord = int.Parse(tbKolKoord.Text);
            KolOgr = int.Parse(tbKolOgr.Text);

            uslovia = new uslov[KolOgr];
            funct = new functional[KolFunc];


            //заполняем критерии
            Control[] panels = new Control[KolFunc];

            for (int j = 0; j <= KolFunc - 1; j++)
            {
                panels = this.Controls.Find("panel" + (j + 3).ToString(), true);
                panels[0].Visible = true;//видимость панелей
            }

            //заполняем ограничения
            Control[] panels1 = new Control[KolOgr];

            for (int j = 0; j <= KolOgr - 1; j++)
            {
                panels1 = this.Controls.Find("panel" + ((j + 1)*10).ToString(), true);
                panels1[0].Visible = true;//видимость панелей
            }
        }              

        private void InitConditions()
        {


            int at, b, c;

            for (int i = 0; i < KolOgr; i++)
            {
                at = (i + 1) * 3;  // номер первого TextBox
                b = (i + 1) * 3 - 1; // номер второго TextBox
                c = (i + 1) * 3 - 2; // номер третьего TextBox
                at += 5;
                b += 5;
                c += 5;
                uslovia[i].left = this.Controls.Find("textBox" + at.ToString(), true)[0].Text; // левая часть
                uslovia[i].znak = this.Controls.Find("textBox" + b.ToString(), true)[0].Text; // знак
                if (uslovia[i].znak == "=") uslovia[i].znak += '=';
                uslovia[i].right = this.Controls.Find("textBox" + c.ToString(), true)[0].Text; // правая часть

            }
            KolBit = int.Parse(tbKol.Text);
            KolOsob = int.Parse(tbKolOsob.Text);
            KolPokol = int.Parse(tbKolPokol.Text);
            Tochn = double.Parse(tbTochn.Text);
            MaxValue = int.Parse(tbMax.Text);
            RadioButton radiobut = new RadioButton();

            for (int i = 0; i < KolFunc; i++)
            {
                at = 2 * (i + 1) - 1;
                b = i + 1;
                radiobut = (RadioButton)this.Controls.Find("radioButton" + at.ToString(), true)[0];

                funct[i].fun = this.Controls.Find("textBox" + b.ToString(), true)[0].Text; // левая часть

                if (radiobut.Checked == true)
                    funct[i].mima = 0;
                else funct[i].mima = 1;
            }

        }

        private void bCount_Click(object sender, EventArgs e)
        {

            InitConditions();
            DateTime start1 = DateTime.Now;
            //Запускаем вычисления
            a = new GenAlgorythm(KolOsob, KolKoord, Tochn, KolBit, uslovia, KolOgr, funct, KolPokol, MaxValue, (MainForm)this.FindForm());
            DateTime end1 = DateTime.Now;
            lTime.Text = "Время: "+(int)((end1 - start1).TotalMilliseconds/1000)+" сек";
            //Открываем форму результатов программы
          
            //kolOsobParet = a.kolvo;
            kolOsobParet = ParetoManager.kolvo;
            
            lbFun.Items.Clear();        
            lbPerem.Items.Clear();
            String stroka = "";

            //Заполняем Listbox с функциями
            for (int i = 0; i < kolOsobParet; i++)
            {
                stroka = "";
                for (int j = 0; j < KolFunc; j++)
                    stroka += (ParetoManager.Param[i, j] + "  ");
                lbFun.Items.Add(stroka);
            }

        }        

        private void lbFun_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbPerem.Items.Clear();
            int j = lbFun.SelectedIndex;
            for (int i = 0; i < KolKoord; i++)
                lbPerem.Items.Add(ParetoManager.Koordinates[j, i].ToString());
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bGraph_Click(object sender, EventArgs e)
        {
            
            Graph d = new Graph(a);
            d.ShowDialog();
        }
               
    }
}
