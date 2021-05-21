using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BoardGames
{
    public struct PoljeInfo
    {
        public bool zauzeto;
        public Boja boja;
    }
    public enum Boja
    {
        bela,
        crna
    }

    public abstract class Figura
    {
        protected Point pozicija;
        protected Boja boja;

        public Figura(Point pozicija, Boja boja)
        {
            this.pozicija = pozicija;
            this.boja = boja;
        }

        public abstract Image GetImage();

        public abstract List<Point> MoguciPotezi(List<Figura> figure, ref PoljeInfo[,] polja);

        public abstract List<Point> NapadnutaPolja(List<Figura> figure, ref PoljeInfo[,] polja);

        public abstract bool OdigrajPotez(Point p, ref List<Figura> figure, ref PoljeInfo[,] polja);

        public Point GetPozicija()
        {
            return pozicija;
        }

        public Boja GetBoja()
        {
            return boja;
        }

        public abstract string Vrsta
        {
            get;
        }

        public abstract void SetPozicija(Point p);

    }

    public class Kralj : Figura
    {
        Image slika;
        private bool pomerioSe = false;

        public Kralj(Point pozicija, Boja boja, Image slika):base(pozicija, boja)
        {
            this.slika = slika;
        }

        public override Image GetImage()
        {
            return slika;
        }

        public override string Vrsta
        {
            get { return "kralj"; }
        }

        public override void SetPozicija(Point p)
        {
            pozicija = p;
        }

        private bool InBound(Point p)
        {
            if (p.X < 0 || p.X > 7 || p.Y < 0 || p.Y > 7)
                return false;
            return true;
        }

        public static PoljeInfo[,] KopiranjePolja(ref PoljeInfo[,] komedija)
        {
            PoljeInfo[,] kopija = new PoljeInfo[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    PoljeInfo dz;
                    dz.zauzeto = komedija[i, j].zauzeto;
                    dz.boja = komedija[i, j].boja;
                    kopija[i, j] = dz;
                }
            }
            return kopija;
        }

        public static List<Figura> KopiranjeListe(ref List<Figura> xd)
        {
            List<Figura> kopija = new List<Figura>();
            foreach (var x in xd)
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

        public static bool Sah(Boja b, ref List<Figura> ecovece, ref PoljeInfo[,] mast)
        {
            Point pozicijaKralja = new Point(1, 1);
            foreach (var x in ecovece)
            {
                if (x.GetBoja() == b && x.Vrsta == "kralj")
                {
                    pozicijaKralja = x.GetPozicija();
                }
            }

            foreach (var x in ecovece)
            {
                if (x.GetBoja() != b)
                {
                    foreach (var p in x.NapadnutaPolja(ecovece, ref mast))
                    {
                        if (p == pozicijaKralja)
                            return true;
                    }
                }
            }
            return false;
        }

        public override List<Point> NapadnutaPolja(List<Figura> figure, ref PoljeInfo[,] polja)
        {
            List<Point> potezi = new List<Point>();
            Point[] tacke = {new Point(pozicija.X - 1, pozicija.Y - 1), new Point(pozicija.X, pozicija.Y - 1), new Point(pozicija.X + 1, pozicija.Y - 1),
            new Point(pozicija.X - 1, pozicija.Y), new Point(pozicija.X + 1, pozicija.Y), new Point(pozicija.X - 1, pozicija.Y + 1),
            new Point(pozicija.X, pozicija.Y + 1), new Point(pozicija.X + 1, pozicija.Y + 1)};
            foreach (var x in tacke)
            {
                if (InBound(x))
                {
                    potezi.Add(x);
                }
            }
            return potezi;
        }

        public override List<Point> MoguciPotezi(List<Figura> figure, ref PoljeInfo[,] polja)
        {
            List<Point> potezi = new List<Point>();
            Point[] tacke = {new Point(pozicija.X - 1, pozicija.Y - 1), new Point(pozicija.X, pozicija.Y - 1), new Point(pozicija.X + 1, pozicija.Y - 1),
            new Point(pozicija.X - 1, pozicija.Y), new Point(pozicija.X + 1, pozicija.Y), new Point(pozicija.X - 1, pozicija.Y + 1),
            new Point(pozicija.X, pozicija.Y + 1), new Point(pozicija.X + 1, pozicija.Y + 1)};

            
                foreach(var x in tacke)
                {
                    bool pomoc = false;
                    foreach(var figura in figure)
                    {
                        if (figura.GetBoja() != boja)
                            foreach (var komedija in figura.NapadnutaPolja(figure, ref polja))
                            {
                                if (komedija == x)
                                {
                                    pomoc = true;
                                }
                            }
                        else
                        {
                            if (figura.GetPozicija() == x)
                                pomoc = true;
                        }
                    }
                    if (!pomoc && InBound(x))
                        potezi.Add(x);
                }

            //mala rokada belog kralja
            List<Point> wtf;

            if (boja == Boja.bela && !pomerioSe  && !Sah(Boja.bela, ref figure, ref polja))
            {
                if (pozicija.X == 4 && pozicija.Y == 7)
                {
                    bool prazno = true;
                    foreach(var x in figure)
                    {
                        if (x.GetPozicija() == new Point(5, 7))
                        {
                            prazno = false;
                            break;
                        }
                            
                        if(x.GetPozicija() == new Point(6, 7))
                        {
                            prazno = false;
                            break;
                        }

                        if(boja != x.GetBoja())
                        {
                            if (x.Vrsta == "kralj")
                            {
                                wtf = new List<Point>() {new Point(x.GetPozicija().X - 1, x.GetPozicija().Y - 1), new Point(x.GetPozicija().X, x.GetPozicija().Y - 1), new Point(x.GetPozicija().X + 1, x.GetPozicija().Y - 1),
                                new Point(x.GetPozicija().X - 1, x.GetPozicija().Y), new Point(x.GetPozicija().X + 1, x.GetPozicija().Y), new Point(x.GetPozicija().X - 1, x.GetPozicija().Y + 1),
                                new Point(x.GetPozicija().X, x.GetPozicija().Y + 1), new Point(x.GetPozicija().X + 1, x.GetPozicija().Y + 1)};
                            }
                            else if (x.Vrsta == "pesak")
                            {
                                if (x.GetBoja() == Boja.bela)
                                {
                                    wtf = new List<Point>() { new Point(x.GetPozicija().X + 1, x.GetPozicija().Y - 1), new Point(x.GetPozicija().X - 1, x.GetPozicija().Y - 1) };
                                }
                                else
                                {
                                    wtf = new List<Point>() { new Point(x.GetPozicija().X + 1, x.GetPozicija().Y + 1), new Point(x.GetPozicija().X - 1, x.GetPozicija().Y + 1) };
                                }
                            }
                            else
                            {
                                wtf = x.NapadnutaPolja(figure, ref polja);
                            }

                            foreach (var y in wtf)
                            {
                                if (y == new Point(5, 7))
                                {
                                    prazno = false;
                                    break;
                                }

                                if (y == new Point(6, 7))
                                {
                                    prazno = false;
                                    break;
                                }
                            }
                        }
                        
                    }

                    bool top = false;
                    foreach(var x in figure)
                    {
                        if(x.GetPozicija() == new Point(7, 7) && x.GetBoja() == boja)
                        {
                            top = true;
                            break;
                        }
                    }
                    if(prazno && top)
                    {
                        potezi.Add(new Point(6, 7));
                    }
                }
            }

            //mala rokada crnog kralja
            else if(!pomerioSe && !Sah(Boja.crna, ref figure, ref polja))
            {
                if (pozicija.X == 4 && pozicija.Y == 0)
                {
                    bool prazno = true;
                    foreach (var x in figure)
                    {
                        if (x.GetPozicija() == new Point(5, 0))
                        {
                            prazno = false;
                            break;
                        }

                        if (x.GetPozicija() == new Point(6, 0))
                        {
                            prazno = false;
                            break;
                        }

                        if(boja != x.GetBoja())
                        {
                            if (x.Vrsta == "kralj")
                            {
                                wtf = new List<Point>() {new Point(x.GetPozicija().X - 1, x.GetPozicija().Y - 1), new Point(x.GetPozicija().X, x.GetPozicija().Y - 1), new Point(x.GetPozicija().X + 1, x.GetPozicija().Y - 1),
                                new Point(x.GetPozicija().X - 1, x.GetPozicija().Y), new Point(x.GetPozicija().X + 1, x.GetPozicija().Y), new Point(x.GetPozicija().X - 1, x.GetPozicija().Y + 1),
                                new Point(x.GetPozicija().X, x.GetPozicija().Y + 1), new Point(x.GetPozicija().X + 1, x.GetPozicija().Y + 1)};
                            }
                            else if (x.Vrsta == "pesak")
                            {
                                if (x.GetBoja() == Boja.bela)
                                {
                                    wtf = new List<Point>() { new Point(x.GetPozicija().X + 1, x.GetPozicija().Y - 1), new Point(x.GetPozicija().X - 1, x.GetPozicija().Y - 1) };
                                }
                                else
                                {
                                    wtf = new List<Point>() { new Point(x.GetPozicija().X + 1, x.GetPozicija().Y + 1), new Point(x.GetPozicija().X - 1, x.GetPozicija().Y + 1) };
                                }
                            }
                            else
                            {
                                wtf = x.NapadnutaPolja(figure, ref polja);
                            }

                            foreach (var y in wtf)
                            {
                                if (y == new Point(5, 0))
                                {
                                    prazno = false;
                                    break;
                                }

                                if (y == new Point(6, 0))
                                {
                                    prazno = false;
                                    break;
                                }
                            }
                        }
                    }

                    bool top = false;
                    foreach (var x in figure)
                    {
                        if (x.GetPozicija() == new Point(7, 0) && x.GetBoja() == boja)
                        {
                            top = true;
                            break;
                        }
                    }
                    if (prazno && top)
                    {
                        potezi.Add(new Point(6, 0));
                    }
                }
            }

            //velika rokada belog kralja
            if (boja == Boja.bela && !pomerioSe && !Sah(Boja.bela, ref figure, ref polja))
            {
                if (pozicija.X == 4 && pozicija.Y == 7)
                {
                    bool prazno = true;
                    foreach (var x in figure)
                    {
                        if (x.GetPozicija() == new Point(3, 7))
                        {
                            prazno = false;
                            break;
                        }

                        if (x.GetPozicija() == new Point(2, 7))
                        {
                            prazno = false;
                            break;
                        }

                        if(boja != x.GetBoja())
                        {
                            if (x.Vrsta == "kralj")
                            {
                                wtf = new List<Point>() {new Point(x.GetPozicija().X - 1, x.GetPozicija().Y - 1), new Point(x.GetPozicija().X, x.GetPozicija().Y - 1), new Point(x.GetPozicija().X + 1, x.GetPozicija().Y - 1),
                                new Point(x.GetPozicija().X - 1, x.GetPozicija().Y), new Point(x.GetPozicija().X + 1, x.GetPozicija().Y), new Point(x.GetPozicija().X - 1, x.GetPozicija().Y + 1),
                                new Point(x.GetPozicija().X, x.GetPozicija().Y + 1), new Point(x.GetPozicija().X + 1, x.GetPozicija().Y + 1)};
                            }
                            else if (x.Vrsta == "pesak")
                            {
                                if (x.GetBoja() == Boja.bela)
                                {
                                    wtf = new List<Point>() { new Point(x.GetPozicija().X + 1, x.GetPozicija().Y - 1), new Point(x.GetPozicija().X - 1, x.GetPozicija().Y - 1) };
                                }
                                else
                                {
                                    wtf = new List<Point>() { new Point(x.GetPozicija().X + 1, x.GetPozicija().Y + 1), new Point(x.GetPozicija().X - 1, x.GetPozicija().Y + 1) };
                                }
                            }
                            else
                            {
                                wtf = x.NapadnutaPolja(figure, ref polja);
                            }

                            foreach (var y in wtf)
                        {
                            if (y == new Point(3, 7) && boja != x.GetBoja())
                            {
                                prazno = false;
                                break;
                            }

                            if (y == new Point(2, 7) && boja != x.GetBoja())
                            {
                                prazno = false;
                                break;
                            }
                        }
                        }

                        
                    }

                    bool top = false;
                    foreach (var x in figure)
                    {
                        if (x.GetPozicija() == new Point(0, 7) && x.GetBoja() == boja)
                        {
                            top = true;
                            break;
                        }
                    }
                    if (prazno && top)
                    {
                        potezi.Add(new Point(2, 7));
                    }
                }
            }

            //velika rokada crnog kralja
            else if(!pomerioSe && !Sah(Boja.crna, ref figure, ref polja))
            {
                if (pozicija.X == 4 && pozicija.Y == 0)
                {
                    bool prazno = true;
                    foreach (var x in figure)
                    {
                        if (x.GetPozicija() == new Point(3, 0))
                        {
                            prazno = false;
                            break;
                        }

                        if (x.GetPozicija() == new Point(2, 0))
                        {
                            prazno = false;
                            break;
                        }

                        foreach (var y in x.NapadnutaPolja(figure, ref polja))
                        {
                            if (y == new Point(3, 0) && boja != x.GetBoja())
                            {
                                prazno = false;
                                break;
                            }

                            if (y == new Point(2, 0) && boja != x.GetBoja())
                            {
                                prazno = false;
                                break;
                            }
                        }
                    }

                    bool top = false;
                    foreach (var x in figure)
                    {
                        if (x.GetPozicija() == new Point(0, 0) && x.GetBoja() == boja)
                        {
                            top = true;
                            break;
                        }
                    }
                    if (prazno && top)
                    {
                        potezi.Add(new Point(2, 0));
                    }
                }
            }
            
            foreach(var x in potezi)
            {

            }

            return potezi;
        }

        public override bool OdigrajPotez(Point p, ref List<Figura> figure, ref PoljeInfo[,] polja)
        {
            foreach(var x in this.MoguciPotezi(figure, ref polja))
            {
                if(p == x)
                {
                    int i = 0;
                    bool pojeo = false;
                    foreach (var figura in figure)
                    {
                        if (figura.GetPozicija() == p)
                        {
                            pojeo = true;
                            break;
                        }
                        i++;
                    }
                    if (pojeo)
                    {
                        figure.RemoveAt(i);
                    }

                    if (Math.Abs(pozicija.X - p.X) > 1)
                    {
                        if(p == new Point(2, 7))
                        {
                            foreach(var top in figure)
                            {
                                if(top.Vrsta == "top" && top.GetPozicija() == new Point(0, 7))
                                {
                                    top.SetPozicija(new Point(3, 7));
                                    polja[0, 7].zauzeto = false;
                                    polja[3, 7].zauzeto = true;
                                    polja[3, 7].boja = boja;
                                    break;
                                }
                            }
                        }

                        if (p == new Point(6, 7))
                        {
                            foreach (var top in figure)
                            {
                                if (top.Vrsta == "top" && top.GetPozicija() == new Point(7, 7))
                                {
                                    top.SetPozicija(new Point(5, 7));
                                    polja[7, 7].zauzeto = false;
                                    polja[5, 7].zauzeto = true;
                                    polja[5, 7].boja = boja;
                                    break;
                                }
                            }
                        }

                        if (p == new Point(2, 0))
                        {
                            foreach (var top in figure)
                            {
                                if (top.Vrsta == "top" && top.GetPozicija() == new Point(0, 0))
                                {
                                    top.SetPozicija(new Point(3, 0));
                                    polja[0, 0].zauzeto = false;
                                    polja[3, 0].zauzeto = true;
                                    polja[3, 0].boja = boja;
                                    break;
                                }
                            }
                        }

                        if (p == new Point(6, 0))
                        {
                            foreach (var top in figure)
                            {
                                if (top.Vrsta == "top" && top.GetPozicija() == new Point(7, 0))
                                {
                                    top.SetPozicija(new Point(5, 0));
                                    polja[7, 0].zauzeto = false;
                                    polja[5, 0].zauzeto = true;
                                    polja[5, 0].boja = boja;
                                    break;
                                }
                            }
                        }
                    }
                    polja[pozicija.X, pozicija.Y].zauzeto = false;
                    polja[p.X, p.Y].zauzeto = true;
                    polja[p.X, p.Y].boja = boja;
                    pozicija = p;
                    Pesak.anPasan = new Point(-1, -1);
                    pomerioSe = true;
                    return true;
                }
            }
            return false;
        }
    }

    public class Dama : Figura
    {
        Image slika;

        public Dama(Point pozicija, Boja boja, Image slika) : base(pozicija, boja)
        {
            this.slika = slika;
        }

        public override Image GetImage()
        {
            return slika;
        }

        public override string Vrsta
        {
            get { return "dama"; }
        }

        public override void SetPozicija(Point p)
        {
            pozicija = p;
        }

        public bool Sah(Boja b, ref List<Figura> ecovece, ref PoljeInfo[,] mast)
        {
            Point pozicijaKralja = new Point(1, 1);
            foreach (var x in ecovece)
            {
                if (x.GetBoja() == b && x.Vrsta == "kralj")
                {
                    pozicijaKralja = x.GetPozicija();
                }
            }

            foreach (var x in ecovece)
            {
                if (x.GetBoja() != b)
                {
                    foreach (var p in x.NapadnutaPolja(ecovece, ref mast))
                    {
                        if (p == pozicijaKralja)
                            return true;
                    }
                }
            }
            return false;
        }

        public override List<Point> NapadnutaPolja(List<Figura> figure, ref PoljeInfo[,] polja)
        {
            List<Point> potezi = new List<Point>();
            int x = pozicija.X - 1;
            int y = pozicija.Y - 1;
            //gore levo  
            while (x >= 0 && y >= 0)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                x--;
                y--;
            }
            //gore
            x = pozicija.X;
            y = pozicija.Y - 1;
            while (x >= 0 && y >= 0)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                y--;
            }

            //gore desno
            x = pozicija.X + 1;
            y = pozicija.Y - 1;
            while (x <= 7 && y >= 0)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                y--;
                x++;
            }

            //desno
            x = pozicija.X + 1;
            y = pozicija.Y;
            while (x <= 7 && y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                x++;
            }

            //desno dole
            x = pozicija.X + 1;
            y = pozicija.Y + 1;
            while (x <= 7 && y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                x++;
                y++;
            }

            //dole
            x = pozicija.X;
            y = pozicija.Y + 1;
            while (x <= 7 && y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                y++;
            }

            //dole levo
            x = pozicija.X - 1;
            y = pozicija.Y + 1;
            while (x >= 0 && y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                x--;
                y++;
            }

            //levo
            x = pozicija.X - 1;
            y = pozicija.Y;
            while (x >= 0 && y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                x--;
            }

            return potezi;
        }

        public override List<Point> MoguciPotezi(List<Figura> figure, ref PoljeInfo[,] polja)
        {
            List<Point> potezi = new List<Point>();
            int x = pozicija.X - 1;
            int y = pozicija.Y - 1;
            //gore levo  
            while(x >= 0 && y >= 0)
            {
                if(polja[x, y].zauzeto)
                {
                    if(polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                x--;
                y--;
            }
            //gore
            x = pozicija.X;
            y = pozicija.Y - 1;
            while (x >= 0 && y >= 0)
            {               
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                y--;
            }
            
            //gore desno
            x = pozicija.X + 1;
            y = pozicija.Y - 1;
            while (x <= 7 && y >= 0)
            {
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                y--;
                x++;
            }

            //desno
            x = pozicija.X + 1;
            y = pozicija.Y;
            while (x <= 7 && y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                x++;
            }
            
            //desno dole
            x = pozicija.X + 1;
            y = pozicija.Y + 1;
            while (x <= 7 && y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                x++;
                y++;
            }
            
            //dole
            x = pozicija.X;
            y = pozicija.Y + 1;
            while (x <= 7 && y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                y++;
            }
            
            //dole levo
            x = pozicija.X - 1;
            y = pozicija.Y + 1;
            while (x >= 0 && y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                x--;
                y++;
            }
      
            //levo
            x = pozicija.X - 1;
            y = pozicija.Y;
            while (x >= 0 && y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                x--;
            }
            
            /*List<Point> potezi1 = new List<Point>();
            foreach(var p in potezi)
            {
                int i = 0;
                bool pojeo = false;
                Point pomoc = pozicija;
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == p)
                    {
                        pojeo = true;
                        break;
                    }
                    i++;
                }
                Figura pojedena = new Pesak(new Point(1, 1), Boja.bela, Image.FromFile("CrniPijun.png"));
                if (pojeo)
                {
                    pojedena = figure[i];
                    figure.RemoveAt(i);
                }
                polja[pozicija.X, pozicija.Y].zauzeto = false;
                polja[p.X, p.Y].zauzeto = true;
                polja[p.X, p.Y].boja = boja;
                pozicija = p;
                if(!Sah(boja, ref figure, ref polja))
                {
                    potezi1.Add(p);
                }
                polja[pozicija.X, pozicija.Y].zauzeto = false;
                if (pojeo)
                {
                    polja[pozicija.X, pozicija.Y].zauzeto = true;

                }
                polja[pomoc.X, pomoc.Y].zauzeto = true;
                this.pozicija = pomoc;
                if (pojeo)
                {
                    figure.Add(pojedena);

                }
            }*/

            return potezi;
        }

        public override bool OdigrajPotez(Point p, ref List<Figura> figure, ref PoljeInfo[,] polja)
        {
            foreach (var x in this.MoguciPotezi(figure, ref polja))
            {
                if (p == x)
                {
                    int i = 0;
                    bool pojeo = false;
                    foreach (var figura in figure)
                    {
                        if (figura.GetPozicija() == p)
                        {
                            pojeo = true;
                            break;
                        }
                        i++;
                    }
                    if (pojeo)
                    {
                        figure.RemoveAt(i);
                    }
                    polja[pozicija.X, pozicija.Y].zauzeto = false;
                    polja[p.X, p.Y].zauzeto = true;
                    polja[p.X, p.Y].boja = boja;
                    pozicija = p;
                    Pesak.anPasan = new Point(-1, -1);
                    return true;
                }
            }
            return false;
        }

    }

    public class Lovac : Figura
    {
        Image slika;

        public Lovac(Point pozicija, Boja boja, Image slika) : base(pozicija, boja)
        {
            this.slika = slika;
        }

        public override Image GetImage()
        {
            return slika;
        }

        public override string Vrsta
        {
            get { return "lovac"; }
        }

        public override void SetPozicija(Point p)
        {
            pozicija = p;
        }

        public override List<Point> NapadnutaPolja(List<Figura> figure, ref PoljeInfo[,] polja)
        {
            List<Point> potezi = new List<Point>();
            int x = pozicija.X - 1;
            int y = pozicija.Y - 1;

            //gore levo
            while (x >= 0 && y >= 0)
            {
                foreach (var figura in figure)
                {
                    if (polja[x, y].zauzeto)
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                x--;
                y--;
            }

            x = pozicija.X + 1;
            y = pozicija.Y - 1;
            //gore desno
            while (x <= 7 && y >= 0)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                x++;
                y--;
            }

            x = pozicija.X - 1;
            y = pozicija.Y + 1;
            //dole levo
            while (x >= 0 && y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                x--;
                y++;
            }

            x = pozicija.X + 1;
            y = pozicija.Y + 1;
            //dole desno
            while (x <= 7 && y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                x++;
                y++;
            }
            return potezi;
        }

        public override List<Point> MoguciPotezi(List<Figura> figure, ref PoljeInfo[,] polja)
        {
            List<Point> potezi = new List<Point>();
            int x = pozicija.X - 1;
            int y = pozicija.Y - 1;

            //gore levo
            while (x >= 0 && y >= 0)
            {
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                x--;
                y--;
            }

            x = pozicija.X + 1;
            y = pozicija.Y - 1;
            //gore desno
            while (x <= 7 && y >= 0)
            {
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                x++;
                y--;
            }

            x = pozicija.X - 1;
            y = pozicija.Y + 1;
            //dole levo
            while (x >= 0 && y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                x--;
                y++;
            }

            x = pozicija.X + 1;
            y = pozicija.Y + 1;
            //dole desno
            while (x <= 7 && y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                x++;
                y++;
            }
            return potezi;
        }

        public override bool OdigrajPotez(Point p, ref List<Figura> figure, ref PoljeInfo[,] polja)
        {
            foreach (var x in this.MoguciPotezi(figure, ref polja))
            {
                if (p == x)
                {
                    int i = 0;
                    bool pojeo = false;
                    foreach (var figura in figure)
                    {
                        if (figura.GetPozicija() == p)
                        {
                            pojeo = true;
                            break;
                        }
                        i++;
                    }
                    if (pojeo)
                    {
                        figure.RemoveAt(i);
                    }
                    polja[pozicija.X, pozicija.Y].zauzeto = false;
                    polja[p.X, p.Y].zauzeto = true;
                    polja[p.X, p.Y].boja = boja;
                    pozicija = p;
                    Pesak.anPasan = new Point(-1, -1);
                    return true;
                }
            }
            return false;
        }
    }

    public class Top : Figura
    {
        Image slika;

        public Top(Point pozicija, Boja boja, Image slika) : base(pozicija, boja)
        {
            this.slika = slika;
        }

        public override Image GetImage()
        {
            return slika;
        }

        public override string Vrsta
        {
            get { return "top"; }
        }

        public override void SetPozicija(Point p)
        {
            pozicija = p;
        }

        public override List<Point> NapadnutaPolja(List<Figura> figure, ref PoljeInfo[,] polja)
        {
            List<Point> potezi = new List<Point>();
            int x = pozicija.X - 1;
            int y = pozicija.Y;
            //levo
            while (x >= 0)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                x--;
            }

            x = pozicija.X + 1;
            y = pozicija.Y;
            //desno
            while (x <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                x++;
            }

            x = pozicija.X;
            y = pozicija.Y + 1;
            //dole
            while (y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                y++;
            }

            x = pozicija.X;
            y = pozicija.Y - 1;
            //gore
            while (y >= 0)
            {
                if (polja[x, y].zauzeto)
                {
                    potezi.Add(new Point(x, y));
                    break;
                }
                potezi.Add(new Point(x, y));
                y--;
            }
            return potezi;
        }

        public override List<Point> MoguciPotezi(List<Figura> figure, ref PoljeInfo[,] polja)
        {
            List<Point> potezi = new List<Point>();
            int x = pozicija.X - 1;
            int y = pozicija.Y;
            //levo
            while (x >= 0)
            {
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                x--;
            }

            x = pozicija.X + 1;
            y = pozicija.Y;
            //desno
            while (x <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                x++;
            }

            x = pozicija.X;
            y = pozicija.Y + 1;
            //dole
            while (y <= 7)
            {
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                y++;
            }

            x = pozicija.X;
            y = pozicija.Y - 1;
            //gore
            while (y >= 0)
            {
                if (polja[x, y].zauzeto)
                {
                    if (polja[x, y].boja == boja)
                    {
                        break;
                    }
                    else
                    {
                        potezi.Add(new Point(x, y));
                        break;
                    }
                }
                potezi.Add(new Point(x, y));
                y--;
            }
            return potezi;
        }

        public override bool OdigrajPotez(Point p, ref List<Figura> figure, ref PoljeInfo[,] polja)
        {
            foreach (var x in this.MoguciPotezi(figure, ref polja))
            {
                if (p == x)
                {
                    int i = 0;
                    bool pojeo = false;
                    foreach (var figura in figure)
                    {
                        if (figura.GetPozicija() == p)
                        {
                            pojeo = true;
                            break;
                        }
                        i++;
                    }
                    if (pojeo)
                    {
                        figure.RemoveAt(i);
                    }
                    polja[pozicija.X, pozicija.Y].zauzeto = false;
                    polja[p.X, p.Y].zauzeto = true;
                    polja[p.X, p.Y].boja = boja;
                    pozicija = p;
                    Pesak.anPasan = new Point(-1, -1);
                    return true;
                }
            }
            return false;
        }
    }


    public class Skakac : Figura
    {
        Image slika;

        public Skakac(Point pozicija, Boja boja, Image slika) : base(pozicija, boja)
        {
            this.slika = slika;
        }

        public override Image GetImage()
        {
            return slika;
        }

        public override string Vrsta
        {
            get { return "skakac"; }
        }

        public override void SetPozicija(Point p)
        {
            pozicija = p;
        }

        private bool InBound(Point p)
        {
            if (p.X < 0 || p.X > 7 || p.Y < 0 || p.Y > 7)
                return false;
            return true;
        }

        public override List<Point> NapadnutaPolja(List<Figura> figure, ref PoljeInfo[,] polja)
        {
            List<Point> potezi = new List<Point>();

            Point[] tacke = {new Point(this.pozicija.X - 2, this.pozicija.Y - 1), new Point(this.pozicija.X - 1, this.pozicija.Y - 2), new Point(this.pozicija.X + 1, this.pozicija.Y - 2),
            new Point(this.pozicija.X + 2, this.pozicija.Y - 1), new Point(this.pozicija.X + 2, this.pozicija.Y + 1), new Point(this.pozicija.X + 1, this.pozicija.Y + 2),
            new Point(this.pozicija.X - 1, this.pozicija.Y + 2), new Point(this.pozicija.X - 2, this.pozicija.Y + 1)};

            foreach (var x in tacke)
            {
                if (InBound(x))
                {
                    potezi.Add(x);
                }
            }
            return potezi;
        }

        public override List<Point> MoguciPotezi(List<Figura> figure, ref PoljeInfo[,] polja)
        {
            List<Point> potezi = new List<Point>();

            Point[] tacke = {new Point(this.pozicija.X - 2, this.pozicija.Y - 1), new Point(this.pozicija.X - 1, this.pozicija.Y - 2), new Point(this.pozicija.X + 1, this.pozicija.Y - 2),
            new Point(this.pozicija.X + 2, this.pozicija.Y - 1), new Point(this.pozicija.X + 2, this.pozicija.Y + 1), new Point(this.pozicija.X + 1, this.pozicija.Y + 2),
            new Point(this.pozicija.X - 1, this.pozicija.Y + 2), new Point(this.pozicija.X - 2, this.pozicija.Y + 1)};

            foreach (var x in tacke)
            {
                if (InBound(x))
                {
                    if (polja[x.X, x.Y].zauzeto && boja == polja[x.X, x.Y].boja)
                    {
                        ;
                    }
                    else
                        potezi.Add(x);
                }
            }
            return potezi;
        }

        public override bool OdigrajPotez(Point p, ref List<Figura> figure, ref PoljeInfo[,] polja)
        {
            foreach (var x in this.MoguciPotezi(figure, ref polja))
            {
                if (p == x)
                {
                    int i = 0;
                    bool pojeo = false;
                    foreach (var figura in figure)
                    {
                        if (figura.GetPozicija() == p)
                        {
                            pojeo = true;
                            break;
                        }
                        i++;
                    }
                    if (pojeo)
                    {
                        figure.RemoveAt(i);
                    }
                    Pesak.anPasan = new Point(-1, -1);
                    polja[pozicija.X, pozicija.Y].zauzeto = false;
                    polja[p.X, p.Y].zauzeto = true;
                    polja[p.X, p.Y].boja = boja;
                    pozicija = p;
                    return true;
                }
            }
            return false;
        }
    }

    //i konacno pesak
    public class Pesak : Figura
    {
        Image slika;
        public static Point anPasan = new Point(-1, -1);

        public Pesak(Point pozicija, Boja boja, Image slika) : base(pozicija, boja)
        {
            this.slika = slika;
        }

        public override Image GetImage()
        {
            return slika;
        }

        public override string Vrsta
        {
            get { return "pesak"; }
        }

        public override void SetPozicija(Point p)
        {
            pozicija = p;
        }

        private bool InBound(Point p)
        {
            if (p.X < 0 || p.X > 7 || p.Y < 0 || p.Y > 7)
                return false;
            return true;
        }

        public override List<Point> NapadnutaPolja(List<Figura> figure, ref PoljeInfo[,] polja)
        {
            List<Point> potezi = new List<Point>();
            if (boja == Boja.bela)
            {
                potezi.Add(new Point(pozicija.X - 1, pozicija.Y - 1));
                potezi.Add(new Point(pozicija.X + 1, pozicija.Y - 1));
            }
            else
            {
                potezi.Add(new Point(pozicija.X - 1, pozicija.Y + 1));
                potezi.Add(new Point(pozicija.X + 1, pozicija.Y + 1));
            }
            return potezi;
        }

        public override List<Point> MoguciPotezi(List<Figura> figure, ref PoljeInfo[,] polja)
        {
            List<Point> potezi = new List<Point>();

            if(boja == Boja.crna)
            {
                if (!polja[pozicija.X, pozicija.Y+1].zauzeto)
                    potezi.Add(new Point(pozicija.X, pozicija.Y + 1));
                if (InBound(new Point(pozicija.X + 1, pozicija.Y + 1)) && polja[pozicija.X + 1, pozicija.Y + 1].zauzeto && polja[pozicija.X + 1, pozicija.Y + 1].boja != boja)
                    potezi.Add(new Point(pozicija.X + 1, pozicija.Y + 1));
                if(InBound(new Point(pozicija.X - 1, pozicija.Y + 1)) && polja[pozicija.X - 1, pozicija.Y + 1].zauzeto && polja[pozicija.X - 1, pozicija.Y + 1].boja != boja)
                    potezi.Add(new Point(pozicija.X - 1, pozicija.Y + 1));
            }


            else
            {
                if (!polja[pozicija.X, pozicija.Y - 1].zauzeto)
                    potezi.Add(new Point(pozicija.X, pozicija.Y - 1));
                if (InBound(new Point(pozicija.X + 1, pozicija.Y - 1)) && polja[pozicija.X + 1, pozicija.Y - 1].zauzeto && polja[pozicija.X + 1, pozicija.Y - 1].boja != boja)
                    potezi.Add(new Point(pozicija.X + 1, pozicija.Y - 1));
                if (InBound(new Point(pozicija.X - 1, pozicija.Y - 1)) && polja[pozicija.X - 1, pozicija.Y - 1].zauzeto && polja[pozicija.X - 1, pozicija.Y - 1].boja != boja)
                    potezi.Add(new Point(pozicija.X - 1, pozicija.Y - 1));                 
            }

            //ako je pesak na pocetku pa moze 2 poteza unapred da odigra
            if(boja == Boja.bela)
            {
                if(pozicija.Y == 6)
                {
                    if (!polja[pozicija.X, pozicija.Y - 1].zauzeto && !polja[pozicija.X, pozicija.Y - 2].zauzeto)
                        potezi.Add(new Point(pozicija.X, pozicija.Y - 2));
                }
            }

            else
            {
                if (pozicija.Y == 1)
                {
                    if (!polja[pozicija.X, pozicija.Y + 1].zauzeto && !polja[pozicija.X, pozicija.Y + 2].zauzeto)
                        potezi.Add(new Point(pozicija.X, pozicija.Y + 2));
                }
            }

            //an pasan
            if(anPasan.X != -1)
            {
                if(pozicija.Y == 3 && anPasan.Y == 3)
                {
                    if(Math.Abs(pozicija.X - anPasan.X) == 1 && !polja[anPasan.X, anPasan.Y - 1].zauzeto)
                    {
                        potezi.Add(new Point(anPasan.X, anPasan.Y - 1));
                    }
                }
                else if(pozicija.Y == 4 && anPasan.Y == 4)
                {
                    if (Math.Abs(pozicija.X - anPasan.X) == 1 && !polja[anPasan.X, anPasan.Y + 1].zauzeto)
                    {
                        potezi.Add(new Point(anPasan.X, anPasan.Y + 1));
                    }
                }
            }

            return potezi;
        }

        public override bool OdigrajPotez(Point p, ref List<Figura> figure, ref PoljeInfo[,] polja)
        {
            foreach (var x in this.MoguciPotezi(figure, ref polja))
            {
                //da li je moguc potez
                if (p == x)
                {
                    if (Math.Abs(pozicija.X - p.X) > 0 && !polja[p.X, p.Y].zauzeto)
                    {
                        int j = 0;
                        bool pojeo1 = false;
                        foreach (var figura in figure)
                        {
                            if (figura.GetPozicija() == anPasan)
                            {
                                pojeo1 = true;
                                break;
                            }
                            j++;
                        }
                        if (pojeo1)
                        {
                            polja[anPasan.X, anPasan.Y].zauzeto = false;
                            figure.RemoveAt(j);
                        }
                    }
                    int i = 0;
                    bool pojeo = false;
                    foreach(var figura in figure)
                    {
                        if(figura.GetPozicija() == p)
                        {
                            pojeo = true;
                            break;
                        }
                        i++;
                    }
                    if (pojeo)
                    {
                        figure.RemoveAt(i);
                    }
                    
                    if(Math.Abs(pozicija.Y - p.Y) > 1)
                    {
                        anPasan = p;
                    }
                    else
                    {
                        anPasan = new Point(-1, -1);
                    }
                    polja[pozicija.X, pozicija.Y].zauzeto = false;
                    polja[p.X, p.Y].zauzeto = true;
                    polja[p.X, p.Y].boja = boja;
                    pozicija = p;
                    return true;
                }
            }
            return false;
        }
    }
}
//code by Dimitrije Andzic