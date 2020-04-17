using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    class Pixel
    {
        int r;
        int g;
        int b;

        public Pixel(int r, int g, int b)
        {
            if (0 <= r && r <= 255)
            {
                this.r = r;
            }
            if (0 <= g && g <= 255)
            {
                this.g = g;
            }
            if (0 <= b && b <= 255)
            {
                this.b = b;
            }
            else
            {
                Console.Write("Le pixel n'est pas valide");
            }
        }

        public int R
        {
            get { return this.r; }
            set { this.r = value; }
        }

        public int G
        {
            get { return this.g; }
            set { this.g = value; }
        }

        public int B
        {
            get { return this.b; }
            set { this.b = value; }
        }

        public string toString()
        {
            string s = "";
            s = "(" + r + ";" + g + ";" + b + ")";
            return s;
        }
    }
}

