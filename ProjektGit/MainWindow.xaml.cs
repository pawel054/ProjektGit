﻿using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjektGit
{
    public partial class MainWindow : Window
    {
        private bool eraseMode = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.Strokes.Clear();
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (inkCanvas != null && colorPicker.SelectedColor.HasValue)
            {
                Color selectedColor = colorPicker.SelectedColor.Value;
                inkCanvas.DefaultDrawingAttributes.Color = selectedColor;
            }
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (inkCanvas != null && thicknessSlider != null)
            {
                inkCanvas.DefaultDrawingAttributes.Width = thicknessSlider.Value;
                inkCanvas.DefaultDrawingAttributes.Height = thicknessSlider.Value;
            }
        }

        private void EraseButton_Click(object sender, RoutedEventArgs e)
        {
            eraseMode = !eraseMode;

            if (eraseMode)
            {
                // Zmień kolor przycisku na czerwony w trybie Erase
                selectBtn.Background = new SolidColorBrush(Colors.LightBlue);
            }
            else
            {
                // Zmień kolor przycisku na zielony w trybie Ink
                selectBtn.Background = new SolidColorBrush(Colors.LightGray);
            }

            inkCanvas.EditingMode = eraseMode ? InkCanvasEditingMode.Select : InkCanvasEditingMode.Ink;
        }

        private void InkCanvas_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (eraseMode)
            {
                inkCanvas.EditingMode = InkCanvasEditingMode.Select;
            }
            else
            {
                inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pliki obrazów|*.png;*.bmp;*.jpg|Wszystkie pliki|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                SaveAsImage(saveFileDialog.FileName);
            }
        }

        private void SaveAsImage(string fileName)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)inkCanvas.ActualWidth, (int)inkCanvas.ActualHeight, 96d, 96d, PixelFormats.Default);
            rtb.Render(inkCanvas);

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
            {
                encoder.Save(fs);
            }
        }
    }
}
