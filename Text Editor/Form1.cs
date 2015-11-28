using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;

namespace Text_Editor
{
    public partial class Form1 : Form
    {
        //deklaracije
        Font font;
        string savePath;
        bool textChanged;

        public Form1()
        {
            InitializeComponent();
            savePath = "";
            Font = textBox1.Font;
        }

        //otvaranje sa ekstenzije ako se dvoklikne na fajl (nemam pojma kako ovo radi; copy/paste sa stackoverflow-a)
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] args = System.Environment.GetCommandLineArgs();
            string filePath = args[0];
            for (int i = 0; i <= args.Length - 1; i++)
            {
                if (args[i].EndsWith(".exe") == false)
                {
                    textBox1.Text = System.IO.File.ReadAllText(args[i],
                    Encoding.Default);
                }
            }
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] args = System.Environment.GetCommandLineArgs();
            string filePath = args[0];
        }
        public sealed class StartupEventArgs : EventArgs
        {

        }

        //funkcija za cuvanje fajlova; bool prompt je za 'save as', da se cuva pod drugim imenom.
        public void saveD(bool prompt)
        {
            SaveFileDialog save = new SaveFileDialog();

            //ako fajl nije sacuvan da pita gde se cuva
            if (savePath == "" || prompt == true)
            {
                save.Filter = "Text File|*.txt|Config File|*.cfg|Log File|*.log|HTML File|*.html|CSS File|*.css|CSV File|*.csv|INI File|*.ini|JSON File|*.json|TSV File|*.tsv|XML File|*.xml|YAML File|*.yaml|All Files|*.*";
                save.Title = "Save";
                save.ShowDialog();

                savePath = save.FileName;
                this.Text = Path.GetFileName(save.FileName) + " - Text Editor";
            }

            //cuvanje fajla
            if (savePath != "")
            {
                textChanged = false;
                using (StreamWriter writer = new StreamWriter(savePath))
                {
                    writer.Write(textBox1.Text);
                }
            }
        }

        //otvaranje tekst fajla
        public void openD()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text File|*.txt|Config File|*.cfg|Log File|*.log|HTML File|*.html|CSS File|*.css|CSV File|*.csv|INI File|*.ini|JSON File|*.json|TSV File|*.tsv|XML File|*.xml|YAML File|*.yaml|All Files|*.*";
            open.Title = "Open";
            open.ShowDialog();

            if (open.FileName != "")
            {
                textBox1.Text = File.ReadAllText(open.FileName);
                savePath = open.FileName;
                this.Text = Path.GetFileName(open.FileName) + " - Text Editor";
                textChanged = false;
            }
        }

        //prikazivanje trenutne pozicije u status bar
        private void textBox1_SelectionChanged(object sender, EventArgs e)
        {
            if (statusBarToolStripMenuItem.Checked)
            {
                int line = textBox1.GetLineFromCharIndex(textBox1.SelectionStart);
                int column = textBox1.SelectionStart - textBox1.GetFirstCharIndexFromLine(line);

                line += 1; column += 1;

                statusStrip_label_line.Text = "Ln: " + line.ToString();
                statusStrip_label_column.Text = "Col: " + column.ToString();
            }
        }

        //gleda ako se tekst promenio da bi se gledalo da li da daje promptExit() na izlazu.
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textChanged = true;
        }

        //file menu exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (promptExit())
            {
                Application.ExitThread();
            }
        }

        //file menu new
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            savePath = "";
            this.Text = "Text Editor";
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
            openD();
        }

        //undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Undo();
        }
        
        //redo
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Redo();
        }

        //cut
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Cut();
        }

        //copy
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Copy();
        }

        //uzimanje trenutnog datuma i sati i stavljanje u tekst
        private void getDateAndTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            textBox1.SelectedText = now.ToString();
        }

        //paste
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Paste();
        }

        //word wrap funkcija
        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.WordWrap == false)
            {
                textBox1.WordWrap = true;
                wordWrapToolStripMenuItem.Checked = true;
            }
            else
            {
                textBox1.WordWrap = false;
                wordWrapToolStripMenuItem.Checked = false;
            }
        }

        //stampanje
        public void printD()
        {
            //PrintDialog print = new PrintDialog();
            printDialog1.ShowDialog();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printD();
        }

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

        //view status bar
        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (statusBarToolStripMenuItem.Checked)
            {
                statusStrip1.Visible = false;
                textBox1.Height += 23;
                statusBarToolStripMenuItem.Checked = false;
            }
            else if (!statusBarToolStripMenuItem.Checked)
            {
                statusStrip1.Visible = true;
                textBox1.Height -= 23;
                statusBarToolStripMenuItem.Checked = true;
            }
        }

        //pita da li zeli da sacuva
        private bool promptExit()
        {
            DialogResult result;

            if (textBox1.Text == "" || !textChanged)
            {
                return true;
            }
            else
            {
                result = MessageBox.Show("Exiting will discart any unsaved changes. Do you wish to save?", "Text Editor", MessageBoxButtons.YesNoCancel);
            }

            if (result == DialogResult.Yes)
            {
                saveD(false);
                return true;
            }
            else if (result == DialogResult.No)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //poruka na zatvaranju programa
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!promptExit())
            {
                e.Cancel = true;
            }
        }

        //otvaranje forme about
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.ShowDialog();
        }

        //search funkcija
        private void textBox_search_TextChanged(object sender, EventArgs e)
        {
            int len = textBox1.TextLength;
            int index = 0;
            int lastIndex = textBox1.Text.LastIndexOf(textBox_search.Text, StringComparison.OrdinalIgnoreCase);

            while (index <= lastIndex)
            {
                textBox1.Find(textBox_search.Text, index, len, RichTextBoxFinds.None);
                textBox1.SelectionBackColor = Color.Yellow;
                index = textBox1.Text.IndexOf(textBox_search.Text, index) + 1;
            }
        }

        //otvaranje foldera gde se nalazi file
        private void openFolderLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(savePath))
            {
                Process.Start("explorer", Path.GetDirectoryName(savePath));
            }
            else
            {
                MessageBox.Show("There is no file to open a location for!", "Error");
            }
        }

        //key combinations
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                saveD(false);
            }
            if (e.Control && e.KeyCode == Keys.O)
            {
                openD();
            }
            if (e.Control && e.KeyCode == Keys.N)
            {
                textBox1.Clear();
                savePath = "";
            }
            if (e.Control && e.KeyCode == Keys.P)
            {
                printD();
            }
        }
    }
}
