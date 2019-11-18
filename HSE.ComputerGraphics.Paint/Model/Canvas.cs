using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Caliburn.Micro;

namespace HSE.ComputerGraphics.Paint.Model
{
    public class Canvas
    {
        public BindableCollection<MyLine> Lines { get; set; } = new BindableCollection<MyLine>();

        //public BindableCollection<LineGroup> LineGroups { get; set; } = new BindableCollection<LineGroup>();

        public void AddNewRandomLine(double width, double height)
        {
            Random rand = new Random();
            int randX1 = rand.Next(0, (int)width);
            int randX2 = rand.Next(0, (int)width);
            int randY1 = rand.Next(0, (int)height);
            int randY2 = rand.Next(0, (int)height);

            Line newLine = new Line {X1 = randX1, Y1 = randY1, X2 = randX2, Y2 = randY2};

            Lines.Add(new MyLine(newLine));
        }
    }
}
