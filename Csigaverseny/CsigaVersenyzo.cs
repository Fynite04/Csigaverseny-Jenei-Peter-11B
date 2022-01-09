using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Csigaverseny
{
    class CsigaVersenyzo
    {
        private Rectangle csigaObj;
        private string nev;
        private int osszPont = 0;
        private bool celbaErt = false;
        private Rectangle palya;
        private List<int> helyezesek = new List<int>();
        private Label versenyHelyezes;

        public CsigaVersenyzo(Rectangle csiga, Rectangle palya, Label versenyHelyezes, string nev)
        {
            this.csigaObj = csiga;
            this.nev = nev;
            this.palya = palya;
            this.versenyHelyezes = versenyHelyezes;

            this.helyezesek.Add(0); // #1
            this.helyezesek.Add(0); // #2
            this.helyezesek.Add(0); // #3
        }

        public int OsszesPont { get { return osszPont; } set { osszPont = value; } }
        public string Nev { get { return nev; } }
        public bool CelbaErt { get { return celbaErt; } set { celbaErt = value; } }
        public List<int> Helyezesek { get { return helyezesek; } set { helyezesek = value; } }
        public Rectangle CsigaObject { get { return csigaObj; } }
        public Rectangle Palya { get { return palya; } set { palya = value; } }
        public Label VersenyHelyezes { get { return versenyHelyezes; } }

        public static List<CsigaVersenyzo> BajnoksagAllas(List<CsigaVersenyzo> csigak)
        {
            var pontok = new List<int>();
            pontok.Add(csigak[0].OsszesPont);
            pontok.Add(csigak[1].OsszesPont);
            pontok.Add(csigak[2].OsszesPont);

            var allas = new List<CsigaVersenyzo>();

            allas.Add(csigak[pontok.IndexOf(pontok.Max())]); // #1
            allas.Add(csigak[pontok.IndexOf(pontok.Min())]); // #3
            allas.Insert(1, csigak.Find(x => !allas.Contains(x))); //#2

            return allas;
        }
    }
}
