using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ModManager_Avalonia.Clases;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModManager_Avalonia;

public partial class ImageCard : UserControl
{
    public static readonly StyledProperty<string> NombreProperty =
           AvaloniaProperty.Register<ImageCard, string>(nameof(Nombre), "Sin nombre");

    public static readonly StyledProperty<int> CardWidthProperty =
        AvaloniaProperty.Register<ImageCard, int>(nameof(CardWidth), 200);

    public static readonly StyledProperty<int> CardHeightProperty =
        AvaloniaProperty.Register<ImageCard, int>(nameof(CardHeight), 200);

    public static readonly StyledProperty<string> ImgPathProperty =
            AvaloniaProperty.Register<ImageCard, string>(nameof(ImgPath));

    public static readonly StyledProperty<Bitmap?> ImageSourceProperty =
        AvaloniaProperty.Register<ImageCard, Bitmap?>(nameof(ImageSource));

    public static readonly StyledProperty<bool> IsSelectableProperty =
        AvaloniaProperty.Register<ImageCard, bool>(nameof(IsSelectable), false);

    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<ImageCard, bool>(nameof(IsSelected), false);

    public string Nombre
    {
        get => GetValue(NombreProperty);
        set => SetValue(NombreProperty, value);
    }

    public int CardWidth
    {
        get => GetValue(CardWidthProperty);
        set => SetValue(CardWidthProperty, value);
    }

    public int CardHeight
    {
        get => GetValue(CardHeightProperty);
        set => SetValue(CardHeightProperty, value);
    }

    public string ImgPath
    {
        get => GetValue(ImgPathProperty);
        set => SetValue(ImgPathProperty, value);
    }

    public Bitmap? ImageSource
    {
        get => GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public bool IsSelectable
    {
        get => GetValue(IsSelectableProperty);
        set => SetValue(IsSelectableProperty, value);
    }

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public ImageCard()
    {
        InitializeComponent();
    }

    private void Card_Click(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var properties = e.GetCurrentPoint(this).Properties;
        if (DataContext is ImageCardModel clickedImageCard && sender is Border border && properties.IsLeftButtonPressed)
        {
            if (clickedImageCard.IsSelectable)
            {
                clickedImageCard.IsSelected = !clickedImageCard.IsSelected;
                if (IsSelected)
                {
                    string newDirMod = Path.Combine(Path.GetDirectoryName(clickedImageCard.DirMod), Path.GetFileName(clickedImageCard.DirMod).Replace("DISABLED_", ""));
                    Directory.Move(clickedImageCard.DirMod, newDirMod);
                    clickedImageCard.DirMod = newDirMod;
                    Debug.WriteLine("DirMod new value Enabled: " + clickedImageCard.DirMod);
                }
                else
                {
                    string newDirMod = Path.Combine(Path.GetDirectoryName(clickedImageCard.DirMod), "DISABLED_" + Path.GetFileName(clickedImageCard.DirMod));
                    Directory.Move(clickedImageCard.DirMod, newDirMod);
                    clickedImageCard.DirMod = newDirMod;
                    Debug.WriteLine("DirMod new value Disabled: " + clickedImageCard.DirMod);
                }
            }
            else
            {
                string modsPath = Path.Combine(AppHelper.PathMods,"Characters",Nombre);
                if (Directory.Exists(modsPath))
                {
                    AppHelper.LoadMods(modsPath);
                }
                else
                {
                    Debug.WriteLine($"No existe: {modsPath}");
                }
            }
        }
    }
}