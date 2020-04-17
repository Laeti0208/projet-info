using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Media;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            MyImage Image = new MyImage("lena.bmp");
            Console.WriteLine(Image.Type);
            Console.WriteLine(Image.Taille);
            Console.WriteLine(Image.Offset);
            Console.WriteLine(Image.Largeur);
            Console.WriteLine(Image.Hauteur);
            Console.WriteLine(Image.Nbbitscouleur);
            //BytesImage();
            //Image.PassageàNégatif();
            //Image.Rotation("lena.bmp", "lenaTEST3.bmp");
            //Image.Miroir();
            //Image.Agrandir();
            //Image.Retrecir();
            Image.PassageDeCouleuràNoirBlanc();
            Image.From_Image_To_File("TEEEEEST.bmp");
            Process.Start("TEEEEEST.bmp");
            Console.ReadKey();
        }
    }
}
