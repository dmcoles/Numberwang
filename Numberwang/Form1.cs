using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private Random r = new Random();
        private string wangdifficulty;
        private List<int> correctAnswer = new List<int>();
        private List<int> keeps = new List<int>();

        public Form1()
        {
            InitializeComponent();
        }

        private bool canDivide(string val1, string val2)
        {
            return (int.Parse(val1) % int.Parse(val2)) == 0;
        }

        private bool GridComplete()
        {
            List<Label> numControls = getNumControls();
            for (int i = 0; i < numControls.Count; i++)
            {
                if (numControls[i].Text == "") return false;
            }
            return true;
        }

        private bool CheckGrid()
        {
            List<Label> numControls = getNumControls();
            for (int i=0; i<numControls.Count; i++)
            {
                if (numControls[i].Text!=correctAnswer[i].ToString()) return false;
            }
            return true;
        }

        private void DisplayNumbers()
        {
            List<Label> numControls = getNumControls();

            for (int i = 0; i<numControls.Count; i++)
            {
                numControls[i].Text = correctAnswer[i].ToString();
            }

        }

        private void RemoveItems()
        {
            List<Label> numControls = getNumControls();

            for (int i = 0; i < numControls.Count; i++)
            {
                if (!keeps.Contains(i))
                {
                    numControls[i].Text = "";
                }
            }

        }

        private List<Label> getNumControls()
        {
            List<Label> numControls = new List<Label>();
            numControls.Add(label1);
            numControls.Add(label3);
            numControls.Add(label5);
            numControls.Add(label13);
            numControls.Add(label15);
            numControls.Add(label17);
            numControls.Add(label25);
            numControls.Add(label27);
            numControls.Add(label29);
            return numControls;
        }

        private List<ToolStripItem> getButtons()
        {
            List<ToolStripItem> buttons = new List<ToolStripItem>();
            buttons.Add(toolStripButton1); buttons.Add(toolStripButton2); buttons.Add(toolStripButton3);
            buttons.Add(toolStripButton4); buttons.Add(toolStripButton5); buttons.Add(toolStripButton6);
            buttons.Add(toolStripButton7); buttons.Add(toolStripButton8); buttons.Add(toolStripButton9);
            buttons.Add(toolStripButton10);
            return buttons;
        }

        private void DisableButtons()
        {
            List<Label> numControls = getNumControls();
            List<ToolStripItem> buttons = getButtons();

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Enabled = true;
                ((ToolStripButton)buttons[i]).Checked = false;
            }

            foreach (Label l in numControls)
            {
                if (l.Text!="")
                {
                    buttons[int.Parse(l.Text) - 1].Enabled = false;
                }
            }
        }

        private void SelectFirstEnabledButton()
        {
            List<ToolStripItem> buttons = getButtons();
            for (int i = 0; i < buttons.Count; i++)
            {
                ((ToolStripButton)buttons[i]).Checked = false;
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].Enabled)
                {
                    ((ToolStripButton)buttons[i]).Checked = true;
                    break;
                }
            }

        }
        private void InsertSelectedNumber(Label l)
        {
            List<ToolStripItem> buttons = getButtons();
            
            for (int i = 0; i < buttons.Count; i++)
            {
                if (((ToolStripButton)buttons[i]).Checked)
                {
                    if (i == (buttons.Count-1))
                    {
                        l.Text = "";
                    }
                    else
                    {
                        l.Text = (i + 1).ToString();
                    }
                    
                }
            }

        }

        private int FindSelected()
        {
            List<ToolStripItem> buttons = getButtons();

            for (int i = 0; i<buttons.Count; i++)
            {
                if (((ToolStripButton)buttons[i]).Checked) return i;
            }
            return -1;
        }

        private string GetValidOperators(string v1, string v2)
        {
            if (canDivide(v1,v2))
            {
                return "+-*/";
            }
            else
            {
                return "+-*";
            }

        }

        private int DoCalculation(char op, string v1, string v2)
        {
            switch (op)
            {
                case '+':
                    return int.Parse(v1) + int.Parse(v2);
                case '-':
                    return int.Parse(v1) - int.Parse(v2);
                case '*':
                    return int.Parse(v1) * int.Parse(v2);
                case '/':
                    return int.Parse(v1) / int.Parse(v2);
            }

            throw new ArgumentException("invalid operator");

        }

        private void CalcLine(List<Label>controls)
        {
            //calculate operators for controls 1 and 3 when the numbers are in controls 0,2 and 4 and then put the result in control 5

            string validOperators = GetValidOperators(controls[0].Text, controls[2].Text);

            char op = validOperators[r.Next(validOperators.Length)];

            controls[1].Text = op.ToString();

            string result = DoCalculation(op,controls[0].Text,controls[2].Text).ToString();

            validOperators = GetValidOperators(result, controls[4].Text);

            char op2 = validOperators[r.Next(validOperators.Length)];

            controls[3].Text = op2.ToString();

            controls[5].Text = DoCalculation(op2, result, controls[4].Text).ToString();
        }

        private void Generate()
        {
            correctAnswer.Clear();

            List<Label> numControls = getNumControls();

            string nums = "123456789";
            for (int i = 0; i < 9; i++)
            {
                int n = r.Next(nums.Length);
                correctAnswer.Add(int.Parse(nums[n].ToString()));
                nums = nums.Remove(n, 1);
            }
            DisplayNumbers();

            //calculate the first row
            numControls.Clear();
            numControls.Add(label1); numControls.Add(label2); numControls.Add(label3);
            numControls.Add(label4); numControls.Add(label5); numControls.Add(label6);
            CalcLine(numControls);

            //calculate the second row
            numControls.Clear();
            numControls.Add(label13); numControls.Add(label14); numControls.Add(label15);
            numControls.Add(label16); numControls.Add(label17); numControls.Add(label18);
            CalcLine(numControls);

            //calculate the third row
            numControls.Clear();
            numControls.Add(label25); numControls.Add(label26); numControls.Add(label27);
            numControls.Add(label28); numControls.Add(label29); numControls.Add(label30);
            CalcLine(numControls);

            //calculate the first column
            numControls.Clear();
            numControls.Add(label1); numControls.Add(label7); numControls.Add(label13);
            numControls.Add(label19); numControls.Add(label25); numControls.Add(label31);
            CalcLine(numControls);

            //calculate the second column
            numControls.Clear();
            numControls.Add(label3); numControls.Add(label9); numControls.Add(label15);
            numControls.Add(label21); numControls.Add(label27); numControls.Add(label33);
            CalcLine(numControls);

            //calculate the third column
            numControls.Clear();
            numControls.Add(label5); numControls.Add(label11); numControls.Add(label17);
            numControls.Add(label23); numControls.Add(label29); numControls.Add(label35);
            CalcLine(numControls);

            numControls = getNumControls();

            int keep = 0;
            wangdifficulty = "Wang";

            if (comboBox1.SelectedIndex == 0)
            {
                keep = 2;
                wangdifficulty = "Easy";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                keep = 1;
                wangdifficulty = "Hard";
            }

            keeps.Clear();

            for (int i = 0; i < keep; i++)
            {
                bool ok = false;
                int n = 0;
                while (!ok)
                {
                    ok = true;
                    n = r.Next(numControls.Count);
                    for (int j = 0; j < keeps.Count; j++)
                    {
                        if (((n / 3) == (keeps[j] / 3)) || ((n % 3) == (keeps[j] % 3))) ok = false;
                    }
                }
                keeps.Add(n);

            }

            RemoveItems();
            DisableButtons();
            SelectFirstEnabledButton();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Generate();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            Generate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*Form f = new Form();
            f.Width = 600;
            f.Height = 800;
            Panel p = new Panel();
            p.Parent = f;
            p.Width = 600;
            p.Height = 800;
            p.Paint += PanelPaint;
            f.Show();*/
            

            printDocument1.PrintController = new StandardPrintController();
            printDocument1.Print();

/*            PrintDialog myPrintDialog = new PrintDialog();
            myPrintDialog.Document = printDocument1;
            if (myPrintDialog.ShowDialog() == DialogResult.OK)
            {
                System.Drawing.Printing.PrinterSettings values;
                values = myPrintDialog.PrinterSettings;
            }*/
            printDocument1.Dispose();
        
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap b = new Bitmap(panel2.Width, panel2.Height);
            panel2.DrawToBitmap(b, panel2.ClientRectangle);
            e.Graphics.DrawString("Numberwang: "+wangdifficulty, new Font("Arial", 24), Brushes.Black, e.PageBounds.Width / 4, e.PageBounds.Height / 4 - e.PageBounds.Height / 8);
            e.Graphics.DrawImage(b, e.PageBounds.Width / 4, e.PageBounds.Height / 4, e.PageBounds.Width / 2, e.PageBounds.Height / 2);
            for (int i=1; i<=9;i++)
            {
                e.Graphics.DrawString(i.ToString(), new Font("Arial", 24), Brushes.Black, e.PageBounds.Width / 4 - e.PageBounds.Width / 8, (e.PageBounds.Height / 10)* i);
            }
        }

        private void PanelPaint(object sender, PaintEventArgs e)
        {
            Bitmap b = new Bitmap(panel2.Width, panel2.Height);
            panel2.DrawToBitmap(b, panel2.ClientRectangle);
            e.Graphics.DrawString("Numberwang: " + wangdifficulty, new Font("Arial", 24), Brushes.Black, e.ClipRectangle.Width / 4, e.ClipRectangle.Height / 4 - e.ClipRectangle.Height / 8);
            e.Graphics.DrawImage(b, e.ClipRectangle.Width / 4, e.ClipRectangle.Height / 4, e.ClipRectangle.Width / 2, e.ClipRectangle.Height / 2);
            for (int i = 1; i <= 9; i++)
            {
                e.Graphics.DrawString(i.ToString(), new Font("Arial", 24), Brushes.Black, e.ClipRectangle.Width / 4 - e.ClipRectangle.Width / 8, (e.ClipRectangle.Height / 10) * i);
            }
        }

        private void toolStripButton3_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ToolStripItem c in toolStrip1.Items)
            {
                ((ToolStripButton)c).Checked = c==sender;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            InsertSelectedNumber((Label)sender);
            DisableButtons();
            SelectFirstEnabledButton();

            if (GridComplete())
            {
                if (CheckGrid())
                {
                    MessageBox.Show("That's numberwang");
                }
                else               
                {
                    MessageBox.Show("Sorry, Not numberwang");
                }

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DisplayNumbers();
            RemoveItems();
            DisableButtons();
            SelectFirstEnabledButton();
        }

    }
}
