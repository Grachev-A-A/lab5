using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab5
{
    public partial class Form1 : Form
    {
        List<Pass> passes = new List<Pass>();
        bool alwaysFindWithFIO = false;

        public Form1()
        {
            InitializeComponent();
            MessageBox.Show(checkBox2.Size.ToString() + checkBox2.Location.X + checkBox2.Location.Y);
            //var i = new ListViewItem();
            //i.Text = "Фамилия Имя Отчество";
            //var a = new ListViewItem.ListViewSubItem();
            //a.Text = "Ntcn";
            //i.SubItems.Add(a);

            //listView1.Items.Add(i);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = !checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var p = new Pass();
            p.level = (int)numericUpDown1.Value;
            p.name = textBox2.Text;
            p.lastName = textBox1.Text;
            p.dadsName = textBox3.Text;
            p.issued = dateTimePicker1.Value;
            p.validTo = dateTimePicker2.Value;
            p.tested = dateTimePicker3.Value;
            if (checkBox1.Checked)
            {
                p.num = passes.Count;
                passes.Add(p);
            }
            else
            {
                p.num = (int)numericUpDown2.Value;
                passes.Insert(p.num, p);
                Pass[] pas = passes.ToArray()
;                for(int i = p.num; i<passes.Count; i++)
                {
                    pas[i].num = i;
                }
                passes = pas.ToList();
                
            }
            drawList(passes);
        }

        void drawList(List<Pass> li)
        {
            listView1.Items.Clear();
            foreach(var item in li)
            {
                ListViewItem lim = new ListViewItem();
                lim.Text = item.num.ToString();
                var slim1 = new ListViewItem.ListViewSubItem();
                slim1.Text = item.lastName;
                lim.SubItems.Add(slim1);
                var slim2 = new ListViewItem.ListViewSubItem(lim, item.name);
                var slim3 = new ListViewItem.ListViewSubItem(lim, item.dadsName);
                var slim4 = new ListViewItem.ListViewSubItem(lim, item.issued.ToString());
                var slim5 = new ListViewItem.ListViewSubItem(lim, item.validTo.ToString());
                var slim6 = new ListViewItem.ListViewSubItem(lim, item.tested.ToString());
                var slim7 = new ListViewItem.ListViewSubItem(lim, item.level.ToString());
                lim.SubItems.AddRange(new ListViewItem.ListViewSubItem[] { slim2, slim3, slim4, slim5, slim6, slim7 });
                listView1.Items.Add(lim);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            alwaysFindWithFIO = checkBox2.Checked;
            if (alwaysFindWithFIO)
            {
                button6.Text = "Найти всё";
            }
            else button6.Text = "Поиск по ФИО";
        }


        private void button4_Click(object sender, EventArgs e)
        {
            var a = new List<Pass>(passes);
            if (alwaysFindWithFIO)
            {
                a = FindByFIO(a, textBox4.Text, textBox5.Text, textBox6.Text);
            }
            for (int i = 0; i < a.Count; i++) 
            {
                if (a[i].timeAfterLastTest() < new TimeSpan(183, 0, 0, 0))
                {
                    a.RemoveAt(i--);
                }
            }
            drawList(a);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            drawList(FindByFIO(passes, textBox4.Text, textBox5.Text, textBox6.Text));
        }
        List<Pass> FindByFIO(List<Pass> s, string LastName, string Name, string MiddleName)
        {
            
            var a = new Pass[s.Capacity];
            s.CopyTo(a);
            var res = a.ToList();
            if (LastName != "")
            {
                for(int i = 0; i<res.Count; i++)
                {
                    if(res[i].lastName != LastName)
                    {
                        res.RemoveAt(i--);
                        
                    }
                }
            }if(Name != "")
            {
                for (int i = 0; i < res.Count; i++)
                {
                    if (res[i].name != Name)
                    {
                        res.RemoveAt(i--);
                    }
                }
            }if(MiddleName != "")
            {
                for (int i = 0; i < res.Count; i++)
                {
                    if (res[i].dadsName != MiddleName)
                    {
                        res.RemoveAt(i--);
                    }
                }
            }
            return res;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var a = new List<Pass>(passes);
            if (alwaysFindWithFIO)
                a = FindByFIO(a, textBox4.Text, textBox5.Text, textBox6.Text);
            for (int i = 0; i < a.Count; i++)
                if (!a[i].needReIssue())
                    a.RemoveAt(i);
            drawList(a);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var a = new List<Pass>(passes);
            if (alwaysFindWithFIO)
                a = FindByFIO(a, textBox4.Text, textBox5.Text, textBox6.Text);
            for (int i = 0; i < a.Count; i++)
                if (a[i].isValid())
                    a.RemoveAt(i);
            drawList(a);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            drawList(passes);
        }
    }

    public struct Pass
    {
        public int num, level;
        public string name, lastName, dadsName;
        public DateTime validTo, tested, issued;

        public bool isValid()
        {
            return validTo > DateTime.Now;
        }
        public TimeSpan timeAfterLastTest()
        {
            return DateTime.Now.Subtract(tested);
        }
        public bool needReIssue()
        {
            return timeAfterLastTest() > new TimeSpan(183, 0, 0, 0) &&
                DateTime.Now.Subtract(validTo) > new TimeSpan(-63, 0, 0, 0);
        }
    }
}
