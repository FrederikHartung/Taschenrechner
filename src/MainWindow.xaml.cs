using System;
using System.Collections.Generic;
using System.IO;
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

namespace Taschenrechner_Praktikum_Tag_2
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        //Hauptvariablen
        string zahl1_vorDemKomma = "";
        string zahl1_nachDemKomma = "";
        string zahl2_vorDemKomma = "";
        string zahl2_nachDemKomma = "";
        string matOperator = "";
        double summe;
        bool gleichZeichen = false;
        bool kommaGesetzt = false;
        int nachkommaStellen;

        //Konfigurationsdatei
        // Pfade relativ zur .exe (bin\Debug) -> Ordner "config" im Projektverzeichnis
        static string speicherort_config = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\config"));
        static string speicherort_konfig = System.IO.Path.Combine(speicherort_config, "Taschenrechner Konfiguration.txt");
        static string speicherort_debug = System.IO.Path.Combine(speicherort_config, "Taschenrechner Debug.txt");
        string[] zeilen = System.IO.File.ReadAllLines(speicherort_konfig);
        string textBoxAusgabe;

        public MainWindow()

        {
            InitializeComponent();
            LeseKonfiguration();
        }

        //      -- Methoden --

        private void Btn_number_Click(object sender, RoutedEventArgs e)
        //addiert die Klicks als String zu jeweils 2 Teilstrings (vor/nach dem Komma) Zahl1 oder Zahl2 je nachdem, ob ein Operator aktiv ist
        {
            string zahl_input = (string)((Button)sender).Content;
            //zahl_input speichert die gedrückte Zahl als String zur Weiterverarbeitung in den späteren Rechenoperationen

            if (gleichZeichen == true && matOperator == "")
            //Wenn der Benutzer nach einem Gleichzeichen eine neue Rechnung startet und das Zwischenergebnis verfällt
            {
                GründlichAufräumen();
                gleichZeichen = false;
            }


            if (matOperator == "" && kommaGesetzt == false)
            //Abfrage aller vier möglichen Kombinationen
            {

                if(zahl1_vorDemKomma == "0")
                {
                    zahl1_vorDemKomma = zahl_input;
                    AktualisiereFenster();
                }
                else if (zahl1_vorDemKomma != "0")
                {
                    zahl1_vorDemKomma += zahl_input;
                    AktualisiereFenster();
                }


                

            }
            else if (matOperator == "" && kommaGesetzt == true)
            {
                zahl1_nachDemKomma += zahl_input;
                AktualisiereFenster();
            }
            else if (matOperator != "" && kommaGesetzt == false)
            {
                zahl2_vorDemKomma += zahl_input;
                AktualisiereFenster();
            }
            else if (matOperator != "" && kommaGesetzt == true)
            {
                zahl2_nachDemKomma += zahl_input;
                AktualisiereFenster();
            }
        }

        private void Btn_operator_Click(object sender, RoutedEventArgs e)
        // verarbeitet und speichert den geklickten Operator 
        // sollte zum zweiten Mal ein Operator gedrückt werden, wird der Befehl zum Aufsummieren gegeben
        {
            SchreibeHauptVariablenWerte();
            if (matOperator == "")
            {
                matOperator = (string)((Button)sender).Content;
                kommaGesetzt = false;
                AktualisiereFenster();
            }
            else
            {
                SummiereAuf();
                matOperator = (string)((Button)sender).Content;
                AktualisiereFenster();
            }
        }

        private double StringZuDouble1(string zahl1vor, string zahl1nach) //awip
        //Verrechnet die Beiden Teilstrings der Zahl 1 zu einen double und gibt ihn zurück
        {
            //Vorbereitung zum parsen Zahl1
            if (zahl1_vorDemKomma == "")
            {
                zahl1_vorDemKomma = "0";
            }
            if (zahl1_nachDemKomma == "")
            {
                zahl1_nachDemKomma = "0";
            }

            string zahl1_gesammt = zahl1vor + "," + zahl1nach;
            return double.Parse(zahl1_gesammt);
        }

        private double StringZuDouble2(string zahl2vor, string zahl2nach)
        //Verrechnet die Beiden Teilstrings der Zahl 2 zu einen double und gibt ihn zurück
        {
            //Vorbereitung zum parsen Zahl2
            if (zahl2_vorDemKomma == "")
            {
                zahl2_vorDemKomma = "0";
            }
            if (zahl2_nachDemKomma == "")
            {
                zahl2_nachDemKomma = "0";
            }

            string zahl2_gesammt = zahl2vor + "," + zahl2nach;
            return double.Parse(zahl2_gesammt);
        }

        private void SummiereAuf()
        // mathematische Methode, die Zahl 1 und Zahl 2 je nach Operator verrechnet
        {
            if ((zahl1_vorDemKomma != "" || zahl1_nachDemKomma != "") && (zahl2_vorDemKomma != "" || zahl2_nachDemKomma != ""))
            {
                double zahl1_double = StringZuDouble1(zahl1_vorDemKomma, zahl1_nachDemKomma);
                double zahl2_double = StringZuDouble2(zahl2_vorDemKomma, zahl2_nachDemKomma);

                if (matOperator == "+")       // plus 
                {
                    summe = zahl1_double + zahl2_double;
                    SchreibeInDebugDatei("SummiereAuf: zahl1_double: " + zahl1_double + " zahl2_double: " + zahl2_double + " Summe: " + summe);
                    Aufräumen();
                }
                else if (matOperator == "-")       // minus
                {
                    summe = zahl1_double - zahl2_double;
                    SchreibeInDebugDatei("SummiereAuf: zahl1_double: " + zahl1_double + " zahl2_double: " + zahl2_double + " Summe: " + summe);
                    Aufräumen();
                }
                else if (matOperator == "*")       // mal
                {
                    summe = zahl1_double * zahl2_double;
                    SchreibeInDebugDatei("SummiereAuf: zahl1_double: " + zahl1_double + " zahl2_double: " + zahl2_double + " Summe: " + summe);
                    Aufräumen();
                }
                else if (matOperator == "/")       // geteilt
                {
                    if (zahl2_double == 0)
                    {
                        MessageBox.Show("Division mit 0 ist mathematisch nicht erlaubt!");
                        GründlichAufräumen();
                    }
                    else if (zahl1_double == 0)
                    {
                        summe = zahl1_double / zahl2_double;
                        SchreibeInDebugDatei("SummiereAuf: zahl1_double: " + zahl1_double + " zahl2_double: " + zahl2_double + " Summe: " + summe);
                        Aufräumen();
                    }
                    else
                    {
                        summe = zahl1_double / zahl2_double;
                        SchreibeInDebugDatei("SummiereAuf: zahl1_double: " + zahl1_double + " zahl2_double: " + zahl2_double + " Summe: " + summe);
                        Aufräumen();
                    }
                }
            }
            else
            {
                SchreibeInDebugDatei("Es wurde versucht zu summieren mit einer leeren Zahl!");
            }

        }

        private void Aufräumen()       // räumt nach dem Aufsummieren auf
        {
            if ((zahl1_vorDemKomma != "" || zahl1_nachDemKomma != "") && (zahl2_vorDemKomma != "" || zahl2_nachDemKomma != ""))
            {
                //erstellen des Verlaufs
                string summe_string = summe.ToString();
                if (summe_string.IndexOf(",") != -1 )
                {
                    if (summe_string.Substring(summe_string.IndexOf(",")).Length > nachkommaStellen)
                    {
                        summe = double.Parse(summe_string.Substring( 0 , summe_string.IndexOf(",") + nachkommaStellen + 1));
                    }
                }

                textBox_history.Text = zahl1_vorDemKomma + "," + zahl1_nachDemKomma + " " + matOperator + " " + zahl2_vorDemKomma + "," + zahl2_nachDemKomma + " = " + summe;
                SchreibeInDebugDatei("Berechne: " + zahl1_vorDemKomma + "," + zahl1_nachDemKomma + " " + matOperator + " " + zahl2_vorDemKomma + "," + zahl2_nachDemKomma + " = " + summe);

                if ((((summe.ToString()).IndexOf(",")) != -1))
                {
                    zahl1_vorDemKomma = (summe.ToString()).Remove((summe.ToString()).IndexOf(","));
                    zahl1_nachDemKomma = (summe.ToString()).Substring(summe.ToString().IndexOf(",") + 1);
                }
                else
                {
                    zahl1_vorDemKomma = summe.ToString();
                    zahl1_nachDemKomma = "";
                }

                SchreibeInDebugDatei("---> indexOf , : " + (summe.ToString().IndexOf(",")) + " zahl1_vorDemKomma: " + zahl1_vorDemKomma + " zahl1_nachDemKomma: " + zahl1_nachDemKomma);

                //Zurücksetzen der Hauptvariablen
                zahl2_vorDemKomma = "";
                zahl2_nachDemKomma = "";
                matOperator = "";
                summe = 0;
                kommaGesetzt = false;
            }
            AktualisiereFenster();
        }

        private void GründlichAufräumen()
        //setzt alle Haupt Variablen zurück
        {
            zahl1_vorDemKomma = "";
            zahl1_nachDemKomma = "";
            zahl2_vorDemKomma = "";
            zahl2_nachDemKomma = "";
            matOperator = "";
            summe = 0;
            kommaGesetzt = false;
            gleichZeichen = false;
            textBox_history.Text = "";

            AktualisiereFenster();
        }
        //setzt alle wichtigen Variablen zurück

        //      --      Sonderfälle     --

        private void btn_sonderfall_gleich_Click(object sender, RoutedEventArgs e)
        //gibt die Summe von Zahl 1 und Zahl 2 aus und setzt einen boolen. Abhängig von der nächsten Benutzereingabe wird dieser boolean in anderen Methoden verwendet
        {
            SchreibeHauptVariablenWerte();

            if (zahl1_vorDemKomma != "" || zahl1_nachDemKomma != "" && zahl2_vorDemKomma != "" || zahl2_nachDemKomma != "")
            {
                gleichZeichen = true;
                SummiereAuf();
            }
        }


        private void btn_sonderfall_plusminus_Click(object sender, RoutedEventArgs e)
        //ändert das Vorzeichen der aktuellen Zahl
        //wenn schon ein Minus da ist, wird dieses entfernt
        {
            if (matOperator == "" && zahl1_vorDemKomma.IndexOf("-") == -1)
            {
                zahl1_vorDemKomma = "-" + zahl1_vorDemKomma;
                AktualisiereFenster();
            }
            else if (matOperator != "" && zahl2_vorDemKomma.IndexOf("-") == -1)
            {
                zahl2_vorDemKomma = "-" + zahl2_vorDemKomma;
                AktualisiereFenster();
            }
            else if (matOperator == "" && zahl1_vorDemKomma.IndexOf("-") != -1)
            {
                zahl1_vorDemKomma = zahl1_vorDemKomma.Replace("-", "");
                AktualisiereFenster();
            }
            else if (matOperator != "" && zahl2_vorDemKomma.IndexOf("-") != -1)
            {
                zahl2_vorDemKomma = zahl2_vorDemKomma.Replace("-", "");
                AktualisiereFenster();
            }
        }

        private void btn_sonderfall_komma_Click(object sender, RoutedEventArgs e)
        //fügt ein Komma zur aktuellen Zahl hinzu
        {
            kommaGesetzt = true;    //fehler
            if (matOperator == "" && zahl1_vorDemKomma.IndexOf(",") == -1)
            {
               // zahl1_vorDemKomma = zahl1_vorDemKomma + ",";
                AktualisiereFenster();
            }
            else if (matOperator != "" && zahl2_vorDemKomma.IndexOf(",") == -1)
            {
              //  zahl2_vorDemKomma = zahl2_vorDemKomma + ",";
                AktualisiereFenster();
            }
        }


        //      -----------------------------------------       Methoden zum löschen von Inhalt        -------------------------
        private void btn_del_backspace_Click(object sender, RoutedEventArgs e)
        //löscht die letzte eingegebene Zahl
        {
                if (zahl2_vorDemKomma != "" || zahl2_nachDemKomma != "")
                //Zahl2
                {
                    if (matOperator != "" && zahl2_nachDemKomma != "")
                    {
                        zahl2_nachDemKomma = zahl2_nachDemKomma.Substring(0, zahl2_nachDemKomma.Length - 1);
                    }
                    else if (matOperator != "" && zahl2_vorDemKomma != "")
                    {
                        gleichZeichen = false;
                        zahl2_vorDemKomma = zahl2_vorDemKomma.Substring(0, zahl2_vorDemKomma.Length - 1);
                    }
                    else
                    {
                        SchreibeInDebugDatei("Fehler bei der btn_del_backspace_Click Methode");
                    }
                }

                else if (zahl2_vorDemKomma == "" && zahl2_nachDemKomma == "")
                //Zahl1
                {
                    if (matOperator != "")
                    {
                        matOperator = "";
                    }
                    else if (zahl1_vorDemKomma != "" || zahl1_nachDemKomma != "")
                    {
                        if (matOperator == "" && zahl1_nachDemKomma != "")
                        {
                            zahl1_nachDemKomma = zahl1_nachDemKomma.Substring(0, zahl1_nachDemKomma.Length - 1);
                        }
                        else if (matOperator == "" && zahl1_vorDemKomma != "")
                        {
                            kommaGesetzt = false;
                            zahl1_vorDemKomma = zahl1_vorDemKomma.Substring(0, zahl1_vorDemKomma.Length - 1);
                        }
                        else
                        {
                            SchreibeInDebugDatei("Fehler bei der btn_del_backspace_Click Methode");
                        }
                    }
            }


            SchreibeInDebugDatei("zahl1_vorDemKomma: " + zahl1_vorDemKomma + " zahl1_nachDemKomma: " + zahl1_nachDemKomma);
            AktualisiereFenster();
        }

        private void btn_del_C_Click(object sender, RoutedEventArgs e)
        // gibt den Befehl, alle Haupt Variablen zurückzusetzen
        {
            GründlichAufräumen();
        }

        private void btn_del_CE_Click(object sender, RoutedEventArgs e)
        //löscht die aktuelle Eingabe  
        {
            if (matOperator == "")      //test
            {
                zahl1_vorDemKomma = "";
                zahl1_nachDemKomma = "";
                kommaGesetzt = false;
                gleichZeichen = false;
                AktualisiereFenster();
            }
            else
            {
                zahl2_vorDemKomma = "";
                zahl2_nachDemKomma = "";
                kommaGesetzt = false;
                AktualisiereFenster();
            }
        }

        private string Übergebe_zahl1_vorDemKomma(string zahl1_vorDemKomma)
        {
            if(zahl1_vorDemKomma == "")
            {
                zahl1_vorDemKomma = "0";
            }
            return zahl1_vorDemKomma;
        }

        private string Übergebe_zahl1_nachDemKomma(string zahl1_nachDemKomma)
        {
            if (zahl1_nachDemKomma == "")
            {
                zahl1_nachDemKomma = "0";
            }
            return zahl1_nachDemKomma;
        }

        private string Übergebe_zahl2_vorDemKomma(string zahl2_vorDemKomma)
        {
            if (zahl2_vorDemKomma == "")
            {
                zahl2_vorDemKomma = "0";
            }
            return zahl2_vorDemKomma;
        }

        private string Übergebe_zahl2_nachDemKomma(string zahl2_nachDemKomma)
        {
            if (zahl2_nachDemKomma == "")
            {
                zahl2_nachDemKomma = "0";
            }
            return zahl2_nachDemKomma;
        }

        private void AktualisiereFenster()
        //aktualisiert das "Display" Fenster automatisch nach einer Änderung
        {
            debug_Textbox.Text = "textBoxAusgabe: " + textBoxAusgabe + "\n zahl1_vorDemKomma: " + zahl1_vorDemKomma + "\n zahl1_nachDemKomma: " + zahl1_nachDemKomma + "\n  Operator: " + matOperator + "\n zahl2_vorDemKomma: " + zahl2_vorDemKomma + "\n zahl2_nachDemKomma: " + zahl2_nachDemKomma + "\n" + "Gleichzeichen: " + gleichZeichen + "\n kommaGesetzt: " + kommaGesetzt + "\n Summe: " + summe;
            SchreibeInDebugDatei(debug_Textbox.Text);
            if (textBoxAusgabe == "zahlen")
            {

                if (matOperator == "")
                    //-> Zahl 1
                {
                    //ohne Komma
                    if (zahl1_vorDemKomma == "" && zahl1_nachDemKomma == "" && kommaGesetzt == false)
                    {
                        textBox_display.Text = "0";
                        SchreibeInDebugDatei("ohne 1");
                    }
                    else if (zahl1_vorDemKomma != "" && zahl1_nachDemKomma == "" && kommaGesetzt == false)
                    {
                        textBox_display.Text = zahl1_vorDemKomma;
                        SchreibeInDebugDatei("ohne 2");

                    }
                    else if (zahl1_vorDemKomma == "" && zahl1_nachDemKomma != "" && kommaGesetzt == false)
                    {
                        textBox_display.Text = "0," + zahl1_nachDemKomma;
                        SchreibeInDebugDatei("ohne 3");

                    }
                    else if (zahl1_vorDemKomma != "" && zahl1_nachDemKomma != "" && kommaGesetzt == false)
                    {
                        textBox_display.Text = zahl1_vorDemKomma + "," + zahl1_nachDemKomma;
                        SchreibeInDebugDatei("ohne 4");

                    }
                    else if (kommaGesetzt == false)
                    {
                        SchreibeInDebugDatei("Fehler in AktualisiereFenster - matOperator == '', ohne Komma ");
                    }

                    //mit Komma
                    else if (zahl1_vorDemKomma == "" && zahl1_nachDemKomma == "" && kommaGesetzt == true)
                    {
                        textBox_display.Text = "0";

                    }
                    else if (zahl1_vorDemKomma != "" && zahl1_nachDemKomma == "" && kommaGesetzt == true)
                    {
                        textBox_display.Text = zahl1_vorDemKomma + ",";

                    }
                    else if (zahl1_vorDemKomma == "" && zahl1_nachDemKomma != "" && kommaGesetzt == true)
                    {
                        textBox_display.Text = "0," + zahl1_nachDemKomma;

                    }
                    else if (zahl1_vorDemKomma != "" && zahl1_nachDemKomma != "" && kommaGesetzt == true)
                    {
                        textBox_display.Text = zahl1_vorDemKomma + "," + zahl1_nachDemKomma;

                    }
                    else if (kommaGesetzt == true)
                    {
                        SchreibeInDebugDatei("Fehler in AktualisiereFenster - matOperator == '', mit Komma ");
                    }
                }
                else if (kommaGesetzt == false && matOperator != "")
                //matOperator != "" -> Zahl 2
                //kommaGesetzt == false
                {
                    if (zahl2_vorDemKomma == "" && zahl2_nachDemKomma == "") //AWIP
                    {
                        textBox_display.Text = StringZuDouble1(zahl1_vorDemKomma, zahl1_nachDemKomma) + " " + matOperator + " ";
                    }
                    else if (zahl2_vorDemKomma != "" && zahl2_nachDemKomma == "")
                    {
                        textBox_display.Text = StringZuDouble1(zahl1_vorDemKomma, zahl1_nachDemKomma) + " " + matOperator + " " + zahl2_vorDemKomma;
                    }
                    else if (zahl2_vorDemKomma == "" && zahl2_nachDemKomma != "")
                    {
                        textBox_display.Text = StringZuDouble1(zahl1_vorDemKomma, zahl1_nachDemKomma) + " " + matOperator + " 0" + zahl2_nachDemKomma;
                    }
                    else if (zahl2_vorDemKomma != "" && zahl2_nachDemKomma != "")
                    {
                        textBox_display.Text = StringZuDouble1(zahl1_vorDemKomma, zahl1_nachDemKomma) + " " + matOperator + " " + zahl2_vorDemKomma + zahl2_nachDemKomma;
                    }
                    else if (kommaGesetzt == false)
                    {
                        SchreibeInDebugDatei("Fehler in AktualisiereFenster - matOperator != '' ");
                    }
                }

                else if (kommaGesetzt == true && matOperator != "")
                {
                    if (zahl2_vorDemKomma == "" && zahl2_nachDemKomma == "") //AWIP
                    {
                        textBox_display.Text = StringZuDouble1(zahl1_vorDemKomma, zahl1_nachDemKomma) + " " + matOperator + " ";
                    }
                    else if (zahl2_vorDemKomma != "" && zahl2_nachDemKomma == "")
                    {
                        textBox_display.Text = StringZuDouble1(zahl1_vorDemKomma, zahl1_nachDemKomma) + " " + matOperator + " " + zahl2_vorDemKomma + ",";
                    }
                    else if (zahl2_vorDemKomma == "" && zahl2_nachDemKomma != "")
                    {
                        textBox_display.Text = StringZuDouble1(zahl1_vorDemKomma, zahl1_nachDemKomma) + " " + matOperator + " 0," + zahl2_nachDemKomma;
                    }
                    else if (zahl2_vorDemKomma != "" && zahl2_nachDemKomma != "")
                    {
                        textBox_display.Text = StringZuDouble1(zahl1_vorDemKomma, zahl1_nachDemKomma) + " " + matOperator + " " + zahl2_vorDemKomma + "," + zahl2_nachDemKomma;
                    }
                    else
                    {
                        SchreibeInDebugDatei("Fehler in AktualisiereFenster - matOperator != '' ");
                    }
                }
            }
            
            //------------------------------------- text ----------------------------------
            else if (textBoxAusgabe == "text")
            {

                if (kommaGesetzt == false && matOperator == "")
                //-> Zahl 1
                {
                    if (zahl1_vorDemKomma == "" && zahl1_nachDemKomma == "")
                    {
                        textBox_display.Text = "Null";
                    }
                    else if (zahl1_vorDemKomma != "" && zahl1_nachDemKomma == "")
                    {
                        textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma));
                    }
                    else if (zahl1_vorDemKomma == "" && zahl1_nachDemKomma != "")
                    {
                        textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + "," + KonvZahlenNachDemKomma(Übergebe_zahl1_nachDemKomma(zahl1_nachDemKomma));
                    }
                    else if (zahl1_vorDemKomma != "" && zahl1_nachDemKomma != "")
                    {
                        textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + "," + KonvZahlenNachDemKomma(Übergebe_zahl1_nachDemKomma(zahl1_nachDemKomma));
                    }
                }
                else if (kommaGesetzt == true && matOperator == "")
                {
                    if (zahl1_vorDemKomma == "" && zahl1_nachDemKomma == "")
                    {
                        textBox_display.Text = "Null,";
                    }
                    else if (zahl1_vorDemKomma != "" && zahl1_nachDemKomma == "")
                    {
                        textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + ",";
                    }
                    else
                    {
                        textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + "," + KonvZahlenNachDemKomma(Übergebe_zahl1_nachDemKomma(zahl1_nachDemKomma));
                    }
                }
                else if (kommaGesetzt == false && matOperator != "")
                //--zahl 2 --
                {
                    if (zahl1_nachDemKomma == "")
                    //kein Komma, keine zahl1 nach Komma
                    {
                        if (zahl2_vorDemKomma == "" && zahl2_nachDemKomma == "")
                        //es gibt gar keine Zahl
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + " " + matOperator + " ";
                        }

                        else if (zahl2_vorDemKomma != "" && zahl2_nachDemKomma == "")
                        //es gibt eine Zahl vor dem Komma
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + " " + matOperator + " " + KonvZahlenVorDemKomma(zahl2_vorDemKomma);
                        }
                        else if (zahl2_vorDemKomma == "" && zahl2_nachDemKomma != "")
                        //es gibt eine Zahl nach dem Komma
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + " " + matOperator + " " + KonvZahlenVorDemKomma("0") + "," + KonvZahlenNachDemKomma((zahl2_nachDemKomma));
                        }
                        else if (zahl2_vorDemKomma != "" && zahl2_nachDemKomma != "")
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + " " + matOperator + " " + KonvZahlenVorDemKomma(zahl2_vorDemKomma) + "," + KonvZahlenNachDemKomma((zahl2_nachDemKomma));
                        }
                    }
                    else if (zahl1_nachDemKomma != "")
                    //kein Komma gesetzt, zahl1_nachDemKomma ist nicht leer
                    {
                        if (zahl2_vorDemKomma == "" && zahl2_nachDemKomma == "")
                        //es gibt gar keine Zahl und zahl1_nachDemKomma != ""
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + "," + KonvZahlenNachDemKomma(zahl1_nachDemKomma) + " " + matOperator + " ";
                        }

                        else if (zahl2_vorDemKomma != "" && zahl2_nachDemKomma == "")
                        //es gibt eine Zahl vor dem Komma
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + "," + KonvZahlenNachDemKomma(zahl1_nachDemKomma) + " " + matOperator + " " + KonvZahlenVorDemKomma(Übergebe_zahl2_vorDemKomma(zahl2_vorDemKomma));
                        }
                        else if (zahl2_vorDemKomma == "" && zahl2_nachDemKomma != "")
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + "," + KonvZahlenNachDemKomma(zahl1_nachDemKomma) + " " + matOperator + " " + KonvZahlenVorDemKomma("0") + KonvZahlenNachDemKomma(zahl2_nachDemKomma);
                        }
                        else if (zahl2_vorDemKomma != "" && zahl2_nachDemKomma != "")
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + "," + KonvZahlenNachDemKomma(zahl1_nachDemKomma) + " " + matOperator + " " + KonvZahlenVorDemKomma(zahl2_vorDemKomma) + "," + KonvZahlenNachDemKomma(zahl2_nachDemKomma);
                        }
                    }
                }


                else if (kommaGesetzt == true)
                //wenn ein Komma gesetzt wurde
                {
                    if (zahl1_nachDemKomma == "")
                    // Komma, keine zahl1 nach Komma
                    {
                        if (zahl2_vorDemKomma == "" && zahl2_nachDemKomma == "")
                        //es gibt gar keine Zahl
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + " " + matOperator + " " + KonvZahlenVorDemKomma("0") + ",";
                        }

                        else if (zahl2_vorDemKomma != "" && zahl2_nachDemKomma == "")
                        //es gibt eine Zahl vor dem Komma
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + " " + matOperator + " " + KonvZahlenVorDemKomma(zahl2_vorDemKomma) + ",";
                        }
                        else if (zahl2_vorDemKomma == "" && zahl2_nachDemKomma == "")
                        //es gibt eine Zahl nach dem Komma
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + " " + matOperator + " " + KonvZahlenVorDemKomma("0") + "," + KonvZahlenNachDemKomma((zahl2_nachDemKomma));
                        }
                        else if (zahl2_vorDemKomma != "" && zahl2_nachDemKomma != "")
                        //es gibt beide Zahlen
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + " " + matOperator + " " + KonvZahlenVorDemKomma(zahl2_vorDemKomma) + "," + KonvZahlenNachDemKomma((zahl2_nachDemKomma));
                        }
                    }
                    else if (zahl1_nachDemKomma != "")
                    {
                        if (zahl2_vorDemKomma == "" && zahl2_nachDemKomma == "")
                        //es gibt gar keine Zahl und zahl1_nachDemKomma != ""
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + "," + KonvZahlenNachDemKomma(zahl1_nachDemKomma) + " " + matOperator + " " + KonvZahlenNachDemKomma("0") + ",";
                        }

                        else if (zahl2_vorDemKomma != "" && zahl2_nachDemKomma == "")
                        //es gibt eine Zahl vor dem Komma
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + "," + KonvZahlenNachDemKomma(zahl1_nachDemKomma) + " " + matOperator + " " + KonvZahlenVorDemKomma(Übergebe_zahl2_vorDemKomma(zahl2_vorDemKomma)) + ",";
                        }
                        else if (zahl2_vorDemKomma == "" && zahl2_nachDemKomma != "")
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + "," + KonvZahlenNachDemKomma(zahl1_nachDemKomma) + " " + matOperator + " " + KonvZahlenVorDemKomma("0") + "," + KonvZahlenNachDemKomma(zahl2_nachDemKomma);
                        }
                        else if (zahl2_vorDemKomma != "" && zahl2_nachDemKomma != "")
                        //es gibt beide Zahlen
                        {
                            textBox_display.Text = KonvZahlenVorDemKomma(Übergebe_zahl1_vorDemKomma(zahl1_vorDemKomma)) + "," + KonvZahlenNachDemKomma(zahl1_nachDemKomma) + " " + matOperator + " " + KonvZahlenVorDemKomma(zahl2_vorDemKomma) + "," + KonvZahlenNachDemKomma((zahl2_nachDemKomma));
                        }
                    }
                }

                }
            }

        private string KonvZahlenNachDemKomma(string zahl1)
        {
            var einezln = new[] { "null", "eins", "zwei", "drei", "vier", "fünf", "sechs", "sieben", "acht", "neun" };
            string buchstabe = " ";

            for (int i = 0; i < zahl1.Length; i++)
            {

                buchstabe += einezln[int.Parse(zahl1.Substring(i, 1))];
                SchreibeInDebugDatei("ZahlenNachDemKomma()--> Index: " + i + " Wert: " + einezln[int.Parse(zahl1.Substring(i, 1))]);
            }
            return buchstabe;
        }

        private string KonvZahlenVorDemKomma(string zahl1)
        {
            var einzeln = new[] { "", "ein", "zwei", "drei", "vier", "fünf", "sechs", "sieben", "acht", "neun", "zehn", "elf", "zwölf" , "dreizehn", "vierzehn", "fünfzehn", "sechszehn", "siebzehn", "achtzehn","neunzehn","zwanzig"};
            var tausender = new[] {"", "tausend", "millionen", "milliarden", "billionen", "billiarden", "trilliaden"};
            string buchstabe = "";
            string ausgabe = "";
            bool minus = false;
            int counter_durchläufe = 0;

            //für Minus Zahlen
            if (zahl1.IndexOf("-") != -1)
            {
                zahl1 = zahl1.Replace("-", "");
                minus = true;
            }

            for (int durchläufeÜbrig = (((zahl1.Length - 1) / 3) + 1); durchläufeÜbrig > 0; durchläufeÜbrig--)
                //Verarbeitung von rechts nach links
            {
                if (durchläufeÜbrig != 1)
                {
                    string zuVerarbeiten = zahl1.Substring(zahl1.Length - (3 * (counter_durchläufe + 1)), 3);

                    if (int.Parse(zuVerarbeiten.Substring(0, 1)) > 0)
                    {
                        buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(0, 1))] + "hundert"; //Hunderter
                    }

                    //zehner
                    if (zuVerarbeiten.Substring(0, 3) == "000")
                    {
                        buchstabe += "";
                    }
                    else if (zuVerarbeiten.Substring(1, 2) == "00")
                    {
                        buchstabe += "";
                    }
                    else if (int.Parse(zuVerarbeiten.Substring(1, 1)) == 0 && int.Parse(zuVerarbeiten.Substring(0, 1)) != 2 && int.Parse(zuVerarbeiten.Substring(0, 1)) != 7)
                    {
                        buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(0, 1))] + "zig";
                    }
                    else if (int.Parse(zuVerarbeiten.Substring(1, 2)) <= 20 && int.Parse(zuVerarbeiten.Substring(1, 2)) > 0)
                    {
                        buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(1, 2))];
                    }
                    else if (int.Parse(zuVerarbeiten.Substring(1, 2)) > 20 && int.Parse(zuVerarbeiten.Substring(1, 2)) < 30)
                    {
                        buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(2, 1))] + "undzwanzig";
                    }
                    else if ((int.Parse(zuVerarbeiten.Substring(1, 2)) >= 70) && (int.Parse(zuVerarbeiten.Substring(1, 2)) <= 80))
                    {
                        buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(2, 1))] + "undsiebzig";
                    }
                    else if (int.Parse(zuVerarbeiten.Substring(1, 2)) >= 30)
                    {
                        buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(2, 1))] + "und" + einzeln[int.Parse(zuVerarbeiten.Substring(1, 1))] + "zig";
                    }

                    if(buchstabe != "")
                    {
                        buchstabe += tausender[counter_durchläufe]; //Tausender Potenzen
                    }

                }
                else if (durchläufeÜbrig == 1)
                {
                    int rest = zahl1.Length - counter_durchläufe * 3;
                    string zuVerarbeiten = zahl1.Substring(0, rest);

                    if (rest == 3)
                    {
                        if (int.Parse(zuVerarbeiten.Substring(0, 1)) > 0)
                        {
                            buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(0, 1))] + "hundert"; //Hunderter
                        }

                        //zehner
                        if (zuVerarbeiten.Substring(0, 3) == "000")
                        {
                              buchstabe += "";
                        }
                        else if (zuVerarbeiten.Substring(1, 2) == "00")
                        {
                            buchstabe += "";
                        }
                        else if (int.Parse(zuVerarbeiten.Substring(1, 2)) <= 20 && int.Parse(zuVerarbeiten.Substring(1, 2)) > 0)
                        {
                            buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(1, 2))];
                        }
                        else if (int.Parse(zuVerarbeiten.Substring(1, 2)) > 20 && int.Parse(zuVerarbeiten.Substring(1, 2)) < 30)
                        {
                            buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(2, 1))] + "undzwanzig";
                        }
                        else if ((int.Parse(zuVerarbeiten.Substring(1, 2)) >= 70) && (int.Parse(zuVerarbeiten.Substring(1, 2)) <= 80))
                        {
                            buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(2, 1))] + "undsiebzig";
                        }
                        else if (int.Parse(zuVerarbeiten.Substring(1, 2)) >= 30)
                        {
                            buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(2, 1))] + "und" + einzeln[int.Parse(zuVerarbeiten.Substring(1, 1))] + "zig";
                        }
                    }
                    else if (rest == 2)
                    {
                        //zehner
                        if(int.Parse(zuVerarbeiten.Substring(1, 1)) == 0 && int.Parse(zuVerarbeiten.Substring(0, 1)) != 2 && int.Parse(zuVerarbeiten.Substring(0, 1)) != 7)
                        {
                            buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(0, 1))] + "zig";
                        }
                        else if (int.Parse(zuVerarbeiten.Substring(0, 2)) <= 20 && int.Parse(zuVerarbeiten.Substring(0, 2)) > 0)
                        {
                            buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(0, 2))];
                        }
                        else if (int.Parse(zuVerarbeiten.Substring(0, 2)) > 20 && int.Parse(zuVerarbeiten.Substring(0, 2)) < 30)
                        {
                            buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(1, 1))] + "undzwanzig";
                        }
                        else if ((int.Parse(zuVerarbeiten.Substring(0, 2)) >= 70) && (int.Parse(zuVerarbeiten.Substring(0, 2)) <= 80))
                        {
                            buchstabe += einzeln[int.Parse(zuVerarbeiten.Substring(1, 1))] + "undsiebzig";
                        }
                        else if (int.Parse(zuVerarbeiten.Substring(0, 2)) >= 30)
                        {
                            buchstabe = einzeln[int.Parse(zuVerarbeiten.Substring(1, 1))] + "und" + einzeln[int.Parse(zuVerarbeiten.Substring(0, 1))] + "zig" + buchstabe;
                        }
                    }
                    else if (rest == 1)
                    {
                        if (int.Parse(zuVerarbeiten.Substring(0, 1)) == 0 && counter_durchläufe == 0)
                        {
                            buchstabe = "Null";
                            minus = false;
                        }
                        else if(int.Parse(zuVerarbeiten.Substring(0, 1)) == 1 && counter_durchläufe > 1 && int.Parse(zuVerarbeiten.Substring(0, 1)) > 0)
                        {
                            buchstabe = "eine" + buchstabe;
                        }
                        else if (int.Parse(zuVerarbeiten.Substring(0, 1)) == 1 && counter_durchläufe == 0)
                        {
                            buchstabe = "eins" + buchstabe;
                        }
                        else
                        {
                            buchstabe = einzeln[int.Parse(zuVerarbeiten.Substring(0, 1))] + buchstabe;
                        }
                    }

                    buchstabe = buchstabe + tausender[counter_durchläufe]; //Tausender Potenzen

                }
                counter_durchläufe++;
                if(buchstabe != "")
                {
                    string ersterBuchstabe = (buchstabe.Substring(0, 1)).ToUpper();
                    buchstabe = buchstabe.Remove(0, 1);
                    ausgabe = ersterBuchstabe + buchstabe + " " + ausgabe;
                }

                buchstabe = "";
            }

            if (minus == true)
            {
                ausgabe = " minus " + ausgabe;
            }

            return ausgabe;
        }

        //    -------------------------------------------  Debug und Konfiguration laden  -------------------------------------------

        private void SchreibeHauptVariablenWerte()
        //Methoden zum schreiben von allen relevanten Variablen in einer Debug Datei
        {
            string status = "zahl1_vorDemKomma: " + zahl1_vorDemKomma + " zahl1_nachDemKomma: " + zahl1_nachDemKomma + " Operator: " + matOperator + " zahl2_vorDemKomma: " + zahl2_vorDemKomma + " zahl2_nachDemKomma: " + zahl2_nachDemKomma;
            string status1 = "Gleichzeichen: " + gleichZeichen + " kommaGesetzt: " + kommaGesetzt + " Summe: " + summe;
            SchreibeInDebugDatei(status + "\n" + status1);
        }

        private void SchreibeInDebugDatei(string inhalt)      //Methoden zum schreiben von Inhalt(String) in einer Debug Datei
        {
            using (StreamWriter datei = new StreamWriter(speicherort_debug, true))
            {
                datei.WriteLine(inhalt);
            }
        }

        public void LeseKonfiguration()  //Liest beim Programmstart die Konfigurationsdatei aus
        {
            File.WriteAllText(speicherort_debug, "Debug Datei erfolgreich erstellt! \n");
            SchreibeInDebugDatei("--------------------------------------------------------------------");
            SchreibeInDebugDatei("Konfigurationsdatei mit folgenden Inhalt geladen:");
            foreach (string line in zeilen)
            {
                SchreibeInDebugDatei(line);
            }

            //---------------------------------------

            //      soll Schrift oder Zahlen ausgegeben werden?

            if (zeilen[0] == "textBoxAusgabe = text")
            {
                textBoxAusgabe = "text";
                SchreibeInDebugDatei("Setze 'textBoxAusgabe = text' (Konfigurationsdatei)");
            }
            else
            {
                textBoxAusgabe = "zahlen";
                SchreibeInDebugDatei("Setze 'textBoxAusgabe = zahlen' (default)");
            }


            //---------------------------------------

            //      wie viele Nachkommastellen?

            if (zeilen[2].IndexOf("nachKommaStellen") != 1)
            {
                string nachKommaStellen_string = zeilen[1].Substring(zeilen[1].IndexOf(" = ") + 3);
                int nachKommaStellen_int = int.Parse(nachKommaStellen_string);
                SchreibeInDebugDatei("Setze 'nachKommaStellen = " + nachKommaStellen_int + "' (Konfigurationsdatei)");
                nachkommaStellen = nachKommaStellen_int;
            }
            else
            {
                nachkommaStellen = 4;
                SchreibeInDebugDatei("Setze 'nachkommaStellen = 4' (default)");
            }

            //Schriftgröße des Displays 

            if (zeilen[2].IndexOf("fontSize_display = ") != -1)
            {
                string fontSize_Verlauf_string = zeilen[2].Substring(zeilen[2].IndexOf(" = ") + 3);
                double fontSize_display = double.Parse(fontSize_Verlauf_string);
                SchreibeInDebugDatei("Setze 'textBox_display.FontSize = " + fontSize_display + "' (Konfigurationsdatei)");
                textBox_display.SetValue(TextElement.FontSizeProperty, fontSize_display);
            }
            else
            {
                textBox_display.FontSize = 30;
                SchreibeInDebugDatei("Setze 'textBox_display.FontSize = 30' (default)");
                this.textBox_display.FontSize = 30;
            }
            //---------------------------------------------

            //Schriftgröße des Verlaufs
            if (zeilen[3].IndexOf("fontSize_verlauf = ") != -1)
            {
                string fontSizeSize_string = zeilen[3].Substring(zeilen[3].IndexOf(" = ") + 3);
                double fontSize_verlauf = double.Parse(fontSizeSize_string);
                SchreibeInDebugDatei("Setze 'textBox_display.FontSize = " + fontSize_verlauf + "' (Konfigurationsdatei)");
                textBox_history.SetValue(TextElement.FontSizeProperty, fontSize_verlauf);
            }

            else
            {
                textBox_history.FontSize = 20;      //default
                SchreibeInDebugDatei("Setze die Größe des Verlaufs auf '20' (default)");
            }
            AktualisiereFenster();

            SchreibeInDebugDatei("-------------------------------------------------------------------- \n \n ---------------Benutzereingaben-------------");
        }
    }
}
        