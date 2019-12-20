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
        private Line lastClickedLine;
        private List<MyLine> currentSelection = new List<MyLine>();
        private List<LineGroup> currentGroupSelection = new List<LineGroup>();
        private List<LineGroup> lineGroups = new List<LineGroup>();
        private Dictionary<Line, MyLine> lines = new Dictionary<Line, MyLine>();
        private Point previousMousePosition;
        private bool isMousePressed;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAddLine_Click(object sender, RoutedEventArgs e)
        {
            Line newLine = GetRandomLine();
            MyLine line = new MyLine { Line = newLine };
            lines.Add(newLine, line);

            MainCanvas.Children.Add(newLine);
        }

        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            if (canvas == null)
                return;

            HitTestResult hitTestResult = VisualTreeHelper.HitTest(canvas, e.GetPosition(canvas));

            if (hitTestResult.VisualHit is Line selectedLine)
            {
                MyLine myLine = lines[selectedLine];

                if (Keyboard.IsKeyDown(Key.LeftCtrl) == false)
                {
                    foreach (var line in currentSelection)
                        line.Deselect();
                    currentSelection.Clear();
                    currentGroupSelection.Clear();
                }

                //Select new element
                if (currentSelection.Contains(myLine) == false)
                {
                    if (myLine.Group != null)
                    {
                        LineGroup group = lineGroups.Find(x => myLine.Group == x);
                        group.Select();
                        currentSelection.AddRange(group.Lines);
                        currentGroupSelection.Add(group);
                    }
                    else
                        currentSelection.Add(myLine);
                    currentSelection.Last().Select();
                    lastClickedLine = selectedLine;
                    lbEquation.Text = $"Уравнение: {lastClickedLine.GetLineConstants()}";
                }
            }
            else if (hitTestResult.VisualHit is Canvas)
            {
                foreach (var line in currentSelection)
                    line.Deselect();
                currentSelection.Clear();
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

            if (currentSelection.Any())
            {
                MyLine currentLine = lines[lastClickedLine];
                lbEquation.Text = $"Уравнение: {lastClickedLine.GetLineConstants()}";

                float radius = 10;
                bool isMouseNearBegin = Math.Pow(lastClickedLine.X1 - previousMousePosition.X, 2) + Math.Pow(lastClickedLine.Y1 - previousMousePosition.Y, 2) < Math.Pow(radius, 2);
                bool isMouseNearEnd = Math.Pow(lastClickedLine.X2 - previousMousePosition.X, 2) + Math.Pow(lastClickedLine.Y2 - previousMousePosition.Y, 2) < Math.Pow(radius, 2);

                if (isMouseNearBegin || isMouseNearEnd)
                    lastClickedLine.Cursor = Cursors.SizeNWSE;
                else
                    lastClickedLine.Cursor = Cursors.SizeAll;

                if (isMousePressed)
                {
                    if (isMouseNearBegin)
                    {
                        lastClickedLine.X1 = currentMousePosition.X;
                        lastClickedLine.Y1 = currentMousePosition.Y;
                    }
                    else if (isMouseNearEnd)
                    {
                        lastClickedLine.X2 = currentMousePosition.X;
                        lastClickedLine.Y2 = currentMousePosition.Y;
                    }
                    else
                    {
                        Vector delta = previousMousePosition - currentMousePosition;
                        if (currentLine.Group != null)
                            currentLine.Group.Move(delta);
                        else
                            currentLine.Move(delta);
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
            if (currentSelection.Any() == false)
                return;

            List<LineGroup> groupsToDelete = new List<LineGroup>();
            foreach (var line in currentSelection)
            {
                MainCanvas.Children.Remove(line.Line);
                if (line.Group != null)
                {
                    if(groupsToDelete.Contains(line.Group) == false)
                        groupsToDelete.Add(line.Group);
                }
                
                lines.Remove(line.Line);
            }
            foreach (var group in groupsToDelete)
            {
                lineGroups.Remove(group);
            }

            currentSelection.Clear();
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

        private void btnGroup_Click(object sender, RoutedEventArgs e)
        {
            LineGroup newLineGroup = new LineGroup(lines.Values.Where(x => x.IsSelected == true).ToList());
            lineGroups.Add(newLineGroup);
        }
    }
}
