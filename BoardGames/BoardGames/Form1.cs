﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoardGames
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        PictureBox[,] tabla = new PictureBox[8, 8];
        List<Figura> figure = new List<Figura>();
        private void Form1_Load(object sender, EventArgs e)
        {
            int l = 50;
            int t = 50;
            //velicina tekstboksa zavisi od rezolucije monitora
            int size = 60;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tabla[j, i] = new PictureBox();
                    tabla[j, i].Left = l;
                    tabla[j, i].Top = t;
                    tabla[j, i].Width = size;
                    tabla[j, i].Height = size;
                    if (i % 2 == j % 2)
                    {
                        tabla[j, i].BackColor = Color.White;
                    }
                    else
                    {
                        tabla[j, i].BackColor = Color.Black;
                    }

                    tabla[j, i].SizeMode = PictureBoxSizeMode.CenterImage;
                    tabla[j, i].Tag = j + "," + i;
                    tabla[j, i].Click += new EventHandler(KlikNaPolje);
                    tabla[j, i].Visible = true;
                    //tabla[j, i].Name = "pictureBox" + i.ToString() + j.ToString();
                    Controls.Add(tabla[j, i]);

                    l += size;
                }
                t += size;
                l = 50;
            }

            //pesaci
            for (int i = 0; i < 8; i++)
            {
                Pesak p = new Pesak(new Point(i, 1), Boja.crna, Image.FromFile("CrniPijun.png"));
                Pesak pb = new Pesak(new Point(i, 6), Boja.bela, Image.FromFile("Beli pijun.png"));
                figure.Add(p);
                figure.Add(pb);
                tabla[i, 1].Image = p.GetImage();
                tabla[i, 6].Image = pb.GetImage();
            }

            //topovi
            Top top = new Top(new Point(0, 0), Boja.crna, Image.FromFile("Crni top.png"));
            Top top2 = new Top(new Point(7, 0), Boja.crna, Image.FromFile("Crni top.png"));
            tabla[0, 0].Image = top.GetImage();
            tabla[7, 0].Image = top2.GetImage();
            figure.Add(top);
            figure.Add(top2);
            Top top3 = new Top(new Point(0, 7), Boja.bela, Image.FromFile("Beli top.png"));
            Top top4 = new Top(new Point(7, 7), Boja.bela, Image.FromFile("Beli top.png"));
            tabla[0, 7].Image = top3.GetImage();
            tabla[7, 7].Image = top4.GetImage();
            figure.Add(top3);
            figure.Add(top4);

            //skakaci
            Skakac sk = new Skakac(new Point(1, 0), Boja.crna, Image.FromFile("Crni skakac.png"));
            Skakac sk2 = new Skakac(new Point(6, 0), Boja.crna, Image.FromFile("Crni skakac.png"));
            tabla[1, 0].Image = sk.GetImage();
            tabla[6, 0].Image = sk2.GetImage();
            figure.Add(sk);
            figure.Add(sk2);
            Skakac sk3 = new Skakac(new Point(1, 7), Boja.bela, Image.FromFile("Beli skakac.png"));
            Skakac sk4 = new Skakac(new Point(6, 7), Boja.bela, Image.FromFile("Beli skakac.png"));
            tabla[1, 7].Image = sk3.GetImage();
            tabla[6, 7].Image = sk4.GetImage();
            figure.Add(sk3);
            figure.Add(sk4);

            //lovci
            Lovac lov = new Lovac(new Point(2, 0), Boja.crna, Image.FromFile("Crni lovac.png"));
            Lovac lov2 = new Lovac(new Point(5, 0), Boja.crna, Image.FromFile("Crni lovac.png"));
            tabla[2, 0].Image = lov.GetImage();
            tabla[5, 0].Image = lov2.GetImage();
            figure.Add(lov);
            figure.Add(lov2);
            Lovac lov3 = new Lovac(new Point(2, 7), Boja.bela, Image.FromFile("Beli lovac.png"));
            Lovac lov4 = new Lovac(new Point(5, 7), Boja.bela, Image.FromFile("Beli lovac.png"));
            tabla[2, 7].Image = lov3.GetImage();
            tabla[5, 7].Image = lov4.GetImage();
            figure.Add(lov3);
            figure.Add(lov4);

            //kraljice
            Dama d = new Dama(new Point(3, 0), Boja.crna, Image.FromFile("Crna kraljica.png"));
            Dama d2 = new Dama(new Point(3, 7), Boja.bela, Image.FromFile("Bela kraljica.png"));
            tabla[3, 0].Image = d.GetImage();
            tabla[3, 7].Image = d2.GetImage();
            figure.Add(d);
            figure.Add(d2);

            //kraljevi
            Kralj kralj = new Kralj(new Point(4, 0), Boja.crna, Image.FromFile("Crni kralj.png"));
            Kralj kralj2 = new Kralj(new Point(4, 7), Boja.bela, Image.FromFile("Beli kralj.png"));
            tabla[4, 0].Image = kralj.GetImage();
            tabla[4, 7].Image = kralj2.GetImage();
            figure.Add(kralj);
            figure.Add(kralj2);
        }

        private int NadjiFiguru(Point pozicija)
        {
            int poz = -1;
            int i = 0;
            foreach(var x in figure)
            {
                Point autizam = x.GetPozicija();
                if (x.GetPozicija() == pozicija)
                {
                    poz = i;
                    break;
                }
                i++;
            }
            return poz;
        }

        private int NadjiFiguru(Point pozicija, ref List<Figura> jesvala)
        {
            int poz = -1;
            int i = 0;
            foreach (var x in jesvala)
            {
                Point autizam = x.GetPozicija();
                if (x.GetPozicija() == pozicija)
                {
                    poz = i;
                    break;
                }
                i++;
            }
            return poz;
        }

        private void ObojiMogucaPolja(List<Point> tacke)
        {
            foreach(var c in tacke)
            {
                tabla[c.X, c.Y].BackColor = Color.LightBlue;
            }
        }

        private bool Moguce(Point p, Figura f)
        {
            foreach(var x in f.MoguciPotezi(figure))
            {
                if (x == p)
                    return true;
            }
            return false;
        }

        private void ObojTablu()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i % 2 == j % 2)
                    {
                        tabla[j, i].BackColor = Color.White;
                    }
                    else
                    {
                        tabla[j, i].BackColor = Color.Black;
                    }
                }
            }
        }
        
        private bool Sah(Boja b, ref List<Figura> ecovece)
        {
            Point pozicijaKralja = new Point(1, 1);
            foreach(var x in ecovece)
            {
                if(x.GetBoja() == b && x.Vrsta == "kralj")
                {
                    pozicijaKralja = x.GetPozicija();
                }
            }

            foreach(var x in ecovece)
            {
                if(x.GetBoja() != b)
                {
                    foreach (var y in x.MoguciPotezi(ecovece))
                    {
                        if (y == pozicijaKralja)
                            return true;
                    }
                }
            }
            return false;
        }

        private bool Mat(Boja b, ref List<Figura> ecovece)
        {
            foreach(var figura in ecovece)
            {
                var pozicija = figura.GetPozicija();
                var potezi = figura.MoguciPotezi(ecovece);
                foreach(var potez in potezi)
                {
                    figura.OdigrajPotez(potez, ref ecovece);
                    if (!Sah(b, ref ecovece))
                        return false;
                    figura.OdigrajPotez(pozicija, ref ecovece);
                }
            }
            return true;
        }

        private List<Figura> KopiranjeListe(ref List<Figura> figure)
        {
            List<Figura> kopija = new List<Figura>();
            foreach(var x in figure)
            {
                switch (x.Vrsta)
                {
                    case "kralj":
                        kopija.Add(new Kralj(x.GetPozicija(), x.GetBoja(), x.GetImage()));
                        break;
                    case "dama":
                        kopija.Add(new Dama(x.GetPozicija(), x.GetBoja(), x.GetImage()));
                        break;
                    case "top":
                        kopija.Add(new Top(x.GetPozicija(), x.GetBoja(), x.GetImage()));
                        break;
                    case "lovac":
                        kopija.Add(new Lovac(x.GetPozicija(), x.GetBoja(), x.GetImage()));
                        break;
                    case "skakac":
                        kopija.Add(new Skakac(x.GetPozicija(), x.GetBoja(), x.GetImage()));
                        break;
                    case "pesak":
                        kopija.Add(new Pesak(x.GetPozicija(), x.GetBoja(), x.GetImage()));
                        break;
                }
            }
            return kopija;
        }
        /*
        private bool Mat(Boja b, List<Figura> autizam)
        {
            for(int i = 0; i < autizam.Count; i++)
            {
                Point bozePomozi = autizam[i].GetPozicija();
                if(autizam[i].GetBoja() == b)
                    foreach(var y in autizam[i].MoguciPotezi(autizam))
                    {
                        int index = NadjiFiguru(y, ref autizam);
                        Figura pojedena = autizam[0];
                        if (index != -1)
                        {
                            pojedena = autizam[index];
                        }
                        autizam[i].OdigrajPotez(y, ref autizam);
                        if (!Sah(b, ref autizam))
                            return false;
                        if (index != -1)
                        {
                            autizam.Add(pojedena);
                        }
                        autizam[i].SetPozicija(bozePomozi);
                    }
            }
            return true;
        }*/

        bool beliIgra = true;
        bool selektovanje = true;
        Figura potez;
        bool matic = false;
        private void KlikNaPolje(object sender, EventArgs e)
        {
            if (!matic)
            {
                string s = (sender as PictureBox).Tag.ToString();
                string[] unos = s.Split(',');
                int i = int.Parse(unos[0]);
                int j = int.Parse(unos[1]);

                if (beliIgra)
                {
                    if (selektovanje)
                    {
                        int figpoz = NadjiFiguru(new Point(i, j));
                        if (figpoz != -1 && figure[figpoz].GetBoja() == Boja.bela)
                        {
                            ObojiMogucaPolja(figure[figpoz].MoguciPotezi(figure));
                            selektovanje = false;
                            potez = figure[figpoz];
                        }
                    }
                    else
                    {
                        ObojTablu();
                        int index = NadjiFiguru(new Point(i, j));
                        Figura pojedena = figure[0];
                        if (index != -1)
                        {
                            pojedena = figure[index];
                        }
                        Point pomoc = potez.GetPozicija();
                        bool odigraj = potez.OdigrajPotez(new Point(i, j), ref figure);

                        if (odigraj)
                        {
                            if (Sah(Boja.bela, ref figure))
                            {
                                MessageBox.Show("Sah Vam je");
                                selektovanje = true;
                                potez.SetPozicija(pomoc);
                                if (index != -1)
                                {
                                    figure.Add(pojedena);
                                }
                            }
                            else
                            {
                                tabla[pomoc.X, pomoc.Y].Image = null;
                                tabla[i, j].Image = potez.GetImage();
                                selektovanje = true;
                                beliIgra = false;
                                if (potez.Vrsta == "kralj")
                                {
                                    if (Math.Abs(pomoc.X - potez.GetPozicija().X) > 1)
                                    {
                                        if (potez.GetBoja() == Boja.bela)
                                        {
                                            if (pomoc.X - potez.GetPozicija().X > 0)
                                            {
                                                tabla[0, 7].Image = null;
                                                tabla[3, 7].Image = Image.FromFile("Beli top.png");
                                            }
                                            else
                                            {
                                                tabla[7, 7].Image = null;
                                                tabla[5, 7].Image = Image.FromFile("Beli top.png");
                                            }
                                        }
                                        else
                                        {
                                            if (pomoc.X - potez.GetPozicija().X > 0)
                                            {
                                                tabla[0, 0].Image = null;
                                                tabla[3, 0].Image = Image.FromFile("Crni top.png");
                                            }
                                            else
                                            {
                                                tabla[7, 0].Image = null;
                                                tabla[5, 0].Image = Image.FromFile("Crni top.png");
                                            }
                                        }
                                    }
                                }
                            }
                            /*
                            List<Figura> kopija = KopiranjeListe(ref figure);
                            if (Mat(Boja.crna, kopija))
                            {
                                MessageBox.Show("Beli je pobedio");
                                matic = true;
                            }*/
                        }
                        else
                        {
                            MessageBox.Show("Nemoguc potez");
                            selektovanje = true;
                        }
                    }
                }

                else
                {
                    if (selektovanje)
                    {
                        int figpoz = NadjiFiguru(new Point(i, j));
                        if (figpoz != -1 && figure[figpoz].GetBoja() == Boja.crna)
                        {
                            ObojiMogucaPolja(figure[figpoz].MoguciPotezi(figure));
                            selektovanje = false;
                            potez = figure[figpoz];
                        }
                    }
                    else
                    {
                        ObojTablu();
                        int index = NadjiFiguru(new Point(i, j));
                        Figura pojedena = figure[0];
                        if (index != -1)
                        {
                            pojedena = figure[index];
                        }
                        Point pomoc = potez.GetPozicija();
                        bool odigraj = potez.OdigrajPotez(new Point(i, j), ref figure);

                        if (odigraj)
                        {
                            if (Sah(Boja.crna, ref figure))
                            {
                                MessageBox.Show("Sah Vam je");
                                selektovanje = true;
                                potez.SetPozicija(pomoc);
                                if (index != -1)
                                {
                                    figure.Add(pojedena);
                                }
                            }
                            else
                            {
                                tabla[pomoc.X, pomoc.Y].Image = null;
                                tabla[i, j].Image = potez.GetImage();
                                selektovanje = true;
                                beliIgra = true;
                                if (potez.Vrsta == "kralj")
                                {
                                    if (Math.Abs(pomoc.X - potez.GetPozicija().X) > 1)
                                    {
                                        if (potez.GetBoja() == Boja.bela)
                                        {
                                            if (pomoc.X - potez.GetPozicija().X > 0)
                                            {
                                                tabla[0, 7].Image = null;
                                                tabla[3, 7].Image = Image.FromFile("Beli top.png");
                                            }
                                            else
                                            {
                                                tabla[7, 7].Image = null;
                                                tabla[5, 7].Image = Image.FromFile("Beli top.png");
                                            }
                                        }
                                        else
                                        {
                                            if (pomoc.X - potez.GetPozicija().X > 0)
                                            {
                                                tabla[0, 0].Image = null;
                                                tabla[3, 0].Image = Image.FromFile("Crni top.png");
                                            }
                                            else
                                            {
                                                tabla[7, 0].Image = null;
                                                tabla[5, 0].Image = Image.FromFile("Crni top.png");
                                            }
                                        }
                                    }
                                }
                            }
                            /*
                            List<Figura> kopija = KopiranjeListe(ref figure);
                                if (Mat(Boja.bela, kopija))
                                {
                                    MessageBox.Show("Crni je pobedio");
                                    matic = true;
                                }*/
                        }
                        else
                        {
                            MessageBox.Show("Nemoguc potez");
                            selektovanje = true;
                        }
                    }
                }
            }
        }
    }
}
