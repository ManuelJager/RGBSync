using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBSync
{
    public struct Color
    {
        public byte red { get; set; }
        public byte green { get; set; }
        public byte blue { get; set; }

        public Color(byte red, byte green, byte blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        public static Color Red => new Color(0xff, 0x00, 0x00);
        public static Color Green => new Color(0x00, 0xff, 0x00);
        public static Color Blue => new Color(0x00, 0x00, 0xff);
        public static Color White => new Color(0xff, 0xff, 0xff);
    }
}
