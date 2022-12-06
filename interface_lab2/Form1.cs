using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace interface_lab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            errc.Text = "0";
            rpt.Text = "1";
        }

        FlowEff Eff = new FlowEff();

        private void button1_Click(object sender, EventArgs e)
        {
            double i;
            int j;

            CalcData CData = new CalcData();
            EffData EData = new EffData();

            double Sum;
            CData.Error_chance = 0;

            if (Double.TryParse(errc.Text, out i) == true)
                CData.Error_chance = i;
            if (CData.Error_chance >= 1 || CData.Error_chance < 0)
            {
                MessageBox.Show("Некорректные данные, вероятность ошибки должна быть не меньше 0% и меньше 100%");
                return;
            }

            if (Int32.TryParse(rpt.Text, out j) && j > 0)
                EData.RepeatCount = j;
            else
            {
                MessageBox.Show("Некорректные данные, количество повторений должно быть больше 0");
                return;
            }

            if (Double.TryParse(p1.Text, out i) && i >= 0)
                EData.P1 = i;
            else
            {
                MessageBox.Show("Некорректные данные, потери Р1 должны быть больше оибо равны 0");
                return;
            }

            if (Double.TryParse(p2.Text, out i) && i >= 0)
                EData.P2 = i;
            else
            {
                MessageBox.Show("Некорректные данные, потери Р2 должны быть больше оибо равны 0");
                return;
            }

            if (Double.TryParse(reqavg.Text, out i) && i > 0)
                EData.RequestAvg = i;
            else
            {
                MessageBox.Show("Некорректные данные, среднее время прихода заявок должно быть больше 0");
                return;
            }

            if (Double.TryParse(shutavg.Text, out i) && i > 0)
                EData.ShutdownAvg = i;
            else
            {
                MessageBox.Show("Некорректные данные, среднее время отказа должно быть больше 0");
                return;
            }

            GroupBox gb_j;
            GroupBox gb_wc;
            DataGridView dgv;
            
            for (int ii = 1; ii <= 2; ii++)
            {
                if (ii == 1)
                {
                    gb_j = groupBox1;
                    gb_wc = groupBox2;
                    dgv = dataGridView2;
                }
                else
                {
                    gb_j = groupBox4;
                    gb_wc = groupBox5;
                    dgv = dataGridView1;
                }

                CData.Joints = new List<string>();
                CData.Ways = new List<List<int>>();
                CData.Way_chances = new List<double>();
                Sum = 0;

                foreach (var txt in gb_j.Controls)
                {
                    if (txt is Label) continue;
                    if ((txt as TextBox).Text != "")
                        CData.Joints.Add((txt as TextBox).Text);
                }

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    List<int> cells_list = new List<int>();
                    foreach (DataGridViewCell cell in row.Cells)
                        if (Int32.TryParse((string)cell.Value, out j) == true)
                            cells_list.Add(j);
                    CData.Ways.Add(cells_list);
                }

                foreach (var txt in gb_wc.Controls)
                {
                    if (txt is Label) continue;
                    if (Double.TryParse((txt as TextBox).Text, out i) == true)
                        CData.Way_chances.Add(i);
                }

                foreach (double ch in CData.Way_chances)
                    Sum += ch;
                if (Sum != 1)
                {
                    MessageBox.Show($"Подтема {ii}: Некорректные данные, вероятности выбора маршрутов не дают 100% в сумме");
                    return;
                }

                if (ii == 1)
                    result1.Text = Eff.Calc_eff(CData, EData).ToString();
                else
                    result2.Text = Eff.Calc_eff(CData, EData).ToString();
            }

        }
    }
}