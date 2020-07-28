using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BluepringNumbering
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtDir.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void txtDir_TextChanged(object sender, EventArgs e)
        {
            ViewList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            ViewList();

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(this.txtDir.Text))
            {
                return;
            }
            var dir = new System.IO.DirectoryInfo(this.txtDir.Text);
            foreach (var item in renameList)
            {
                var path1 = System.IO.Path.Combine(dir.FullName, item.Key);
                var path2 = System.IO.Path.Combine(dir.FullName, item.Value);
                System.IO.File.Move(path1, path2);
            }
            MessageBox.Show("completed");
        }

        private Dictionary<string, string> renameList = new Dictionary<string, string>();

        private void ViewList()
        {
            if (!System.IO.Directory.Exists(this.txtDir.Text))
            {
                return;
            }
            this.listBox1.Items.Clear();
            this.listBox2.Items.Clear();
            this.renameList.Clear();

            var dir = new System.IO.DirectoryInfo(this.txtDir.Text);
            var files = dir.GetFiles();
            var nums = new char[]{ '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            foreach (var file in files)
            {
                var phase = 0;
                var prefix = "";
                var suffix = "";
                var major = "";
                var miner = "";
                var delimiter = "";
                foreach (var c in file.Name)
                {
                    if (nums.Contains(c))
                    {
                        if (phase <= 1)
                        {
                            phase = 1;
                            major += c;
                        }
                        else if (phase <= 3)
                        {
                            phase = 3;
                            miner += c;
                        }
                    }
                    else
                    {
                        if (phase == 0)
                        {
                            prefix += c;
                        }
                        else if (phase <= 2)
                        {
                            delimiter += c;
                            phase = 2;
                        }
                        else if (phase <= 4)
                        {
                            suffix += c;
                            phase = 4;
                        }
                    }
                }

                if (major.Length < numMajor.Value)
                {
                    major = major.PadLeft((int)numMajor.Value, '0');
                }
                var m = 0;
                if (chkEda.Checked && !string.IsNullOrWhiteSpace(miner) && int.TryParse(miner, out m) && miner.Length < numMiner.Value)
                {
                    miner = miner.PadLeft((int)numMiner.Value, '0');
                }

                var newFileName = prefix + major + delimiter + miner + suffix;
                this.listBox1.Items.Add(file.Name);
                this.listBox2.Items.Add(newFileName);
                renameList.Add(file.Name, newFileName);
            }
        }
    }
}
