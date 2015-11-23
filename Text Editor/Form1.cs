using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace Text_Editor
{
    public partial class Form1 : Form
    {
        //deklaracije
        Font font;
        string savePath;

        public Form1()
        {
            InitializeComponent();
            savePath = "";
            Font = textBox1.Font;
        }

        //funkcija za cuvanje fajlova; bool prompt je za 'save as', da se cuva pod drugim imenom.
        public void saveD(bool prompt)
        {
            //ako fajl nije sacuvan da pita gde se cuva
            if (savePath == "" || prompt == true)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Text File|*.txt|All Files|*.*";
                save.Title = "Save";
                save.ShowDialog();

                if (save.FileName != "")
                {
                    savePath = save.FileName;
                }
            }
            //ako filename nije prazan, da ga cuva
            else if (savePath != "")
            {
                using (StreamWriter writer = new StreamWriter(savePath))
                {
                    writer.Write(textBox1.Text);
                }
            }
        }

        //file menu exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //file menu new
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            savePath = "";
        }

        //file menu save
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveD(false);
        }

        //file menu save as
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveD(true);
        }

        //otvaranje tekst dokumenta
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text File|*.txt|All Files|*.*";
            open.Title = "Open";
            open.ShowDialog();

            if (open.FileName != "")
            {
                textBox1.Text = File.ReadAllText(open.FileName);
                savePath = open.FileName;
            }
        }

        //word wrap funkcija
        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.WordWrap == false)
            {
                textBox1.WordWrap = true;
                textBox1.ScrollBars = ScrollBars.Vertical;
                wordWrapToolStripMenuItem.Checked = true;
            }
            else
            {
                textBox1.WordWrap = false;
                textBox1.ScrollBars = ScrollBars.Both;
                wordWrapToolStripMenuItem.Checked = false;
            }
        }

        //stampanje
        private StreamReader streamToPrint;

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog print = new PrintDialog();
            print.ShowDialog();

            /*if (savePath == "")
            {
                MessageBox.Show("Save the current file or open an existing file!", "Error");
            }
            else
            {
                try
                {
                    streamToPrint = new StreamReader(savePath);
                    try
                    {
                        PrintDocument pd = new PrintDocument();
                        pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                        pd.Print();
                    }
                    finally
                    {
                        streamToPrint.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }*/
        }

        /*private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               font.GetHeight(ev.Graphics);

            // Print each line of the file.
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count *
                   font.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, font, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }*/
        //kraj stampanja

        //biranje fonta
        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontd = new FontDialog();
            DialogResult result = fontd.ShowDialog();
            if (result == DialogResult.OK)
            {
                font = fontd.Font;
                textBox1.Font = font;
            }
        }

        //ovo treba da bude ctrl + s, ali ne radi
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(Control.ModifierKeys == Keys.Control && Control.ModifierKeys == Keys.S)
            {
                saveD(false);
            }
        }
    }
}
