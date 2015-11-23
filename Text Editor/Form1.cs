﻿using System;
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

        //otvaranje tekst fajla
        public void openD()
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

        //pita da li zeli da sacuva
        private bool promptExit()
        {
            if (textBox1.Text == "")
            {
                return true;
            }
            DialogResult result = MessageBox.Show("Exiting will discart any unsaved changes. Do you wish to save?", "Text Editor", MessageBoxButtons.YesNoCancel);

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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!promptExit())
            {
                e.Cancel = true;
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

        //otvaranje forme about
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.ShowDialog();
        }
    }
}
