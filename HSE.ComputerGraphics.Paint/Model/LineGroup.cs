using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Caliburn.Micro;

namespace HSE.ComputerGraphics.Paint.Model
{
    public class LineGroup
    {
        public BindableCollection<Line> Lines { get; set; } = new BindableCollection<Line>();
    }
}
