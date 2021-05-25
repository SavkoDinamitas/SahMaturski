using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGames
{
    public class AI_engine
    {

        private List<Figura> KopiranjeListe(List<Figura> figure)
        {
            List<Figura> kopija = new List<Figura>();
            foreach (var x in figure)
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

        PoljeInfo[,] KopiranjePolja(ref PoljeInfo[,] komedija)
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

        int[,] pesakPozB = new int[,] {{ 928, 928, 928, 928, 928, 928, 928, 928},
            { 78,  83,  86,  73, 102,  82,  85,  90 },
             { 7,  29,  21,  44,  40,  31,  44,   7 },
           { -17,  16,  -2,  15,  14,   0,  15, -13 },
           { -26,   3,  10,   9,   6,   1,   0, -23 },
           { -22,   9,   5, -11, -10,  -2,   3, -19},
           { -31,   8,  -7, -37, -36, -14,   3, -31},
            {0, 0, 0, 0, 0, 0, 0, 0}};

        int[,] skakacPozB = new int[,] { { -66, -53, -75, -75, -10, -55, -58, -7 },
            { -3,  -6, 100, -36,   4,  62,  -4, -14 },
            { 10,  67,   1,  74,  73,  27,  62,  -2},
            { 24,  24,  45,  37,  33,  41,  25,  17},
            { -1,   5,  31,  21,  22,  35,   2,   0},
           { -18,  10,  13,  22,  18,  15,  11, -14},
           { -23, -15,   2,   0,   2,   0, -23, -20},
           { -74, -23, -26, -24, -19, -35, -22, -69}};

        int[,] lovacPozB = new int[,] { { -59, -78, -82, -76, -23,-107, -37, -50 },
           { -11,  20,  35, -42, -39,  31,   2, -22 },
            { -9,  39, -32,  41,  52, -10,  28, -14},
            { 25,  17,  20,  34,  26,  25,  15,  10},
            { 13,  10,  17,  23,  17,  16,   0,   7},
            { 14,  25,  24,  15,   8,  25,  20,  15},
            { 19,  20,  11,   6,   7,   6,  20,  16},
            { -7,   2, -15, -12, -14, -15, -10, -10}};

        int[,] topPozB = new int[,] { { 35,  29,  33,   4,  37,  33,  56,  50 },
            { 55,  29,  56,  67,  55,  62,  34,  60 },
            { 19,  35,  28,  33,  45,  27,  25,  15},
            { 0,   5,  16,  13,  18,  -4,  -9,  -6},
           { -28, -35, -16, -21, -13, -29, -46, -30},
           { -42, -28, -42, -25, -25, -35, -26, -46},
           { -53, -38, -31, -26, -29, -43, -44, -53},
           { -30, -24, -18,   5,  -2, -18, -31, -32}};

        int[,] damaPozB = new int[,] { { 6,   1,  -8,-104,  69,  24,  88,  26 },
            { 14,  32,  60, -10,  20,  76,  57,  24 },
            { -2,  43,  32,  60,  72,  63,  43,   2},
            { 1, -16,  22,  17,  25,  20, -13,  -6},
           { -14, -15,  -2,  -5,  -1, -10, -20, -22},
           { -30,  -6, -13, -11, -16, -11, -16, -27},
           { -36, -18,   0, -19, -15, -15, -21, -38},
           { -39, -30, -31, -13, -31, -36, -34, -42}};

        int[,] kraljPozC = new int[,] { { 4,  54,  47, -99, -99,  60,  83, -62 },
           { -32,  10,  55,  56,  56,  55,  10,   3 },
           { -62,  12, -57,  44, -67,  28,  37, -31},
           { -55,  50,  11,  -4, -19,  13,   0, -49},
           { -55, -43, -52, -28, -51, -47,  -8, -50},
           { -47, -42, -43, -79, -64, -32, -29, -32},
            { -4,   3, -14, -50, -57, -18,  13,   4},
            { 17,  30,  -3, -14,   6,  -1,  40,  18}};




        int[,] pesakPozC = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0 },
                                       {-31,   8,  -7, -37, -36, -14,   3, -31},
                                       { -22,   9,   5, -11, -10,  -2,   3, -19},
                                       {-26,   3,  10,   9,   6,   1,   0, -23},
                                       {-17,  16,  -2,  15,  14,   0,  15, -13},
                                       {7,  29,  21,  44,  40,  31,  44,   7},
                                       {78,  83,  86,  73, 102,  82,  85,  90},
                                       {928,   928,   928,   928,   928,   928,   928,   928}};

        int[,] skakacPozC = new int[,] { { -74, -23, -26, -24, -19, -35, -22, -69 },
                                        {-23, -15,   2,   0,   2,   0, -23, -20},
                                        {-18,  10,  13,  22,  18,  15,  11, -14},
                                        {-1,   5,  31,  21,  22,  35,   2,   0},
                                        {24,  24,  45,  37,  33,  41,  25,  17},
                                        {10,  67,   1,  74,  73,  27,  62,  -2},
                                        {-3,  -6, 100, -36,   4,  62,  -4, -14},
                                        {-66, -53, -75, -75, -10, -55, -58, -70}};

        int[,] lovacPozC = new int[,] { { -7,   2, -15, -12, -14, -15, -10, -10 },
                                        {19,  20,  11,   6,   7,   6,  20,  16},
                                        {14,  25,  24,  15,   8,  25,  20,  15},
                                        {13,  10,  17,  23,  17,  16,   0,   7},
                                        {25,  17,  20,  34,  26,  25,  15,  10},
                                        {-9,  39, -32,  41,  52, -10,  28, -14},
                                        {-11,  20,  35, -42, -39,  31,   2, -22},
                                        {-59, -78, -82, -76, -23,-107, -37, -50}};

        int[,] topPozC = new int[,] { { -30, -24, -18,   5,  -2, -18, -31, -32 },
                                        {-53, -38, -31, -26, -29, -43, -44, -53},
                                        {-42, -28, -42, -25, -25, -35, -26, -46},
                                        {-28, -35, -16, -21, -13, -29, -46, -30},
                                        {0,   5,  16,  13,  18,  -4,  -9,  -6},
                                        {19,  35,  28,  33,  45,  27,  25,  15},
                                        {55,  29,  56,  67,  55,  62,  34,  60},
                                        {35,  29,  33,   4,  37,  33,  56,  50}};

        int[,] damaPozC = new int[,] { { -39, -30, -31, -13, -31, -36, -34, -42 },
                                        {-36, -18,   0, -19, -15, -15, -21, -38},
                                        {-30,  -6, -13, -11, -16, -11, -16, -27},
                                        {-14, -15,  -2,  -5,  -1, -10, -20, -22},
                                        {1, -16,  22,  17,  25,  20, -13,  -6},
                                        {-2,  43,  32,  60,  72,  63,  43,   2},
                                        {14,  32,  60, -10,  20,  76,  57,  24},
                                        {6,   1,  -8,-104,  69,  24,  88,  26}};

        int[,] kraljPozB = new int[,] { { 17,  30,  -3, -14,   6,  -1,  40,  18 },
                                        {-4,   3, -14, -50, -57, -18,  13,   4},
                                        {-47, -42, -43, -79, -64, -32, -29, -32},
                                        {-55, -43, -52, -28, -51, -47,  -8, -50},
                                        {-55,  50,  11,  -4, -19,  13,   0, -49},
                                        {-62,  12, -57,  44, -67,  28,  37, -31},
                                        {-32,  10,  55,  56,  56,  55,  10,   3},
                                        {4,  54,  47, -99, -99,  60,  83, -62}};


        bool Mat(Boja b, ref List<Figura> ecovece, ref PoljeInfo[,] polja)
        {
            var komedija = KopiranjeListe(ecovece);
            foreach (var figura in komedija)
            {
                if (figura.GetBoja() == b && figura.MoguciPotezi(ecovece, ref polja).Count > 0)
                {
                    return false;
                }
            }
            return true;
        }

        long StanjeTable(ref List<Figura> figure, ref PoljeInfo[,] polja)
        {
            if(Mat(Boja.bela, ref figure, ref polja))
            {
                return long.MinValue;
            }

            if(Mat(Boja.crna, ref figure, ref polja))
            {
                return long.MaxValue;
            }

            long skorCrni = 0;
            long skorBeli = 0;
            foreach(var figura in figure)
            {
                long skor = 0;
                if (figura.GetBoja() == Boja.bela)
                {
                    switch (figura.Vrsta)
                    {
                        case "kralj":
                            skor += kraljPozB[figura.GetPozicija().Y, figura.GetPozicija().X];
                            break;
                        case "dama":
                            skor += damaPozB[figura.GetPozicija().Y, figura.GetPozicija().X];
                            break;
                        case "top":
                            skor += topPozB[figura.GetPozicija().Y, figura.GetPozicija().X];
                            break;
                        case "lovac":
                            skor += lovacPozB[figura.GetPozicija().Y, figura.GetPozicija().X];
                            break;
                        case "skakac":
                            skor += skakacPozB[figura.GetPozicija().Y, figura.GetPozicija().X];
                            break;
                        case "pesak":
                            skor += pesakPozB[figura.GetPozicija().Y, figura.GetPozicija().X];
                            break;
                    }
                }
                else
                {
                    switch (figura.Vrsta)
                    {
                        case "kralj":
                            skor += kraljPozC[figura.GetPozicija().Y, figura.GetPozicija().X];
                            break;
                        case "dama":
                            skor += damaPozC[figura.GetPozicija().Y, figura.GetPozicija().X];
                            break;
                        case "top":
                            skor += topPozC[figura.GetPozicija().Y, figura.GetPozicija().X];
                            break;
                        case "lovac":
                            skor += lovacPozC[figura.GetPozicija().Y, figura.GetPozicija().X];
                            break;
                        case "skakac":
                            skor += skakacPozC[figura.GetPozicija().Y, figura.GetPozicija().X];
                            break;
                        case "pesak":
                            skor += pesakPozC[figura.GetPozicija().Y, figura.GetPozicija().X];
                            break;
                    }
                }

                if(figura.GetBoja() == Boja.bela)
                {
                    skorBeli = skorBeli + skor + figura.Vrednost;
                }
                else
                {
                    skorCrni = skorCrni + skor + figura.Vrednost;
                }
            }

            return skorBeli - skorCrni;
        }

        bool KrajIgre(ref List<Figura> figure, ref PoljeInfo[,] polja)
        {
            return (Mat(Boja.bela, ref figure, ref polja) || Mat(Boja.crna, ref figure, ref polja));
        }
        
        (long, Point, Point) GameMax(ref List<Figura> figure, ref PoljeInfo[,] polja, long beta, int depth)
        {
            if(depth == 0 || KrajIgre(ref figure, ref polja))
            {
                return (StanjeTable(ref figure, ref polja), new Point(-1, -1), new Point(-1, -1)); 
            }

            long alpha = long.MinValue;
            Point best_move = new Point(-1, -1);
            Point figpoz = new Point(-1, -1);

            //var onokad = KopiranjeListe(ref figure);
            for(int h = 0; h < figure.Count; h++)
            {
                var x = figure[h];
                if(x.GetBoja() == Boja.bela)
                {
                    List<Point> komedija = x.MoguciPotezi(figure, ref polja);
                    foreach (var potez in komedija)
                    {
                        int i = 0;
                        bool pojeo = false;
                        Point pomoc = x.GetPozicija();
                        foreach (var figura in figure)
                        {
                            if (figura.GetPozicija() == potez)
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
                        polja[x.GetPozicija().X, x.GetPozicija().Y].zauzeto = false;
                        polja[potez.X, potez.Y].zauzeto = true;
                        polja[potez.X, potez.Y].boja = x.GetBoja();
                        x.SetPozicija(potez);

                        long skor;
                        var kopija = KopiranjeListe(figure);
                        var kopolja = KopiranjePolja(ref polja);

                        Point xd;
                        Point cemuovo;
                        (skor, xd, cemuovo) = GameMin(ref kopija, ref kopolja, beta, depth - 1);
                        if (skor >= alpha)
                        {
                            alpha = skor;
                            best_move = potez;
                            figpoz = pomoc;
                        }
                        if (alpha >= beta)
                            goto labela;


                        polja[x.GetPozicija().X, x.GetPozicija().Y].zauzeto = false;
                        if (pojeo)
                        {
                            polja[x.GetPozicija().X, x.GetPozicija().Y].zauzeto = true;
                            polja[x.GetPozicija().X, x.GetPozicija().Y].boja = pojedena.GetBoja();
                        }
                        polja[pomoc.X, pomoc.Y].zauzeto = true;
                        polja[pomoc.X, pomoc.Y].boja = x.GetBoja();
                        x.SetPozicija(pomoc);
                        if (pojeo)
                        {
                            figure.Insert(i, pojedena);

                        }
                    }
                }
            }
            labela:
            return (alpha, best_move, figpoz);
        }

        (long, Point, Point) GameMin(ref List<Figura> figure, ref PoljeInfo[,] polja, long alpha, int depth)
        {
            if (depth == 0 || KrajIgre(ref figure, ref polja))
            {
                return (StanjeTable(ref figure, ref polja), new Point(-1, -1), new Point(-1, -1));
            }

            long beta = long.MaxValue;
            Point best_move = new Point(-1, -1);
            Point figpoz = new Point(-1, -1);

            //var onokad = KopiranjeListe(ref figure);
            for (int h = 0; h < figure.Count; h++)
            {
                var x = figure[h];
                if(x.GetBoja() == Boja.crna)
                {
                    List<Point> komedija = x.MoguciPotezi(figure, ref polja);
                    foreach (var potez in komedija)
                    {
                        //igranje poteza
                        int i = 0;
                        bool pojeo = false;
                        Point pomoc = x.GetPozicija();
                        foreach (var figura in figure)
                        {
                            if (figura.GetPozicija() == potez)
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
                        polja[x.GetPozicija().X, x.GetPozicija().Y].zauzeto = false;
                        polja[potez.X, potez.Y].zauzeto = true;
                        polja[potez.X, potez.Y].boja = x.GetBoja();
                        x.SetPozicija(potez);

                        //minimax algoritam
                        long skor;
                        var kopija = KopiranjeListe(figure);
                        var kopolja = KopiranjePolja(ref polja);

                        Point xd;
                        Point cemuovo;
                        (skor, xd, cemuovo) = GameMax(ref kopija, ref kopolja, beta, depth - 1);
                        if (skor <= beta)
                        {
                            beta = skor;
                            best_move = potez;
                            figpoz = pomoc;
                        }
                        if (beta <= alpha)
                            goto labela1;

                        //vracanje poteza
                        polja[x.GetPozicija().X, x.GetPozicija().Y].zauzeto = false;
                        if (pojeo)
                        {
                            polja[x.GetPozicija().X, x.GetPozicija().Y].zauzeto = true;
                            polja[x.GetPozicija().X, x.GetPozicija().Y].boja = pojedena.GetBoja();
                        }
                        polja[pomoc.X, pomoc.Y].zauzeto = true;
                        polja[pomoc.X, pomoc.Y].boja = x.GetBoja();
                        x.SetPozicija(pomoc);
                        if (pojeo)
                        {
                            figure.Insert(i, pojedena);

                        }
                    }
                }  
            }
            labela1:
            return (beta, best_move, figpoz);
        }

        public (long, Point, Point) Game(ref List<Figura> figure, ref PoljeInfo[,] polja, int depth, Boja boja)
        {
            var kopija = KopiranjeListe(figure);
            var kopolja = KopiranjePolja(ref polja);
            if (boja == Boja.bela)
                return GameMax(ref kopija, ref kopolja, long.MaxValue, depth);
            return GameMin(ref kopija, ref kopolja, long.MinValue, depth);
        }
    }
}
