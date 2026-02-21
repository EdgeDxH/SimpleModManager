using Avalonia.Controls.Shapes;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModManager_Avalonia.Clases
{
    public class ImageCardModel : INotifyPropertyChanged
    {
        private string imgPath = "";
        private bool isSelected = false;

        public string Nombre { get; set; }
        public string DirMod { get; set; }
        public int CardWidth { get; set; }
        public int CardHeight { get; set; }
        public string ImgPath
        {
            get { return imgPath; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Debug.WriteLine("La ruta esta vacia");
                }
                else if (!File.Exists(value))
                {
                    Debug.WriteLine("Imagen no encontrada");
                }
                else
                {
                    try
                    {
                        using var stream = File.OpenRead(value);
                        var bitmap = Bitmap.DecodeToWidth(stream, CardWidth);
                        ImageSource = bitmap;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"✗ Error cargando {value}: {ex.Message}");
                        ImageSource = null;
                    }
                }
                imgPath = value;
            }}
        public Bitmap? ImageSource { get; set; }
        public bool IsSelectable { get; set; } = false;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
