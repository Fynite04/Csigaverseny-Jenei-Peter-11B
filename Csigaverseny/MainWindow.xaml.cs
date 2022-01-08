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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer idozito = new DispatcherTimer();
        List<int> osszPontok = new List<int>();
        List<bool> celbaErt = new List<bool>();
        List<Rectangle> csigak = new List<Rectangle>();
        List<Rectangle> palyak = new List<Rectangle>();
        List<Rectangle> helyek = new List<Rectangle>();
        List<Label> helyezesek = new List<Label>();

        public MainWindow()
        {
            InitializeComponent();
            KezdoInicializalas();

            idozito.Interval = TimeSpan.FromSeconds(0.01);
            idozito.Tick += new EventHandler(RaceEventHandler);
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

            //helyezesek[sebessegek.IndexOf(sebessegek.Max())] = 1;
            //helyezesek[sebessegek.IndexOf(sebessegek.Min())] = 3;
            //int second = helyezesek.FindIndex(x => x != sebessegek.Max() && x != sebessegek.Min());
            //helyezesek[second] = 2;

            //this.osszPontok[sebessegek.IndexOf(sebessegek.Max())] = 3;
            //this.osszPontok[sebessegek.IndexOf(sebessegek.Min())] = 1;
            //int second = osszPontok.FindIndex(x => x != sebessegek.Max() && x != sebessegek.Min());
            //this.osszPontok[second] = 2;


            // Csigák mozgatása a sebességek lista alapján
            for (int i = 0; i < csigak.Count; i++)
            {
                // Ha még nem ért célba...
                if (!celbaErt[i])
                {
                    // ...mozgassa jobbra az adott random értékkel
                    double leftMargin = csigak[i].Margin.Left + Convert.ToDouble(sebessegek[i]);
                    double topMargin = csigak[i].Margin.Top;
                    csigak[i].Margin = new Thickness(leftMargin, topMargin, 0, 0);

                    // Ha elérte a célt
                    if (csigak[i].Margin.Left >= 800)
                    {
                        if (helyek.Count == 0)
                        {
                            helyek.Add(csigak[i]);
                            palyak[i].Fill = Brushes.Gold;
                            osszPontok[i] += 3;
                            helyezesek[i].Content = "1";
                            celbaErt[i] = true;
                        }
                        else if(helyek.Count == 1)
                        {
                            helyek.Add(csigak[i]);
                            palyak[i].Fill = Brushes.Silver;
                            osszPontok[i] += 2;
                            helyezesek[i].Content = "2";
                            celbaErt[i] = true;
                        }
                        else if (helyek.Count == 2)
                        {
                            helyek.Add(csigak[i]);
                            palyak[i].Fill = Brushes.SandyBrown;
                            celbaErt[i] = true;
                            osszPontok[i] += 1;
                            helyezesek[i].Content = "3";
                            idozito.Stop();

                            ujBajnoksagBtn.IsEnabled = true;
                            ujFutamBtn.IsEnabled = true;
                        }
                    }

                }
            }

        }

        private void KezdoInicializalas()
        {
            // Összes pontok
            osszPontok.Add(0); // csiga1
            osszPontok.Add(0); // csiga2
            osszPontok.Add(0); // csiga3

            helyezesek.Add(helyezes1);
            helyezesek.Add(helyezes2);
            helyezesek.Add(helyezes3);

            // Célba értek e
            celbaErt.Add(false);
            celbaErt.Add(false);
            celbaErt.Add(false);

            // Csigák lista
            csigak.Add(csiga1);
            csigak.Add(csiga2);
            csigak.Add(csiga3);

            // Pályák listája
            palyak.Add(palya1);
            palyak.Add(palya2);
            palyak.Add(palya3);
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            idozito.Start();
            startBtn.IsEnabled = false;
            ujBajnoksagBtn.IsEnabled = false;
            ujFutamBtn.IsEnabled = false;
        }
    }
}
