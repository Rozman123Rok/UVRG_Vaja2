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

        int stevilo_tock = 0;
        bool dobili_st_tock;
        int nacin_generiranja = 1; // kateri nacin bo 2 je enakomerno, 1 je normalno
        bool tocke_zgenerirane = false;

        public class Point
        {
            public int x, y;
            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        List<Point> tocke = new List<Point>();
        List<Point> lupina_tocke = new List<Point>();

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int i = int.Parse(textBox1.Text);
            //MessageBox.Show("Test: " + i);
            stevilo_tock = i;
            dobili_st_tock = true;
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
                MessageBox.Show("Normalno");
            }
            else if (radioButton2.Checked == true)
            {
                nacin_generiranja = 2;
                MessageBox.Show("Enakomerno");
            }
            else 
            {
                MessageBox.Show("Nisi izbral nacina zato je avtomatsko enakomerno!");
            }
            if (stevilo_tock == 0) {
                MessageBox.Show("Nisi vnesel stevila tock zato so avtomatsko 100");
                stevilo_tock = 100;
            }
            //MessageBox.Show("Stevilo tock : " + stevilo_tock);
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
                    x = rnd.Next(50, 650);
                    y = rnd.Next(50, 500);
                    tocke.Add(new Point(x, y));
                    g.FillRectangle(aBrush, x, y, 2, 2); // narisemo tocko
                }
                MessageBox.Show("Narisal vse tocke!");
               /* for (int i = 0; i < stevilo_tock; i++) {
                    MessageBox.Show("Koordinate" + i + ".tocke: x: " + tocke[i].x + " y: " + tocke[i].y);
                }*/
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

                //g.Clear(Color.White); // pocistim vse tocke in daljice ko je zacetek novih koordinat

                if (radioButton3.Checked == true)
                {
                    /// Jarvisov obhod
                    MessageBox.Show("Jarvisov obhod");

                    /// poisci najmanjso tocko
                    int min_x = 99999, stevec_min_x=0;
                    int index_tocke=0;
                    MessageBox.Show("Stevilo tock: " + stevilo_tock);
                    for (int i = 0; i < stevilo_tock; i++) { 
                        if(tocke[i].x < min_x) 
                        {
                            min_x = tocke[i].x;
                            // nasel tocko ki ima (trenutno) najnizjo x vrednost
                            if(stevec_min_x == 0) 
                            {
                                // prva tocka zato si jo lahko kr shranim
                                index_tocke = i;
                                stevec_min_x++;
                            }

                            if (tocke[i].y < tocke[index_tocke].y) {
                                // imamo tocko z novim najmanjsim y in je tot tocka ki je najmanjs trenutno
                                index_tocke = i;
                            }
                        }
                    }

                    MessageBox.Show("Pobarval bom min tocko: ");
                    g.FillRectangle(bBrush, tocke[index_tocke].x, tocke[index_tocke].y, 2, 2); // narisemo tocko
                    lupina_tocke.Add(tocke[index_tocke]);

                }
                else if (radioButton4.Checked == true)
                {
                    /// Hitra konveksna lupina
                    MessageBox.Show("Hitra konveksna lupina");
                }
            }
            else 
            {
                MessageBox.Show("Tocke se niso zgenerirane");
            }
        }
    }
}
