using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace ConsoleApp1
{
    class MyImage
    {


        #region Champs
        string type;
        int taille;
        int offset;
        int largeur;
        int hauteur;
        int nbbitscouleur;
        Pixel[,] image;
        #endregion

        #region Constructeur 
        public MyImage(string file)
        {

            byte[] tabImage = File.ReadAllBytes(file);
            char a = Convert.ToChar(tabImage[0]);
            type += a;
            char b = Convert.ToChar(tabImage[1]);
            type += b;
            if (type != "BM")
            {
                Console.WriteLine("Le fichier n'est pas un bmp et ne peut pas être traité");
            }
            else
            {
                this.taille = tabImage[2] + (tabImage[3] * 256) + (tabImage[4] * 65536) + (tabImage[5] * 16777216);
                this.offset = tabImage[10] + (tabImage[11] * 256) + (tabImage[12] * 65536) + (tabImage[13] * 16777216);
                this.largeur = tabImage[18] + (tabImage[19] * 256) + (tabImage[20] * 65536) + (tabImage[21] * 16777216);
                this.hauteur = tabImage[22] + (tabImage[23] * 256) + (tabImage[24] * 65536) + (tabImage[25] * 16777216);
                this.nbbitscouleur = tabImage[28] + (tabImage[29] * 256);
                this.image = CréerMatricePixels(tabImage, hauteur, largeur);
            }


        }
        #endregion

        #region Propriétés
        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public int Taille
        {
            get { return this.taille; }
            set { this.taille = value; }
        }

        public int Offset
        {
            get { return this.offset; }
            set { this.offset = value; }
        }

        public int Largeur
        {
            get { return this.largeur; }
            set { this.largeur = value; }
        }

        public int Hauteur
        {
            get { return this.hauteur; }
            set { this.hauteur = value; }
        }
        public int Nbbitscouleur
        {
            get { return this.nbbitscouleur; }
            set { this.nbbitscouleur = value; }
        }

        public Pixel[,] Image
        {
            get { return this.image; }
            set { this.image = value; }
        }
        #endregion

        #region Méthodes
        public void From_Image_To_File(string newfile)
        {
            byte[] tab = new byte[taille];
            if (type == "BM")
            {
                byte[] typeLE = { 66, 77 };
                byte[] tailleLE = Int_To_Endian(taille);
                byte[] offsetLE = Int_To_Endian(offset);
                byte[] largeurLE = Int_To_Endian(largeur);
                byte[] hauteurLE = Int_To_Endian(hauteur);
                byte[] nbbitscouleurLE = Int_To_Endian(nbbitscouleur);

                typeLE.CopyTo(tab, 0);
                tailleLE.CopyTo(tab, 2);
                largeurLE.CopyTo(tab, 18);
                hauteurLE.CopyTo(tab, 22);
                offsetLE.CopyTo(tab, 10);
                nbbitscouleurLE.CopyTo(tab, 28);
                tab[26] = 1;                               //Nombre de plans
                tab[14] = 40;                              //Taille du header d'info
                int a = offset;
                for (int i = 0; i < hauteur; i++)
                {

                    for (int j = 0; j < largeur; j++)
                    {
                        tab[a] = Convert.ToByte(image[i, j].R);
                        tab[a + 1] = Convert.ToByte(image[i, j].G);
                        tab[a + 2] = Convert.ToByte(image[i, j].B);
                        a = a + 3;
                    }
                }

                File.WriteAllBytes(newfile, tab);

            }
            else
            {
                Console.WriteLine("Le fichier n'est pas du bon type");
            }
        }

        public Pixel[,] CréerMatricePixels(byte[] tabImage, int hauteur, int largeur)
        {
            Pixel[,] matricePixel = new Pixel[hauteur, largeur];
            int a = 54;
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    Pixel pixel = new Pixel(tabImage[a], tabImage[a + 1], tabImage[a + 2]);
                    matricePixel[i, j] = pixel;
                    a = a + 3;
                }
            }

            return matricePixel;

        }


        public int LittleEndian_To_Int(byte[] tabbyte, int indexdépart)
        {
            int index = 0;
            if (tabbyte.Length < 4)
            {
                byte[] tab = { 0, 0, 0, 0 };
                for (int i = 0; i < tabbyte.Length; i++)
                {
                    tab[i] = tabbyte[i];
                }

                index = BitConverter.ToInt32(tab, indexdépart);
                Console.WriteLine("int: " + index);
            }
            else
            {
                index = BitConverter.ToInt32(tabbyte, 0);
                Console.WriteLine("int:" + index);
            }
            return index;
        }

        public byte[] Int_To_Endian(int nb)
        {
            byte[] bytes = BitConverter.GetBytes(nb);
            Console.Write(nb + " en Little Endian : { ");
            foreach (int a in bytes)
            {
                Console.Write(a + " ");
            }
            Console.WriteLine("}");
            return bytes;
        }
        #endregion

        #region Traitement 


        public void PassageDeCouleuràGris()
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    int a = (image[i, j].R + image[i, j].G + image[i, j].B) / 3;
                    byte b = Convert.ToByte(a);
                    image[i, j].R = b;
                    image[i, j].G = b;
                    image[i, j].B = b;
                }
            }
        }
        public void PassageDeCouleuràNoirBlanc()
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    int a = (image[i, j].R + image[i, j].G + image[i, j].B) / 3;
                    if (a < 128)
                    {
                        image[i, j].R = 0;
                        image[i, j].G = 0;
                        image[i, j].B = 0;
                    }
                    else
                    {
                        image[i, j].R = 255;
                        image[i, j].G = 255;
                        image[i, j].B = 255;
                    }
                }
            }
        }
        public void PassageàNégatif()
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j].R = 255 - image[i, j].R;
                    image[i, j].G = 255 - image[i, j].G;
                    image[i, j].B = 255 - image[i, j].B;
                }
            }
            int taille1 = (largeur / 2) - 1;
        }
        public void Rotation(string myfile, string newfile)
        {
            Bitmap b = new Bitmap(myfile);
            b.RotateFlip(RotateFlipType.Rotate90FlipX);
            b.Save(newfile);
        }
        public void Miroir()
        {
            Pixel[,] nouvelle = new Pixel[hauteur, largeur];
            int taille1 = 0;
            if (largeur % 2 == 0)
            {
                taille1 = (largeur / 2) - 1;
            }
            else
            {
                taille1 = (largeur - 1) / 2;
            }

            for (int i = 0; i < nouvelle.GetLength(0); i++)
            {
                for (int j = 0; j < nouvelle.GetLength(1); j++)
                {
                    nouvelle[i, j] = image[i, j];
                }
            }
            for (int i = 0; i < nouvelle.GetLength(0); i++)
            {
                int n = 0;
                for (int j = 0; j <= taille1; j++)
                {
                    int index = nouvelle.GetLength(1) - 1 - n;
                    image[i, index] = nouvelle[i, j];
                    n++;
                }
            }
        }
        public void Retrecir()
        {
            Pixel[,] nouvelle = new Pixel[hauteur, largeur];
            for (int i = 0; i < nouvelle.GetLength(0); i++)//on fait la copie 
            {
                for (int j = 0; j < nouvelle.GetLength(1); j++)
                {
                    nouvelle[i, j] = image[i, j];
                }
            }
            image = new Pixel[nouvelle.GetLength(0) / 2, nouvelle.GetLength(1) / 2];

            for (int i = 0; i < nouvelle.GetLength(0) / 2; i = i + 2)
            {
                for (int j = 0; j < nouvelle.GetLength(1) / 2; j++)
                {
                    image[i, j] = nouvelle[i, j + 2];
                }
            }
        }
        public void Agrandir()
        {
            Pixel[,] nouvelle = new Pixel[hauteur, largeur];
            for (int i = 0; i < nouvelle.GetLength(0); i++)//on fait la copie 
            {
                for (int j = 0; j < nouvelle.GetLength(1); j++)
                {
                    nouvelle[i, j] = image[i, j];
                }
            }
            image = new Pixel[hauteur * 2, largeur * 2];
            int l = 0;
            int c = 0;
            for (int i = 0; i < nouvelle.GetLength(0); i++)
            {
                for (int j = 0; j < nouvelle.GetLength(1); j++)
                {
                    image[l, c] = nouvelle[i, j];
                    image[l, c + 1] = nouvelle[i, j];
                    image[l + 1, c] = nouvelle[i, j];
                    image[l + 1, c + 1] = nouvelle[i, j];
                    c = c + 2;
                }
                l = l + 2;
                c = 0;
            }

        }
        #endregion
    }
}