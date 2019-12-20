using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace HSE.ComputerGraphics.Paint.UI
{
    public class LineGroup : ICanvasObject
    {
        public List<MyLine> Lines { get; set; }
        public LineGroup LastState { get; set; }

        public LineGroup(List<MyLine> lines)
        {
            Lines = new List<MyLine>();

            foreach (var line in lines)
            {
                Lines.Add(line);
                Lines.Last().Group = this;
            }
            //foreach (ICanvasObject canvasObject in lines)
            //{
            //    if (canvasObject is MyLine line)
            //    {
            //        Lines.Add(line);
            //    }
            //    else if (canvasObject is LineGroup group)
            //    {
            //        Lines.AddRange(group.Lines);
            //    }
            //}
        }

        public void Move(Vector delta)
        {
            Lines.ForEach(x => x.Move(delta));
        }

        public void Select()
        {
            Lines.ForEach(x => x.Select());
        }

        public void Deselect()
        {
            Lines.ForEach(x => x.Deselect());
        }

        public override int GetHashCode()
        {
            return Lines.Sum(x => x.GetHashCode());
        }
    }
}
