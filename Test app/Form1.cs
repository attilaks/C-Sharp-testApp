using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Test_app
{
    public partial class Form1 : Form
    {
        public string name;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 1;
            comboBox4.SelectedIndex = 1;
        }

        private void openFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                name = openFileDialog1.FileName;
                textBoxMain.Clear();
                textBoxMain.Text = File.ReadAllText(name, Encoding.Default);
            }
        }

        private void saveFile_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = name;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                name = saveFileDialog1.FileName;
                File.WriteAllText(name, textBoxMain.Text);
            }
        }

        private void ApplyFilter_Click(object sender, EventArgs e)
        {
            string[] lines = textBoxMain.Text.Split('\n');
            int len = lines.GetLength(0);
            List<string> ors = new List<string>();
            List<string> ands = new List<string>();
            int sublineIdx;

            textBox1.Text = textBox1.Text.Trim(' ');
            textBox2.Text = textBox2.Text.Trim(' ');
            textBox3.Text = textBox3.Text.Trim(' ');
            textBox4.Text = textBox4.Text.Trim(' ');
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                if (comboBox1.SelectedIndex == 0)
                    ands.Add(textBox1.Text);
                else
                    ors.Add(textBox1.Text);
            }
            if (!String.IsNullOrEmpty(textBox2.Text))
            {
                if (comboBox2.SelectedIndex == 0)
                    ands.Add(textBox2.Text);
                else
                    ors.Add(textBox2.Text);
            }
            if (!String.IsNullOrEmpty(textBox3.Text))
            {
                if (comboBox3.SelectedIndex == 0)
                    ands.Add(textBox3.Text);
                else
                    ors.Add(textBox3.Text);
            }
            if (!String.IsNullOrEmpty(textBox4.Text))
            {
                if (comboBox4.SelectedIndex == 0)
                    ands.Add(textBox4.Text);
                else
                    ors.Add(textBox4.Text);
            }
            textBoxMain.Clear();
            for (int i = 0; i < len; ++i)
            {
                sublineIdx = -1;
                foreach (string ptr in ors)
                {
                    sublineIdx = lines[i].IndexOf(ptr, StringComparison.CurrentCultureIgnoreCase);
                    if (sublineIdx != -1)
                    {
                        textBoxMain.Text = String.Concat(textBoxMain.Text, lines[i], "\n");
                        break ;
                    }
                }
                if (sublineIdx == -1)
                {
                    foreach (string ptr in ands)
                    {
                        sublineIdx = lines[i].IndexOf(ptr, StringComparison.CurrentCultureIgnoreCase);
                        if (sublineIdx == -1)
                            break ;
                    }
                    if (sublineIdx != -1)
                        textBoxMain.Text = String.Concat(textBoxMain.Text, lines[i], "\n");
                }
            }
            GC.Collect();
        }


        private void textBoxMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (fileName.Length == 1 && fileName[0].EndsWith(".txt"))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            }
        }

        private void textBoxMain_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop);
                textBoxMain.Clear();
                textBoxMain.Text = File.ReadAllText(fileName[0], Encoding.Default);
            }
        }
    }
}
