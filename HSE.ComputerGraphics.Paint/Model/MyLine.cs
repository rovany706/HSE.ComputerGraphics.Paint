using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HSE.ComputerGraphics.Paint.Model
{
    public class MyLine

    {
        public Line Line { get; set; }
        public bool IsPartOfGroup { get; set; }

        public MyLine(Line line)
        {
            this.Line = line;
            IsPartOfGroup = false;
        }
    }
}
