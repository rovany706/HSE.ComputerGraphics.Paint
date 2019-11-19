using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using Caliburn.Micro;
using HSE.ComputerGraphics.Paint.Model;
using HSE.ComputerGraphics.Paint.Views;

namespace HSE.ComputerGraphics.Paint.ViewModels
{
    public class AppViewModel : Conductor<object>
    {
        public BindableCollection<LineViewModel> Lines { get; set; } = new BindableCollection<LineViewModel>();

        public double Width { get; set; }

        public double Height { get; set; }

        public int PanelX { get; set; }

        public int PanelY { get; set; }

        public void DrawNewLine()
        {
            Lines.Add(LineViewModel.CreateLineViewModel(Width, Height));
        }
        public void Click(Line line, object view, MouseEventArgs e)
        {
            //Point currentMousePosition = e.GetPosition(view.MainCanvas);
            MessageBox.Show($"{line.X1}");
        }
    }
}