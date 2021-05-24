using System;
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

        PoljeInfo[,] polja = new PoljeInfo[8, 8];
        PictureBox[,] tabla = new PictureBox[8, 8];
        List<Figura> figure = new List<Figura>();
        AI_engine AI = new AI_engine();
        private void Form1_Load(object sender, EventArgs e)
        {
            int l = 50;
            int t = 50;
            int size = 60;
            //pocetne zauzete pozicije
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (i == 0 || i == 1)
                    {
                        polja[j, i].zauzeto = true;
                        polja[j, i].boja = Boja.crna;
                    }

                    else if(i == 6 || i == 7)
                    {
                        polja[j, i].zauzeto = true;
                        polja[j, i].boja = Boja.bela;
                    }


                    else
                        polja[j, i].zauzeto = false;
                }
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox4.SizeMode = PictureBoxSizeMode.CenterImage;

            pictureBox1.BackColor = Color.Black;
            pictureBox2.BackColor = Color.Black;
            pictureBox3.BackColor = Color.Black;
            pictureBox4.BackColor = Color.Black;

            pictureBox1.Image = Image.FromFile("Bela kraljica.png");
            pictureBox2.Image = Image.FromFile("Beli top.png");
            pictureBox3.Image = Image.FromFile("Beli skakac.png");
            pictureBox4.Image = Image.FromFile("Beli lovac.png");

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
            polja[0, 0].zauzeto = true;
            polja[7, 0].zauzeto = true;
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

        PoljeInfo[,] KopiranjePolja(ref PoljeInfo[,] komedija)
        {
            PoljeInfo[,] kopija = new PoljeInfo[8, 8];
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    PoljeInfo dz;
                    dz.zauzeto = komedija[i, j].zauzeto;
                    dz.boja = komedija[i, j].boja;
                    kopija[i, j] = dz;
                }
            }
            return kopija;
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
            foreach(var x in f.MoguciPotezi(figure, ref polja))
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
        
        public bool Sah(Boja b, ref List<Figura> ecovece, ref PoljeInfo[,] mast)
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
                    foreach(var p in x.NapadnutaPolja(ecovece, ref mast))
                    {
                        if (p == pozicijaKralja)
                            return true;
                    }
                }
            }
            return false;
        }

        private bool Mat(Boja b, ref List<Figura> ecovece)
        {
            ecovece = KopiranjeListe(ref figure);
            /*
            List<Figura> kopija = KopiranjeListe(ref ecovece);
            PoljeInfo[,] xd = KopiranjePolja(ref polja);
            for(int i = 0; i < kopija.Count; i++)
            {
                Figura figura = kopija[i]; 
                if(figura.GetBoja() == b)
                {
                    var pozicija = figura.GetPozicija();
                    var potezi = figura.MoguciPotezi(kopija, ref xd);
                    Figura pojedena = new Pesak(new Point(1, 1), Boja.bela, Image.FromFile("Crna kraljica.png"));
                    foreach (var potez in potezi)
                    {
                        xd[pozicija.X, pozicija.Y].zauzeto = false;
                        Boja pocBoja = xd[potez.X, potez.Y].boja;
                        xd[potez.X, potez.Y].zauzeto = true;
                        xd[potez.X, potez.Y].boja = b;
                        int index = NadjiFiguru(potez, ref kopija);
                        if (index != -1)
                            pojedena = kopija[index];
                        Point megaXd = Pesak.anPasan;
                        figura.OdigrajPotez(potez, ref kopija, ref xd);
                        Pesak.anPasan = megaXd;
                        if (!Sah(b, ref kopija, ref xd))
                        {
                            //this.Text = figura.Vrsta + " " + potez.X.ToString() + " " + potez.Y.ToString();
                            return false;
                        }
                        xd[pozicija.X, pozicija.Y].zauzeto = true;
                        xd[potez.X, potez.Y].zauzeto = false;
                        xd[potez.X, potez.Y].boja = pocBoja;
                        figura.SetPozicija(pozicija);
                        if (index != -1)
                            kopija.Add(pojedena);
                    }
                }
            }  
            return true;*/
            foreach(var figura in ecovece)
            {
                if (figura.GetBoja() == b && figura.MoguciPotezi(figure, ref polja).Count > 0)
                {
                    return false;
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

        void CrtajTablu(ref List<Figura> figure)
        {
            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tabla[i, j].Image = null;
                }
            }

            foreach(var x in figure)
            {
                tabla[x.GetPozicija().X, x.GetPozicija().Y].Image = x.GetImage();
            }
        }

        int PesakNaKraju(ref List<Figura> figure)
        {
            for(int i = 0; i < figure.Count; i++)
            {
                Figura x = figure[i];
                if(x.Vrsta == "pesak")
                {
                    if (x.GetBoja() == Boja.bela && x.GetPozicija().Y == 0)
                        return i;
                    if (x.GetBoja() == Boja.crna && x.GetPozicija().Y == 7)
                        return i;
                }
            }
            return -1;
        }

        bool beliIgra = true;
        bool selektovanje = true;
        Figura potez;
        bool matic = false;
        private void KlikNaPolje(object sender, EventArgs e)
        {
            if (!matic && !backgroundWorker1.IsBusy)
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
                        this.Text = i.ToString() + " " + j.ToString();
                        if (figpoz != -1 && figure[figpoz].GetBoja() == Boja.bela)
                        {
                            ObojiMogucaPolja(figure[figpoz].MoguciPotezi(figure, ref polja));
                            tabla[i, j].BackColor = Color.LightBlue;
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
                        bool odigraj = potez.OdigrajPotez(new Point(i, j), ref figure, ref polja);

                        if (odigraj)
                        {
                            tabla[pomoc.X, pomoc.Y].Image = null;
                            tabla[i, j].Image = potez.GetImage();
                            selektovanje = true;
                            beliIgra = false;
                            CrtajTablu(ref figure);

                            if(PesakNaKraju(ref figure) != -1)
                            {
                                for(int k = 0; k < 8; k++)
                                {
                                    for (int h = 0; h < 8; h++)
                                    {
                                        tabla[k, h].Enabled = false;
                                    }
                                }
                                pictureBox1.Enabled = true;
                                pictureBox2.Enabled = true;
                                pictureBox3.Enabled = true;
                                pictureBox4.Enabled = true;
                                pictureBox1.Visible = true;
                                pictureBox2.Visible = true;
                                pictureBox3.Visible = true;
                                pictureBox4.Visible = true;
                            }
                            
                            List<Figura> kopija = KopiranjeListe(ref figure);
                            if (Mat(Boja.crna, ref kopija) && Sah(Boja.crna, ref kopija, ref polja))
                            {
                                MessageBox.Show("Beli je pobedio");
                                matic = true;
                            }
                            else if(Mat(Boja.crna, ref kopija))
                            {
                                MessageBox.Show("Pat");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Nemoguc potez");
                            selektovanje = true;
                        }
                    }
                }

                if(!beliIgra)
                {
                    //poziv engina
                    backgroundWorker1.RunWorkerAsync();
                    //}
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int index = PesakNaKraju(ref figure);
            Figura dama;
            if (figure[index].GetBoja() == Boja.crna)
                dama = new Dama(figure[index].GetPozicija(), Boja.crna, Image.FromFile("Crna kraljica.png"));
            else
                dama = new Dama(figure[index].GetPozicija(), Boja.bela, Image.FromFile("Bela kraljica.png"));

            figure.RemoveAt(index);
            figure.Add(dama);
            ObojTablu();

            tabla[dama.GetPozicija().X, dama.GetPozicija().Y].Image = null;
            tabla[dama.GetPozicija().X, dama.GetPozicija().Y].Image = dama.GetImage();

            pictureBox1.Enabled = false;
            pictureBox1.Visible = false;
            pictureBox2.Enabled = false;
            pictureBox2.Visible = false;
            pictureBox3.Enabled = false;
            pictureBox3.Visible = false;
            pictureBox4.Enabled = false;
            pictureBox4.Visible = false;

            for (int k = 0; k < 8; k++)
            {
                for (int h = 0; h < 8; h++)
                {
                    tabla[k, h].Enabled = true;
                }
            }

            List<Figura> kopija = KopiranjeListe(ref figure);
            if (Mat(Boja.bela, ref kopija) && Sah(Boja.bela, ref kopija, ref polja))
            {
                MessageBox.Show("Crni je pobedio");
                matic = true;
            }
            else if (Mat(Boja.bela, ref kopija))
            {
                MessageBox.Show("Pat");
                matic = true;
            }
            
            kopija = KopiranjeListe(ref figure);
            if (Mat(Boja.crna, ref kopija) && Sah(Boja.crna, ref kopija, ref polja))
            {
                MessageBox.Show("Beli je pobedio");
                matic = true;
            }
            else if (Mat(Boja.crna, ref kopija))
            {
                MessageBox.Show("Pat");
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            int index = PesakNaKraju(ref figure);
            Figura top;
            if (figure[index].GetBoja() == Boja.crna)
                top = new Top(figure[index].GetPozicija(), Boja.crna, Image.FromFile("Crni top.png"));
            else
                top = new Top(figure[index].GetPozicija(), Boja.bela, Image.FromFile("Beli top.png"));

            figure.RemoveAt(index);
            figure.Add(top);

            tabla[top.GetPozicija().X, top.GetPozicija().Y].Image = null;
            tabla[top.GetPozicija().X, top.GetPozicija().Y].Image = top.GetImage();
            ObojTablu();

            pictureBox1.Enabled = false;
            pictureBox1.Visible = false;
            pictureBox2.Enabled = false;
            pictureBox2.Visible = false;
            pictureBox3.Enabled = false;
            pictureBox3.Visible = false;
            pictureBox4.Enabled = false;
            pictureBox4.Visible = false;

            for (int k = 0; k < 8; k++)
            {
                for (int h = 0; h < 8; h++)
                {
                    tabla[k, h].Enabled = true;
                }
            }

            List<Figura> kopija = KopiranjeListe(ref figure);
            if (Mat(Boja.bela, ref kopija) && Sah(Boja.bela, ref kopija, ref polja))
            {
                MessageBox.Show("Crni je pobedio");
                matic = true;
            }
            else if (Mat(Boja.bela, ref kopija))
            {
                MessageBox.Show("Pat");
                matic = true;
            }

            kopija = KopiranjeListe(ref figure);
            if (Mat(Boja.crna, ref kopija) && Sah(Boja.crna, ref kopija, ref polja))
            {
                MessageBox.Show("Beli je pobedio");
                matic = true;
            }
            else if (Mat(Boja.crna, ref kopija))
            {
                MessageBox.Show("Pat");
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            int index = PesakNaKraju(ref figure);
            Figura skakac;
            if (figure[index].GetBoja() == Boja.crna)
                skakac = new Skakac(figure[index].GetPozicija(), Boja.crna, Image.FromFile("Crni skakac.png"));
            else
                skakac = new Skakac(figure[index].GetPozicija(), Boja.bela, Image.FromFile("Beli skakac.png"));

            figure.RemoveAt(index);
            figure.Add(skakac);
            ObojTablu();

            tabla[skakac.GetPozicija().X, skakac.GetPozicija().Y].Image = null;
            tabla[skakac.GetPozicija().X, skakac.GetPozicija().Y].Image = skakac.GetImage();

            pictureBox1.Enabled = false;
            pictureBox1.Visible = false;
            pictureBox2.Enabled = false;
            pictureBox2.Visible = false;
            pictureBox3.Enabled = false;
            pictureBox3.Visible = false;
            pictureBox4.Enabled = false;
            pictureBox4.Visible = false;

            for (int k = 0; k < 8; k++)
            {
                for (int h = 0; h < 8; h++)
                {
                    tabla[k, h].Enabled = true;
                }
            }

            List<Figura> kopija = KopiranjeListe(ref figure);
            if (Mat(Boja.bela, ref kopija) && Sah(Boja.bela, ref kopija, ref polja))
            {
                MessageBox.Show("Crni je pobedio");
                matic = true;
            }
            else if (Mat(Boja.bela, ref kopija))
            {
                MessageBox.Show("Pat");
                matic = true;
            }

            kopija = KopiranjeListe(ref figure);
            if (Mat(Boja.crna, ref kopija) && Sah(Boja.crna, ref kopija, ref polja))
            {
                MessageBox.Show("Beli je pobedio");
                matic = true;
            }
            else if (Mat(Boja.crna, ref kopija))
            {
                MessageBox.Show("Pat");
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            int index = PesakNaKraju(ref figure);
            Figura lovac;
            if (figure[index].GetBoja() == Boja.crna)
                lovac = new Lovac(figure[index].GetPozicija(), Boja.crna, Image.FromFile("Crni lovac.png"));
            else
                lovac = new Lovac(figure[index].GetPozicija(), Boja.bela, Image.FromFile("Beli lovac.png"));

            figure.RemoveAt(index);
            figure.Add(lovac);

            tabla[lovac.GetPozicija().X, lovac.GetPozicija().Y].Image = null;
            tabla[lovac.GetPozicija().X, lovac.GetPozicija().Y].Image = lovac.GetImage();
            ObojTablu();

            pictureBox1.Enabled = false;
            pictureBox1.Visible = false;
            pictureBox2.Enabled = false;
            pictureBox2.Visible = false;
            pictureBox3.Enabled = false;
            pictureBox3.Visible = false;
            pictureBox4.Enabled = false;
            pictureBox4.Visible = false;

            for (int k = 0; k < 8; k++)
            {
                for (int h = 0; h < 8; h++)
                {
                    tabla[k, h].Enabled = true;
                }
            }

            List<Figura> kopija = KopiranjeListe(ref figure);
            if (Mat(Boja.bela, ref kopija) && Sah(Boja.bela, ref kopija, ref polja))
            {
                MessageBox.Show("Crni je pobedio");
                matic = true;
            }
            else if (Mat(Boja.bela, ref kopija))
            {
                MessageBox.Show("Pat");
                matic = true;
            }

            kopija = KopiranjeListe(ref figure);
            if (Mat(Boja.crna, ref kopija) && Sah(Boja.crna, ref kopija, ref polja))
            {
                MessageBox.Show("Beli je pobedio");
                matic = true;
            }
            else if (Mat(Boja.crna, ref kopija))
            {
                MessageBox.Show("Pat");
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            long nebitno;
            Point gde;
            Point igram;
            (nebitno, gde, igram) = AI.Game(ref figure, ref polja, 3, Boja.crna);
            int cimeigram = NadjiFiguru(igram);
            potez = figure[cimeigram];
            ObojTablu();
            int index = NadjiFiguru(gde);
            Figura pojedena = figure[0];
            if (index != -1)
            {
                pojedena = figure[index];
            }
            Point pomoc = potez.GetPozicija();
            bool odigraj = potez.OdigrajPotez(gde, ref figure, ref polja);

            if (odigraj)
            {

                tabla[pomoc.X, pomoc.Y].Image = null;
                //tabla[i, j].Image = potez.GetImage();
                selektovanje = true;
                beliIgra = true;
                CrtajTablu(ref figure);

                if (PesakNaKraju(ref figure) != -1)
                {
                    int momenat = PesakNaKraju(ref figure);
                    Figura dama;
                    if (figure[momenat].GetBoja() == Boja.crna)
                        dama = new Dama(figure[momenat].GetPozicija(), Boja.crna, Image.FromFile("Crna kraljica.png"));
                    else
                        dama = new Dama(figure[momenat].GetPozicija(), Boja.bela, Image.FromFile("Bela kraljica.png"));

                    figure.RemoveAt(momenat);
                    figure.Add(dama);
                    ObojTablu();

                    tabla[dama.GetPozicija().X, dama.GetPozicija().Y].Image = null;
                    tabla[dama.GetPozicija().X, dama.GetPozicija().Y].Image = dama.GetImage();

                    List<Figura> xd = KopiranjeListe(ref figure);
                    if (Mat(Boja.bela, ref xd) && Sah(Boja.bela, ref xd, ref polja))
                    {
                        MessageBox.Show("Crni je pobedio");
                        matic = true;
                    }
                    else if (Mat(Boja.bela, ref xd))
                    {
                        MessageBox.Show("Pat");
                        matic = true;
                    }

                }
                List<Figura> kopija = KopiranjeListe(ref figure);
                if (Mat(Boja.bela, ref kopija) && Sah(Boja.bela, ref kopija, ref polja))
                {
                    MessageBox.Show("Crni je pobedio");
                    matic = true;
                }
                else if (Mat(Boja.bela, ref kopija))
                {
                    MessageBox.Show("Pat");
                    matic = true;
                }
            }
            else
            {
                MessageBox.Show(potez.Vrsta + " " + gde);
                selektovanje = true;
            }
        }
    }
}
