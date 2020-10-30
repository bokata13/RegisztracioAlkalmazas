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
using System.Globalization;

namespace RegisztracioAlkalmazas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var lista = listBox1.Items.Cast<String>().ToList();
            try
            {
                bool ok = false;
                if (string.IsNullOrWhiteSpace(tb_ujhobbi.Text))
                {
                    MessageBox.Show("Az új hobbi mező üres vagy csak szóközöket tartalmaz. Töltsd ki a mezőt!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tb_ujhobbi.Clear();
                    tb_ujhobbi.Focus();
                }
                else 
                {
                    if (lista.Contains(tb_ujhobbi.Text))
                    {
                        MessageBox.Show("A megnevezett hobbi már benne van a listában, nem adjuk hozzá ismét", "Információ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        tb_ujhobbi.Clear();
                        tb_ujhobbi.Focus();
                    }
                    else
                    {
                        ok = true;
                    }

                    if (ok == true)
                    {
                        listBox1.Items.Add((tb_ujhobbi.Text).Trim());
                        tb_ujhobbi.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {

            DateTime dDate;
            DateTime dTCurrent = DateTime.Now;
            string nev = tb_nev.Text;
            string szuldat = "";
            string nem = "";
            string hobbi = "";
            bool szuldatok = false;
            bool hobbiok = false;
            bool nevok = false;
            bool nemok = false;

            try 
            {

                if (nev.Trim().Length == 0)
                {
                    MessageBox.Show("Az név mező üres vagy csak szóközöket tartalmaz. Töltsd ki a mezőt!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tb_nev.Clear();
                    tb_nev.Focus();

                }
                else 
                {
                    nevok = true;
                }

                if ((tb_szuldatum.Text).Trim().Length > 0)
                {
                    string[] formats = { "yyyy/mm/dd", "yyyy/m/d" , "yyyy/mm/d", "yyyy/m/d", "yyyy.mm.dd", "yyyy.m.d", "yyyy.mm.d", "yyyy.m.d", "yyyy-mm-dd", "yyyy-m-d", "yyyy-mm-d", "yyyy-m-d" };

                    if (DateTime.TryParseExact(tb_szuldatum.Text, formats, CultureInfo.CurrentCulture, DateTimeStyles.None, out dDate))
                    {
                        szuldat = tb_szuldatum.Text;

                        int currentDateValues = Convert.ToInt32(dTCurrent.ToString("yyyymmdd"));
                        int inputDateValues = Convert.ToInt32(szuldat.Replace("/", "").Replace(".", "").Replace("-", ""));

                        if (inputDateValues > currentDateValues)
                        {
                            MessageBox.Show("A szül dátum nem lehet a jövőben. Töltsd ki a mezőt helyesen!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tb_szuldatum.Clear();
                            tb_szuldatum.Focus();
                            szuldat = "";
                        }
                        else
                        {
                            szuldatok = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("A szül dátum nem megfelelő. Lehetéges formátumok: yyyy/mm/dd, yyyy.mm.dd, yyyy-mm-dd", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else 
                {
                    MessageBox.Show("Az szül datáum mező üres vagy csak szóközöket tartalmaz. Töltsd ki a mezőt!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tb_szuldatum.Clear();
                    tb_szuldatum.Focus();
                }

                if (rb_f.Checked || rb_n.Checked)
                {
                    if (rb_f.Checked)
                    {
                        nem = "férfi";
                    }
                    else
                    {
                        nem = "nő";
                    }

                    nemok = true;

                }
                else
                {
                    MessageBox.Show("Az nem megjelölése kötelező", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rb_f.Focus();
                    rb_n.Focus();
                }

                if (listBox1.SelectedIndex >= 0)
                {
                    hobbi = Convert.ToString(listBox1.SelectedItem);
                    hobbiok = true;
                }
                else
                {
                    MessageBox.Show("Kedvenc hobbi kiválasztása kötelező", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    listBox1.Focus();
                }


                if (nemok == true && hobbiok == true && nevok == true && szuldatok == true)
                {
                    var lista = listBox1.Items.Cast<String>().ToList();
                    Stream text;
                    SaveFileDialog mentes = new SaveFileDialog();

                    mentes.Filter = "txt files (*.txt)|*.txt";
                    mentes.FilterIndex = 2;
                    mentes.RestoreDirectory = true;
                    mentes.AddExtension = true;

                    if (mentes.ShowDialog() == DialogResult.OK)
                    {
                        if ((text = mentes.OpenFile()) != null)
                        {
                            StreamWriter iras = new StreamWriter(text);

                            iras.WriteLine(nev +";"+ szuldat + ";" + nem + ";" + hobbi);

                            for (int i = 0; i < lista.Count; i++)
                            {
                                iras.Write(lista[i] + ";");
                            }

                            iras.Close();
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> lista = new List<string>();
            Stream szoveg = null;
            OpenFileDialog betoltes = new OpenFileDialog();
            betoltes.Title = "File megnyitása";
            betoltes.Filter = "TXT files|*.txt";
            betoltes.RestoreDirectory = true;
            betoltes.AddExtension = true;
            if (betoltes.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((szoveg = betoltes.OpenFile()) != null)
                    {
                        using (szoveg)
                        {
                            StreamReader olvas = new StreamReader(szoveg);
                            while (!olvas.EndOfStream)
                            {
                                lista.Add(olvas.ReadLine());
                            }
                        }
                    }


                    String beolvasott = lista[0];
                    char[] spearator = { ';' };
                    List<string> adatok = new List<string>(beolvasott.Split(spearator));

                    tb_nev.Text = Convert.ToString(adatok[0]);
                    tb_szuldatum.Text = Convert.ToString(adatok[1]);

                    if (Convert.ToString(adatok[2]) == "nő")
                    {
                        rb_n.Checked = true;
                    }
                    else 
                    {
                        rb_f.Checked = true;
                    }

                    String str = lista[1];
                    List<string> strlist = new List<string>(str.Split(spearator));

                    foreach (String s in strlist)
                    {
                        listBox1.DataSource = strlist;
                        if (s == (Convert.ToString(adatok[3])))
                        {
                            listBox1.SelectedItem = s;
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}
