using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Csigaverseny
{
    public partial class MainWindow : Window
    {
        DispatcherTimer idozito = new DispatcherTimer();
        List<Rectangle> csigak = new List<Rectangle>();
        List<Rectangle> helyek = new List<Rectangle>();
        List<CsigaVersenyzo> versenyzok = new List<CsigaVersenyzo>();

        Brush palyaSzin;

        public MainWindow()
        {
            InitializeComponent();
            KezdoInicializalas();

            idozito.Interval = TimeSpan.FromSeconds(0.01);
            idozito.Tick += new EventHandler(RaceEventHandler);

            palyaSzin = palya1.Fill;
        }

        private void RaceEventHandler(object sender, EventArgs e)
        {
            // Random sebességek
            var rnd = new Random();
            List<int> sebessegek = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                sebessegek.Add(rnd.Next(1, 5));
            }

            // Csigák mozgatása a sebességek lista alapján
            for (int i = 0; i < versenyzok.Count; i++)
            {
                // Ha még nem ért célba...
                if (!versenyzok[i].CelbaErt)
                {
                    // ...mozgassa jobbra az adott random értékkel
                    double leftMargin = versenyzok[i].CsigaObject.Margin.Left + Convert.ToDouble(sebessegek[i]);
                    double topMargin = versenyzok[i].CsigaObject.Margin.Top;
                    versenyzok[i].CsigaObject.Margin = new Thickness(leftMargin, topMargin, 0, 0);

                    // Ha elérte a célt
                    if (versenyzok[i].CsigaObject.Margin.Left >= 800)
                    {
                        // #1
                        if (helyek.Count == 0)
                        {
                            helyek.Add(csigak[i]);
                            versenyzok[i].Palya.Fill = Brushes.Gold;
                            versenyzok[i].OsszesPont += 3;
                            versenyzok[i].Helyezesek[0]++;
                            versenyzok[i].VersenyHelyezes.Content = "1";
                            versenyzok[i].CelbaErt = true;
                        }
                        // #2
                        else if(helyek.Count == 1)
                        {
                            helyek.Add(csigak[i]);
                            versenyzok[i].Palya.Fill = Brushes.Silver;
                            versenyzok[i].OsszesPont += 2;
                            versenyzok[i].Helyezesek[1]++;
                            versenyzok[i].VersenyHelyezes.Content = "2";
                            versenyzok[i].CelbaErt = true;
                        }
                        // #3
                        else if (helyek.Count == 2)
                        {
                            helyek.Add(csigak[i]);
                            versenyzok[i].Palya.Fill = Brushes.SandyBrown;
                            versenyzok[i].CelbaErt = true;
                            versenyzok[i].OsszesPont += 1;
                            versenyzok[i].Helyezesek[2]++;
                            versenyzok[i].VersenyHelyezes.Content = "3";
                            idozito.Stop();

                            ujBajnoksagBtn.IsEnabled = true;
                            ujFutamBtn.IsEnabled = true;

                            MutasdBajnoksagAllast();
                        }
                    }

                }
            }
        }

        private void KezdoInicializalas()
        {
            // Csiga versenyzők
            versenyzok.Add(new CsigaVersenyzo(csiga1, palya1, helyezes1, "csiga1"));
            versenyzok.Add(new CsigaVersenyzo(csiga2, palya2, helyezes2, "csiga2"));
            versenyzok.Add(new CsigaVersenyzo(csiga3, palya3, helyezes3, "csiga3"));

            // Csigák lista
            csigak.Add(csiga1);
            csigak.Add(csiga2);
            csigak.Add(csiga3);
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            idozito.Start();
            startBtn.IsEnabled = false;
            ujBajnoksagBtn.IsEnabled = false;
            ujFutamBtn.IsEnabled = false;
        }

        private void MutasdBajnoksagAllast()
        {
            var allas = CsigaVersenyzo.BajnoksagAllas(versenyzok);

            // Név
            allasNev_1.Content = allas[0].Nev;
            allasNev_2.Content = allas[1].Nev;
            allasNev_3.Content = allas[2].Nev;

            // Nyertes érmei
            allas1_1.Content = allas[0].Helyezesek[0];
            allas2_1.Content = allas[0].Helyezesek[1];
            allas3_1.Content = allas[0].Helyezesek[2];

            // 2. hely érmei
            allas1_2.Content = allas[1].Helyezesek[0];
            allas2_2.Content = allas[1].Helyezesek[1];
            allas3_2.Content = allas[1].Helyezesek[2];

            // 3. hely érmei
            allas3_1.Content = allas[2].Helyezesek[0];
            allas3_2.Content = allas[2].Helyezesek[1];
            allas3_3.Content = allas[2].Helyezesek[2];

            // Pont
            allasPont_1.Content = $"{allas[0].OsszesPont} p";
            allasPont_2.Content = $"{allas[1].OsszesPont} p";
            allasPont_3.Content = $"{allas[2].OsszesPont} p";
        }

        private void ujFutamBtn_Click(object sender, RoutedEventArgs e)
        {
            // Gombok engedélyezése / letiltása
            startBtn.IsEnabled = true;
            ujFutamBtn.IsEnabled = false;
            ujBajnoksagBtn.IsEnabled = true;

            // Csigák start pozícióba állása
            csiga1.Margin = new Thickness(10, 52, 0, 0);
            csiga2.Margin = new Thickness(10, 237, 0, 0);
            csiga3.Margin = new Thickness(10, 422, 0, 0);

            // Pályák alapszínűre állítása
            palya1.Fill = palyaSzin;
            palya2.Fill = palyaSzin;
            palya3.Fill = palyaSzin;

            // Múlt verseny helyezéseinek törlése
            helyezes1.Content = "";
            helyezes2.Content = "";
            helyezes3.Content = "";

            // Nem értek célba
            versenyzok[0].CelbaErt = false;
            versenyzok[1].CelbaErt = false;
            versenyzok[2].CelbaErt = false;

            helyek.Clear();
        }
    }
}
