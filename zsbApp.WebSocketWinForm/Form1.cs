using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zsbApp.WebSocketWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Shown += (s, e) => {
                this.toolStripTextBox1.Text = "ws://127.0.0.1:9146/";
            };
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            int index = this.tabControl1.TabPages.Count + 1;
            UserControl1 uc = new UserControl1(this.toolStripTextBox1.Text, index);
            TabPage tp = new TabPage();
            uc.Parent = tp;
            tp.Text = index.ToString();
            uc.Dock = DockStyle.Fill;
            this.tabControl1.TabPages.Add(tp);
            this.tabControl1.SelectedTab = tp;
        }
    }
}
