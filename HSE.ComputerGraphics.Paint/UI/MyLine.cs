using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HSE.ComputerGraphics.Paint.UI
{
    public class MyLine : ICanvasObject, ICloneable
    {
        public Line Line { get; set; }
        public LineGroup Group { get; set; }
        public bool IsSelected { get; set; }
        public bool Z { get; set; }

        public override int GetHashCode()
        {
            return this.Line.GetHashCode();
        }

        public void Move(Vector delta)
        {
            Line.X1 -= delta.X;
            Line.X2 -= delta.X;
            Line.Y1 -= delta.Y;
            Line.Y2 -= delta.Y;
        }

        public void Select()
        {
            Line.Stroke = Brushes.Red;
            IsSelected = true;
        }

        public void Deselect()
        {
            Line.Stroke = Brushes.Black;
            IsSelected = false;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public List<Line> GetLines()
        {
            return new List<Line> { Line };
        }
    }
}
