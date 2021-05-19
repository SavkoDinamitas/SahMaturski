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

        public abstract List<Point> MoguciPotezi(List<Figura> figure);

        public abstract bool OdigrajPotez(Point p, ref List<Figura> figure);

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

        public override List<Point> MoguciPotezi(List<Figura> figure)
        {
            List<Point> potezi = new List<Point>();
            Point[] tacke = {new Point(this.pozicija.X - 1, this.pozicija.Y - 1), new Point(this.pozicija.X, this.pozicija.Y - 1), new Point(this.pozicija.X + 1, this.pozicija.Y - 1),
            new Point(this.pozicija.X - 1, this.pozicija.Y), new Point(this.pozicija.X + 1, this.pozicija.Y), new Point(this.pozicija.X - 1, this.pozicija.Y + 1),
            new Point(this.pozicija.X, this.pozicija.Y + 1), new Point(this.pozicija.X + 1, this.pozicija.Y + 1)};

            List<Point> wtf;
            foreach(var x  in tacke)
            {
                if (InBound(x))
                {
                    bool pomoc = false;
                    foreach(var y in figure)
                    {
                        if(y.GetBoja() != boja)
                        {
                            if(y.Vrsta == "kralj")
                            {
                                wtf = new List<Point>() {new Point(y.GetPozicija().X - 1, y.GetPozicija().Y - 1), new Point(y.GetPozicija().X, y.GetPozicija().Y - 1), new Point(y.GetPozicija().X + 1, y.GetPozicija().Y - 1),
                                new Point(y.GetPozicija().X - 1, y.GetPozicija().Y), new Point(y.GetPozicija().X + 1, y.GetPozicija().Y), new Point(y.GetPozicija().X - 1, y.GetPozicija().Y + 1),
                                new Point(y.GetPozicija().X, y.GetPozicija().Y + 1), new Point(y.GetPozicija().X + 1, y.GetPozicija().Y + 1)};
                            }
                            else if(y.Vrsta == "pesak")
                            {
                                if(y.GetBoja() == Boja.bela)
                                {
                                    wtf = new List<Point>() { new Point(y.GetPozicija().X + 1, y.GetPozicija().Y - 1), new Point(y.GetPozicija().X - 1, y.GetPozicija().Y - 1) };
                                }
                                else
                                {
                                    wtf = new List<Point>() { new Point(y.GetPozicija().X + 1, y.GetPozicija().Y + 1), new Point(y.GetPozicija().X - 1, y.GetPozicija().Y + 1) };
                                }
                            }
                            else
                            {
                                wtf = y.MoguciPotezi(figure);
                            }
                            foreach (var k in wtf)
                            {
                                if (k == x)
                                {
                                    pomoc = true;
                                    break;
                                }
                            }
                        }
                        

                        if (y.GetPozicija() == x && boja == y.GetBoja())
                        {
                            pomoc = true;
                            break;
                        }

                        if (pomoc)
                            break;
                    }
                    if (!pomoc)
                    {
                        potezi.Add(x);
                    }
                }
            }
            //mala rokada belog kralja
            if(boja == Boja.bela)
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
                                wtf = x.MoguciPotezi(figure);
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
            else
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
                                wtf = x.MoguciPotezi(figure);
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
            if (boja == Boja.bela)
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
                                wtf = x.MoguciPotezi(figure);
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
            else
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

                        foreach (var y in x.MoguciPotezi(figure))
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
            
            return potezi;
        }

        public override bool OdigrajPotez(Point p, ref List<Figura> figure)
        {
            foreach(var x in this.MoguciPotezi(figure))
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
                                    break;
                                }
                            }
                        }
                    }
                    pozicija = p;
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

        public override List<Point> MoguciPotezi(List<Figura> figure)
        {
            List<Point> potezi = new List<Point>();
            int x = pozicija.X - 1;
            int y = pozicija.Y - 1;
                
            while(x >= 0 && y >= 0)
            {
                foreach(var figura in figure)
                {
                    if(figura.GetPozicija() == new Point(x, y))
                    {
                        if(figura.GetBoja() == boja)
                        {
                            goto l;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto l;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                x--;
                y--;
            }
            l:
            x = pozicija.X;
            y = pozicija.Y - 1;
            while (x >= 0 && y >= 0)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto l1;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto l1;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                y--;
            }
            l1:
            x = pozicija.X + 1;
            y = pozicija.Y - 1;
            while (x <= 7 && y >= 0)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto l2;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto l2;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                y--;
                x++;
            }
            l2:

            x = pozicija.X + 1;
            y = pozicija.Y;
            while (x <= 7 && y <= 7)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto l3;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto l3;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                x++;
            }
            l3:

            x = pozicija.X + 1;
            y = pozicija.Y + 1;
            while (x <= 7 && y <= 7)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto l4;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto l4;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                x++;
                y++;
            }
            l4:

            x = pozicija.X;
            y = pozicija.Y + 1;
            while (x <= 7 && y <= 7)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto l5;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto l5;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                y++;
            }
            l5:

            x = pozicija.X - 1;
            y = pozicija.Y + 1;
            while (x >= 0 && y <= 7)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto l6;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto l6;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                x--;
                y++;
            }
            l6:

            x = pozicija.X - 1;
            y = pozicija.Y;
            while (x >= 0 && y <= 7)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto l7;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto l7;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                x--;
            }
            l7:

            return potezi;
        }

        public override bool OdigrajPotez(Point p, ref List<Figura> figure)
        {
            foreach (var x in this.MoguciPotezi(figure))
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
                    pozicija = p;
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

        public override List<Point> MoguciPotezi(List<Figura> figure)
        {
            List<Point> potezi = new List<Point>();
            int x = pozicija.X - 1;
            int y = pozicija.Y - 1;

            while (x >= 0 && y >= 0)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto label3;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto label3;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                x--;
                y--;
            }
            label3:

            x = pozicija.X + 1;
            y = pozicija.Y - 1;

            while (x <= 7 && y >= 0)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto label2;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto label2;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                x++;
                y--;
            }
            label2:

            x = pozicija.X - 1;
            y = pozicija.Y + 1;

            while (x >= 0 && y <= 7)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto label1;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto label1;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                x--;
                y++;
            }
            label1:

            x = pozicija.X + 1;
            y = pozicija.Y + 1;

            while (x <= 7 && y <= 7)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto label;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto label;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                x++;
                y++;
            }
            label:
            return potezi;
        }

        public override bool OdigrajPotez(Point p, ref List<Figura> figure)
        {
            foreach (var x in this.MoguciPotezi(figure))
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
                    pozicija = p;
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

        public override List<Point> MoguciPotezi(List<Figura> figure)
        {
            List<Point> potezi = new List<Point>();
            int x = pozicija.X - 1;
            int y = pozicija.Y;

            while (x >= 0)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto lb;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto lb;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                x--;
            }
            lb:

            x = pozicija.X + 1;
            y = pozicija.Y;

            while (x <= 7)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto lb1;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto lb1;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                x++;
            }
            lb1:

            x = pozicija.X;
            y = pozicija.Y + 1;

            while (y <= 7)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto lb2;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto lb2;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                y++;
            }
            lb2:

            x = pozicija.X;
            y = pozicija.Y - 1;

            while (y >= 0)
            {
                foreach (var figura in figure)
                {
                    if (figura.GetPozicija() == new Point(x, y))
                    {
                        if (figura.GetBoja() == boja)
                        {
                            goto lb3;
                        }
                        else
                        {
                            potezi.Add(new Point(x, y));
                            goto lb3;
                        }
                    }
                }
                potezi.Add(new Point(x, y));
                y--;
            }
            lb3:
            return potezi;
        }

        public override bool OdigrajPotez(Point p, ref List<Figura> figure)
        {
            foreach (var x in this.MoguciPotezi(figure))
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
                    pozicija = p;
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

        public override List<Point> MoguciPotezi(List<Figura> figure)
        {
            List<Point> potezi = new List<Point>();

            Point[] tacke = {new Point(this.pozicija.X - 2, this.pozicija.Y - 1), new Point(this.pozicija.X - 1, this.pozicija.Y - 2), new Point(this.pozicija.X + 1, this.pozicija.Y - 2),
            new Point(this.pozicija.X + 2, this.pozicija.Y - 1), new Point(this.pozicija.X + 2, this.pozicija.Y + 1), new Point(this.pozicija.X + 1, this.pozicija.Y + 2),
            new Point(this.pozicija.X - 1, this.pozicija.Y + 2), new Point(this.pozicija.X - 2, this.pozicija.Y + 1)};

            foreach (var x in tacke)
            {
                if (InBound(x))
                {
                    bool pomoc = false;
                    foreach (var y in figure)
                    {
                        if (y.GetPozicija() == x && boja == y.GetBoja())
                        {
                            pomoc = true;
                            break;
                        }
                    }
                    if (!pomoc)
                        potezi.Add(x);
                }
            }
            return potezi;
        }

        public override bool OdigrajPotez(Point p, ref List<Figura> figure)
        {
            foreach (var x in this.MoguciPotezi(figure))
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

        public override List<Point> MoguciPotezi(List<Figura> figure)
        {
            List<Point> potezi = new List<Point>();

            bool ispred = false;

            if(boja == Boja.crna)
            {
                foreach (var x in figure)
                {
                    if (x.GetPozicija() == new Point(pozicija.X, pozicija.Y + 1))
                        ispred = true;
                    if (x.GetPozicija() == new Point(pozicija.X + 1, pozicija.Y + 1) && x.GetBoja() != boja)
                        potezi.Add(new Point(pozicija.X + 1, pozicija.Y + 1));
                    if(x.GetPozicija() == new Point(pozicija.X - 1, pozicija.Y + 1) && x.GetBoja() != boja)
                        potezi.Add(new Point(pozicija.X - 1, pozicija.Y + 1));
                }

                if (!ispred)
                    potezi.Add(new Point(pozicija.X, pozicija.Y + 1));
            }


            else
            {
                foreach (var x in figure)
                {
                    if (x.GetPozicija() == new Point(pozicija.X, pozicija.Y - 1))
                        ispred = true;
                    if (x.GetPozicija() == new Point(pozicija.X + 1, pozicija.Y - 1) && x.GetBoja() != boja)
                        potezi.Add(new Point(pozicija.X + 1, pozicija.Y - 1));
                    if (x.GetPozicija() == new Point(pozicija.X - 1, pozicija.Y - 1) && x.GetBoja() != boja)
                        potezi.Add(new Point(pozicija.X - 1, pozicija.Y - 1));
                }

                if (!ispred)
                    potezi.Add(new Point(pozicija.X, pozicija.Y - 1));
            }

            //ako je pesak na pocetku pa moze 2 poteza unapred da odigra
            ispred = false;
            if(boja == Boja.bela)
            {
                if(pozicija.Y == 6)
                {
                    foreach (var x in figure)
                    {
                        if (x.GetPozicija() == new Point(pozicija.X, pozicija.Y - 1))
                            ispred = true;
                        if (x.GetPozicija() == new Point(pozicija.X, pozicija.Y - 2))
                            ispred = true;
                    }
                    if (!ispred)
                        potezi.Add(new Point(pozicija.X, pozicija.Y - 2));
                }
            }

            else
            {
                if (pozicija.Y == 1)
                {
                    foreach (var x in figure)
                    {
                        if (x.GetPozicija() == new Point(pozicija.X, pozicija.Y + 1))
                            ispred = true;
                        if (x.GetPozicija() == new Point(pozicija.X, pozicija.Y + 2))
                            ispred = true;
                    }
                    if (!ispred)
                        potezi.Add(new Point(pozicija.X, pozicija.Y + 2));
                }
            }

            return potezi;
        }

        public override bool OdigrajPotez(Point p, ref List<Figura> figure)
        {
            foreach (var x in this.MoguciPotezi(figure))
            {
                //da li je moguc potez
                if (p == x)
                {
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
                    pozicija = p;
                    if(pozicija.Y == 0 || pozicija.Y == 7)
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
//code by Dimitrije Andzic