using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vaja2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int stevilo_tock = 0; // koliko tock moremo zgenerirati
        int nacin_generiranja = 1; // kateri nacin bo 2 je enakomerno, 1 je normalno
        bool tocke_zgenerirane = false; // ce smo jih ze zgenerirali

        public class Tocka // moj class za tocko
        {
            public int x, y;
            public Tocka(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        List<Tocka> tocke = new List<Tocka>(); // kamor si shranim zgenerirane tocke

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int i = int.Parse(textBox1.Text);
            stevilo_tock = i;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Brush aBrush = (Brush)Brushes.Black; // za risanje tock
            Pen pen = new Pen(ForeColor); // za risanje daljic med tockama
            Graphics g = this.CreateGraphics();
        
            g.Clear(Color.White); // pocistim vse tocke in daljice ko je zacetek novih koordinat

            if (radioButton1.Checked == true)
            {
                nacin_generiranja = 1;
                //MessageBox.Show("Normalno");
            }
            else if (radioButton2.Checked == true)
            {
                nacin_generiranja = 2;
                //MessageBox.Show("Enakomerno");
            }
            else 
            {
                MessageBox.Show("Nisi izbral nacina zato je avtomatsko enakomerno!");
            }
            if (stevilo_tock == 0) {
                MessageBox.Show("Nisi vnesel stevila tock zato so avtomatsko 100");
                stevilo_tock = 100;
            }
            
            if (nacin_generiranja == 1) 
            {
                // NORMALNO
            }
            else if (nacin_generiranja == 2)
            {
                
                // ENAKOMETNO
                Random rnd = new Random();
                int x = 0, y = 0;
                for (int i = 0; i < stevilo_tock; i++)
                {
                    x = rnd.Next(50, 650); // random x koordinata
                    y = rnd.Next(50, 500); // random y koordinata
                    tocke.Add(new Tocka(x, y));
                    g.FillRectangle(aBrush, x, y, 2, 2); // narisemo tocko
                }
            }

            tocke_zgenerirane = true; // pomeni da smo zgenerirali tocke in lahko iscemo lupino
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tocke_zgenerirane)
            {
                Brush aBrush = (Brush)Brushes.Black; // za risanje tock
                Brush bBrush = (Brush)Brushes.Red; // za risanje tock
                Pen pen = new Pen(ForeColor); // za risanje daljic med tockama
                Graphics g = this.CreateGraphics();

                if (radioButton3.Checked == true)
                {
                    /// Jarvisov obhod
                    Tocka[] a = new Tocka[stevilo_tock];
                    for (int i = 0; i < stevilo_tock; i++) {
                        a[i] = new Tocka(tocke[i].x, tocke[i].y); // si shranimo tocke v novo polje
                    }

                    Jarvisov_obhod(a, stevilo_tock); // klicemo funkcijo Jarisov obhod
                    tocke.Clear();

                }
                else if (radioButton4.Checked == true)
                {
                    /// Hitra konveksna lupina
                    MessageBox.Show("Hitra konveksna lupina");
                    Tocka[] a = new Tocka[stevilo_tock];
                    for (int i = 0; i < stevilo_tock; i++)
                    {
                        a[i] = new Tocka(tocke[i].x, tocke[i].y); // si shranimo tocke v novo polje
                    }

                    Hitra_konveksna_lupina(a, stevilo_tock); // klicemo funkcijo
                    tocke.Clear();
                }
            }
            else 
            {
                MessageBox.Show("Tocke se niso zgenerirane");
            }
        }


        public static int orentacija(Tocka t1, Tocka t2, Tocka t3)
        {
            int val = (t2.y - t1.y) * (t3.x - t2.x) -
                    (t2.x - t1.x) * (t3.y - t2.y);

            if (val == 0) return 0; // kolinearna 
            if (val > 0) { return 1; }
            else { return 2; }
        }


        public void Jarvisov_obhod(Tocka[] tocke, int st)
        {
            // Shranimo lupino
            List<Tocka> lupina = new List<Tocka>();

            // Najdemo najbolj levo tocko
            int leva = 0;
            for (int i = 1; i < st; i++)
            {
                // gledamo da ima cim manjsi x
                if (tocke[i].x < tocke[leva].x)
                    leva = i;
            }
            int index = leva, nasledni;
            do
            {
                lupina.Add(tocke[index]); // si dodamo tocko index v lupino

                nasledni = (index + 1) % st; // izberemo naslednega in ce je index zadnji el gremo znova na 0

                for (int i = 0; i < st; i++)
                {
                    // gremo skozi vse tocke
                    if (orentacija(tocke[index], tocke[i], tocke[nasledni]) == 2)
                    {
                        nasledni = i; // si shranimo v nasledno
                    }
                }

                index = nasledni; // si shranimo naslednega v index da si dodamo v lupino

            } while (index != leva); // dokler ne pridemo do prve tocke
            
            // narisali bomo lupino
            Brush aBrush = (Brush)Brushes.Black; // za risanje tock
            Brush bBrush = (Brush)Brushes.Red; // za risanje tock
            Pen pen = new Pen(ForeColor); // za risanje daljic med tockama
            Graphics g = this.CreateGraphics();
            for (int i = 0; i < lupina.Count(); i++)
            {
                // Narisemo daljice med tockami
                if (i != lupina.Count() - 1) { g.DrawLine(pen, lupina[i].x, lupina[i].y, lupina[i + 1].x, lupina[i + 1].y); }
                else { g.DrawLine(pen, lupina[i].x, lupina[i].y, lupina[0].x, lupina[0].y); }
            }

        }
        
        List<Tocka> lupina_HKL = new List<Tocka>(); // kamor si shranim tocke lupine za hitro
        public void Hitra_konveksna_lupina(Tocka[] tocke, int st) {
            Brush aBrush = (Brush)Brushes.Black; // za risanje tock
            Brush bBrush = (Brush)Brushes.Red; // za risanje tock
            Pen pen = new Pen(ForeColor); // za risanje daljic med tockama
            Graphics g = this.CreateGraphics();

            /// najprej najdemo minx in maxx tocki
            /// to sta najbolj levo in desno
            int index_min=0, index_max=0;
            for (int i = 0; i < st; i++) {
                if (tocke[i].x < tocke[index_min].x) {
                    index_min = i;
                }
                if (tocke[i].x > tocke[index_max].x) {
                    index_max = i;
                }
            }
            //MessageBox.Show("Dobili min pa max tocki");
            g.FillRectangle(bBrush, tocke[index_min].x, tocke[index_min].y, 2, 2); // narisemo tocko
            g.FillRectangle(bBrush, tocke[index_max].x, tocke[index_max].y, 2, 2); // narisemo tocko
            g.DrawLine(pen, tocke[index_min].x, tocke[index_min].y, tocke[index_max].x, tocke[index_max].y);

            HKL(tocke[index_min], tocke[index_max], 1);
            HKL(tocke[index_min], tocke[index_max], -1);
            //MessageBox.Show("Koncali: " + lupina_HKL.Count());
            /*for (int i = 0; i < lupina_HKL.Count; i++) {
                MessageBox.Show("Pobarval");
                g.FillRectangle(bBrush, lupina_HKL[i].x, lupina_HKL[i].y, 2, 2); // narisemo tocko
            }*/
            //MessageBox.Show("Zaj bom narisal lik");
            //izris();
        }

        /// Vrne na kateri strani je tocka t3 glede na daljico t1 in t2
        /// ce je nad daljico je -1 drgace pa 1
        int Stran(Tocka t1, Tocka t2, Tocka t3)
        {
            int val = (t3.y - t1.y) * (t2.x - t1.x) - (t2.y - t1.y) * (t3.x - t1.x);

            if (val > 0) { return 1; }
            if (val < 0) { return -1; }
            return 0;
        }

        /// vrne razdaljo od tocke do daljice
        int razdalja(Tocka t1, Tocka t2, Tocka t3)
        {
            return Math.Abs((t3.y - t1.y) * (t2.x - t1.x) - (t2.y - t1.y) * (t3.x - t1.x));
        }

        void HKL(Tocka tocka_min, Tocka tocka_max, int stran) {

            Brush aBrush = (Brush)Brushes.Black; // za risanje tock
            Brush bBrush = (Brush)Brushes.Red; // za risanje tock
            Pen pen = new Pen(ForeColor); // za risanje daljic med tockama
            Graphics g = this.CreateGraphics();

            int index = -1;
            int max_razdalja = 0;
            // poiscemo najvecjo razdaljo od tocke do daljice, ki je na pravi strani daljice
            for (int i = 0; i < stevilo_tock; i++)
            {
                int temp = razdalja(tocka_min, tocka_max, tocke[i]);
                if (Stran(tocka_min, tocka_max, tocke[i]) == stran && temp > max_razdalja)
                {
                    index = i;
                    max_razdalja = temp;
                }
            }
            //MessageBox.Show("Max razdalja");
            //g.FillRectangle(bBrush, tocke[index].x, tocke[index].y, 2, 2); // narisemo tocko
            if (index > 0)
            {
                //MessageBox.Show("Max razdalja");
                g.DrawLine(pen, tocke[index].x, tocke[index].y, tocka_max.x, tocka_max.y);
                g.DrawLine(pen, tocke[index].x, tocke[index].y, tocka_min.x, tocka_min.y);
                // Create pen.
                //Brush cBrush = (Brush)Brushes.White; // za risanje tock
                /*Pen blackPen = new Pen(Color.Black, 3);

                // Create points that define polygon.
                Point point1 = new Point(tocka_max.x, tocka_max.y);
                Point point2 = new Point(tocka_min.x,tocka_min.y);
                Point point3 = new Point(tocke[index].x, tocke[index].y);
                Point[] curvePoints =
                         {
                             point1,
                             point2,
                             point3,
                        };

                // Draw polygon to screen.
                //g.DrawPolygon(blackPen, curvePoints);
                g.FillPolygon(aBrush, curvePoints);
                */
            }

            // ce ne najdemo dobene damo zadni dve tocki v lupino
            if (index == -1)
            {
                lupina_HKL.Add(tocka_min);
                lupina_HKL.Add(tocka_max);
                return;
            }

            // Rekurzivno klice sama sebe 
            HKL(tocke[index], tocka_min, -Stran(tocke[index], tocka_min, tocka_max));
            HKL(tocke[index], tocka_max, -Stran(tocke[index], tocka_max, tocka_min));

        }

        void izris() {
            Graphics g = this.CreateGraphics();
            Pen blackPen = new Pen(Color.Black, 3);
            Brush aBrush = (Brush)Brushes.Black; // za risanje tock

            // Create points that define polygon.

            Point[] curvePoints = new Point[lupina_HKL.Count()];
            for (int i = 0; i < lupina_HKL.Count(); i++) {
                curvePoints[i].X = lupina_HKL[i].x;
                curvePoints[i].Y = lupina_HKL[i].y;
            }
            // Draw polygon to screen.
            //g.DrawPolygon(blackPen, curvePoints);
            g.FillPolygon(aBrush, curvePoints);
        }

    }
}
