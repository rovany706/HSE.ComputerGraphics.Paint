﻿using System;
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

namespace HSE.ComputerGraphics.Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            }
            if (hitTestResult.VisualHit is Shape)
            {
                //Select new element
                currentSelection = hitTestResult.VisualHit as Shape;
                currentSelection.Stroke = Brushes.Red;
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

            lbMousePosition.Text = $"X: {currentMousePosition.X} Y:{currentMousePosition.Y}";

            float radius = 6;
            if (isMousePressed && currentSelection != null)
            {
                Line line = currentSelection as Line;

                bool isMouseNearBegin = Math.Pow(line.X1 - previousMousePosition.X, 2) + Math.Pow(line.Y1 - previousMousePosition.Y, 2) < Math.Pow(radius, 2);
                bool isMouseNearEnd = Math.Pow(line.X2 - previousMousePosition.X, 2) + Math.Pow(line.Y2 - previousMousePosition.Y, 2) < Math.Pow(radius, 2);
                Vector delta = previousMousePosition - currentMousePosition;

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
                    //currentSelection.RenderTransform = new TranslateTransform(currentSelection.RenderTransform.Value.OffsetX - delta.X, currentSelection.RenderTransform.Value.OffsetY - delta.Y);
                    line.X1 -= delta.X;
                    line.X2 -= delta.X;
                    line.Y1 -= delta.Y;
                    line.Y2 -= delta.Y;
                }
                previousMousePosition = currentMousePosition;
            }
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
        }

        private Line GetRandomLine()
        {
            Random rand = new Random();
            int randX1 = rand.Next(0, (int)Math.Round(MainCanvas.ActualWidth));
            int randX2 = rand.Next(0, (int)Math.Round(MainCanvas.ActualWidth));
            int randY1 = rand.Next(0, (int)Math.Round(MainCanvas.ActualHeight));
            int randY2 = rand.Next(0, (int)Math.Round(MainCanvas.ActualHeight));

            Line newLine = new Line();
            newLine.X1 = randX1;
            newLine.Y1 = randY1;
            newLine.X2 = randX2;
            newLine.Y2 = randY2;

            newLine.Stroke = Brushes.Black;

            newLine.HorizontalAlignment = HorizontalAlignment.Left;
            newLine.VerticalAlignment = VerticalAlignment.Center;
            newLine.StrokeThickness = 4;

            return newLine;
        }
    }
}
