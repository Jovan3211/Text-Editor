using System;
using System.Drawing;
using System.Windows.Forms;

namespace Text_Editor
{
    public partial class Find : Form
    {
        public Find()
        {
            InitializeComponent();
        }

        Form1 form1 = new Form1();

        private void textBox_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            int len = form1.textBoxPublic.TextLength;
            int index = 0;
            int lastIndex = form1.textBoxPublic.Text.LastIndexOf(this.textBox_search.Text);

            while (index < lastIndex)
            {
                form1.textBoxPublic.Find(this.textBox_search.Text, index, len, RichTextBoxFinds.None);
                form1.textBoxPublic.SelectionBackColor = Color.Yellow;
                index = form1.textBoxPublic.Text.IndexOf(this.textBox_search.Text, index) + 1;
            }
        }

        private void Find_MouseEnter(object sender, EventArgs e)
        {
            form1.Opacity = 100;
        }

        private void Find_MouseLeave(object sender, EventArgs e)
        {
            form1.Opacity = 60;
        }
    }
}
