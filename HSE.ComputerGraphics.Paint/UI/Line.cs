using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace HSE.ComputerGraphics.Paint.UI
{
    public struct LineConstants
    {
        public double A { get; }
        public double B { get; }
        public double C { get; }

        public LineConstants(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }

        public override string ToString()
        {
            return $"{A:#;-#}x{B:+0;-#}y{C:+0;-#}=0";
        }
    }
    public static class LineExtension
    {
        public static LineConstants GetLineConstants(this Line line)
        {
            double A = line.Y2 - line.Y1;
            double B = -(line.X2 - line.X1);
            double C = (line.Y2 - line.Y1) * (-line.X1) - (line.X2 - line.X1) * (-line.Y1);
            return new LineConstants(A, B, C);
        }
    }
}
