using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HSE.ComputerGraphics.Paint.UI;

namespace HSE.ComputerGraphics.Paint
{
    public partial class MainWindow : Window
    {
        private Shape currentSelection;
        private Point previousMousePosition;
        private bool isMousePressed;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAddLine_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Add(GetRandomLine());
        }

        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            if (canvas == null)
                return;

            HitTestResult hitTestResult = VisualTreeHelper.HitTest(canvas, e.GetPosition(canvas));

            if (currentSelection != null)
            {
                //Deselect old element
                currentSelection.Stroke = Brushes.Black;
                currentSelection = null;
                lbEquation.Text = String.Empty;
            }
            if (hitTestResult.VisualHit is Shape)
            {
                //Select new element
                currentSelection = hitTestResult.VisualHit as Shape;
                currentSelection.Stroke = Brushes.Red;

                lbEquation.Text = $"Уравнение: {((Line)currentSelection).GetLineConstants()}";
            }

            //Save mouse position
            previousMousePosition = e.GetPosition(canvas);
            isMousePressed = true;
        }

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            if (canvas == null)
                return;

            Point currentMousePosition = e.GetPosition(canvas);

            Point cartesianPosition = ConvertToCartesianCoords(MainCanvas, currentMousePosition);
            lbMousePosition.Text = $"X: {cartesianPosition.X} Y:{cartesianPosition.Y}";

            if (currentSelection != null)
            {
                Line line = currentSelection as Line;

                lbEquation.Text = $"Уравнение: {line.GetLineConstants()}";

                float radius = 10;
                bool isMouseNearBegin = Math.Pow(line.X1 - previousMousePosition.X, 2) + Math.Pow(line.Y1 - previousMousePosition.Y, 2) < Math.Pow(radius, 2);
                bool isMouseNearEnd = Math.Pow(line.X2 - previousMousePosition.X, 2) + Math.Pow(line.Y2 - previousMousePosition.Y, 2) < Math.Pow(radius, 2);

                if (isMouseNearBegin || isMouseNearEnd)
                    line.Cursor = Cursors.SizeNWSE;
                else
                    line.Cursor = Cursors.SizeAll;

                if (isMousePressed)
                {
                    if (isMouseNearBegin)
                    {
                        line.X1 = currentMousePosition.X;
                        line.Y1 = currentMousePosition.Y;
                    }
                    else if (isMouseNearEnd)
                    {
                        line.X2 = currentMousePosition.X;
                        line.Y2 = currentMousePosition.Y;
                    }
                    else
                    {
                        Vector delta = previousMousePosition - currentMousePosition;
                        line.X1 -= delta.X;
                        line.X2 -= delta.X;
                        line.Y1 -= delta.Y;
                        line.Y2 -= delta.Y;
                    }
                }
            }
            previousMousePosition = currentMousePosition;
        }

        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMousePressed = false;
        }

        private void btnRemoveLine_Click(object sender, RoutedEventArgs e)
        {
            if (currentSelection == null)
                return;

            MainCanvas.Children.Remove(currentSelection);
            currentSelection = null;
            lbEquation.Text = "";
        }

        private Line GetRandomLine()
        {
            Random rand = new Random();
            int randX1 = rand.Next(0, (int)Math.Round(MainCanvas.ActualWidth));
            int randX2 = rand.Next(0, (int)Math.Round(MainCanvas.ActualWidth));
            int randY1 = rand.Next(0, (int)Math.Round(MainCanvas.ActualHeight));
            int randY2 = rand.Next(0, (int)Math.Round(MainCanvas.ActualHeight));

            Line newLine = new Line
            {
                X1 = randX1,
                Y1 = randY1,
                X2 = randX2,
                Y2 = randY2,
                Stroke = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                StrokeThickness = 2,
                Cursor = Cursors.SizeAll
            };

            return newLine;
        }

        private void MainCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AxesDrawer.RemoveAxes(canvas: MainCanvas);
            AxesDrawer.DrawAxes(canvas: MainCanvas);
        }

        private Point ConvertToCartesianCoords(Canvas canvas, Point point)
        {
            Point output = new Point(point.X, canvas.ActualHeight - point.Y);
            return output;
        }

        private void btnLmao_Click(object sender, RoutedEventArgs e)
        {
            MediaElement myMediaElement = new MediaElement();
            myMediaElement.Source = new Uri("Resources\\ricardo.mp4", UriKind.Relative);
            myMediaElement.IsMuted = false;

            VisualBrush myVisualBrush = new VisualBrush();
            myVisualBrush.Viewport = new Rect(0, 0, 0.5, 0.5);
            myVisualBrush.TileMode = TileMode.Tile;
            myVisualBrush.Visual = myMediaElement;

            MainCanvas.Background = myVisualBrush;
        }
    }
}
