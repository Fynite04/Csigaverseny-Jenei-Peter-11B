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
        List<Rectangle> helyek = new List<Rectangle>();
        List<CsigaVersenyzo> versenyzok = new List<CsigaVersenyzo>();
        Brush palyaSzin;

        public MainWindow()
        {
            InitializeComponent();
            CsigaInicializalo();
            TablazatMegjelenito(false);

            idozito.Interval = TimeSpan.FromSeconds(0.01);
            idozito.Tick += new EventHandler(RaceEventHandler);

            palyaSzin = palya1.Fill;
        }

        // Adott időintervallumonként végrehajtja
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
                            helyek.Add(versenyzok[i].CsigaObject);
                            versenyzok[i].Palya.Fill = Brushes.Gold;
                            versenyzok[i].OsszesPont += 3;
                            versenyzok[i].Helyezesek[0]++;
                            versenyzok[i].VersenyHelyezes.Content = "1";
                            versenyzok[i].CelbaErt = true;
                        }
                        // #2
                        else if(helyek.Count == 1)
                        {
                            helyek.Add(versenyzok[i].CsigaObject);
                            versenyzok[i].Palya.Fill = Brushes.Silver;
                            versenyzok[i].OsszesPont += 2;
                            versenyzok[i].Helyezesek[1]++;
                            versenyzok[i].VersenyHelyezes.Content = "2";
                            versenyzok[i].CelbaErt = true;
                        }
                        // #3
                        else if (helyek.Count == 2)
                        {
                            helyek.Add(versenyzok[i].CsigaObject);
                            versenyzok[i].Palya.Fill = Brushes.SandyBrown;
                            versenyzok[i].CelbaErt = true;
                            versenyzok[i].OsszesPont += 1;
                            versenyzok[i].Helyezesek[2]++;
                            versenyzok[i].VersenyHelyezes.Content = "3";
                            idozito.Stop();

                            ujBajnoksagBtn.IsEnabled = true;
                            ujFutamBtn.IsEnabled = true;

                            MutasdBajnoksagAllast();
                            TablazatMegjelenito(true);
                        }
                    }
                }
            }
        }

        // Példányosítja a versenyzőket
        private void CsigaInicializalo()
        {
            versenyzok.Add(new CsigaVersenyzo(csiga1, palya1, helyezes1, "csiga1"));
            versenyzok.Add(new CsigaVersenyzo(csiga2, palya2, helyezes2, "csiga2"));
            versenyzok.Add(new CsigaVersenyzo(csiga3, palya3, helyezes3, "csiga3"));
        }

        // "Start" gomb megnyomásakor
        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            idozito.Start();
            startBtn.IsEnabled = false;
            ujBajnoksagBtn.IsEnabled = false;
            ujFutamBtn.IsEnabled = false;
        }

        // Megjeleníti a táblázatban a megfelelő értékeket
        private void MutasdBajnoksagAllast()
        {
            var allas = CsigaVersenyzo.BajnoksagAllas(versenyzok);

            // Név
            allasNev_1.Content = allas[0].Nev;
            allasNev_2.Content = allas[1].Nev;
            allasNev_3.Content = allas[2].Nev;

            // 1. hely érmei
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

        // "Új futam" gomb megnyomásakor
        private void ujFutamBtn_Click(object sender, RoutedEventArgs e)
        {
            PalyaVisszaallitasa();
        }

        // "Új bajnokság" gomb menyomásakor
        private void ujBajnoksagBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                $"Hely          Név             1.          2.          3.          Pont\n " +
                $"1.            {allasNev_1.Content}          {allas1_1.Content}           {allas2_1.Content}" +
                $"           {allas3_1.Content}            {allasPont_1.Content}\n " +
                $"2.            {allasNev_2.Content}          {allas1_2.Content}           {allas2_2.Content}" +
                $"           {allas3_2.Content}            {allasPont_2.Content}\n " +
                $"3.            {allasNev_3.Content}          {allas1_3.Content}           {allas2_3.Content}" +
                $"           {allas3_3.Content}            {allasPont_3.Content}" +
                $"", "A bajnokság végeredménye");

            PalyaVisszaallitasa();
            versenyzok.Clear();
            CsigaInicializalo();
            TablazatMegjelenito(false);
        }

        // Verseny visszaállítása alap állásba
        private void PalyaVisszaallitasa()
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

        // Megjeleníti vagy eltünteti a táblázatot a paraméter alapján
        private void TablazatMegjelenito(bool megjelenitse)
        {
            List<Label> elemek = new List<Label>();

            // Fejléc
            elemek.Add(header_Hely);
            elemek.Add(header_Nev);
            elemek.Add(header_1);
            elemek.Add(header_2);
            elemek.Add(header_3);
            elemek.Add(header_Pont);

            // 1. sor
            elemek.Add(vHeader_1);
            elemek.Add(allasNev_1);
            elemek.Add(allas1_1);
            elemek.Add(allas2_1);
            elemek.Add(allas3_1);
            elemek.Add(allasPont_1);

            // 2. sor
            elemek.Add(vHeader_2);
            elemek.Add(allasNev_2);
            elemek.Add(allas1_2);
            elemek.Add(allas2_2);
            elemek.Add(allas3_2);
            elemek.Add(allasPont_2);

            // 3. sor
            elemek.Add(vHeader_3);
            elemek.Add(allasNev_3);
            elemek.Add(allas1_3);
            elemek.Add(allas2_3);
            elemek.Add(allas3_3);
            elemek.Add(allasPont_3);

            foreach (Label e in elemek)
            {
                if (megjelenitse)
                    e.Visibility = Visibility.Visible;
                else
                    e.Visibility = Visibility.Hidden;
            }
        }
    }
}
