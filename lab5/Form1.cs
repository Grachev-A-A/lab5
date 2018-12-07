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
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = !checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var p = new Pass();
            p.level = (int)numericUpDown1.Value;
            if(textBox1.Text.Trim()=="" || textBox2.Text.Trim()==""||
                textBox3.Text.Trim() == "")
            {
                MessageBox.Show("Введите не пустые фамилию, имя и отчество!");
                return;
            }
            p.name = textBox2.Text;
            p.lastName = textBox1.Text;
            p.dadsName = textBox3.Text;
            if(dateTimePicker1.Value> DateTime.Now)
            {
                MessageBox.Show("Пропуск не может быть выпущен в будущем!");
                return;
            }
            p.issued = dateTimePicker1.Value;
            p.validTo = dateTimePicker2.Value;
            if(dateTimePicker3.Value > DateTime.Now)
            {
                MessageBox.Show("Аттестация должна быть пройдена в прошлом!");
                return;
            }
            p.tested = dateTimePicker3.Value;
            if (checkBox1.Checked)
            {
                if (passes.Count == 0)
                    p.num = 0;
                else
                    p.num = passes.LastIndexOf(passes.Last())+1;
            }
            else
            {
                p.num = (int)numericUpDown2.Value;
                if (p.num > passes.Capacity - 1)
                {
                    Pass[] a = new Pass[p.num];
                    passes.CopyTo(a);
                    passes = a.ToList();
                }else if (passes[p.num].name != null)
                {
                    if (MessageBox.Show("Пропуск с таким номером существует! Перезаписать данные?", "Rewrite", MessageBoxButtons.YesNo) == DialogResult.Yes){
                        passes.RemoveAt(p.num);
                    } else return;
                }
            }
            passes.Insert(p.num, p);
            label13.Text = (int.Parse(label13.Text) + 1).ToString();
            drawList(passes);
        }

        void drawList(List<Pass> li)
        {
            listView1.Items.Clear();
            label15.Text = 0.ToString();
            foreach(var item in li)
            {
                if (item.name != null)
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
                    label15.Text = (int.Parse(label15.Text) + 1).ToString();
                }
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
                {
                    a.RemoveAt(i--);
                }
            drawList(a);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var a = new List<Pass>(passes);
            if (alwaysFindWithFIO)
                a = FindByFIO(a, textBox4.Text, textBox5.Text, textBox6.Text);
            for (int i = 0; i < a.Count; i++)
                if (a[i].isValid())
                    a.RemoveAt(i--);
            drawList(a);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            drawList(passes);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберете хотя бы один элемент!");
                return;
            }
            foreach(ListViewItem item in listView1.SelectedItems)
            {
                label13.Text = (int.Parse(label13.Text) - 1).ToString();
                passes[int.Parse(item.Text)] = new Pass(passes[int.Parse(item.Text)])
                {
                    name = null
                };
            }
            drawList(passes);
        }
    }

    public struct Pass
    {
        public int num, level;
        public string name, lastName, dadsName;
        public DateTime validTo, tested, issued;

        public Pass(Pass pass) : this()
        {
            num = pass.num;
            level = pass.level;
            name = pass.name;
            lastName = pass.lastName;
            dadsName = pass.dadsName;
            validTo = pass.validTo;
            tested = pass.tested;
            issued = pass.issued;
        }

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
            return timeAfterLastTest() < new TimeSpan(183, 0, 0, 0) &&
                DateTime.Now.Subtract(validTo) > new TimeSpan(-63, 0, 0, 0)
                && isValid();
        }
    }
}
