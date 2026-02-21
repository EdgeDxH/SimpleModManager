using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using ModManager_Avalonia.Clases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModManager_Avalonia
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Games> gamesCollection = App.Configuration.GetSection("Games").Get<ObservableCollection<Games>>();
        public ObservableCollection<Category> categories = new ObservableCollection<Category>();
        public ObservableCollection<ImageCardModel> IconCards { get; set; } = new ObservableCollection<ImageCardModel>();
        public ObservableCollection<ImageCardModel> ModCards => AppHelper.ModCards;
        private bool filterByMods = false;
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            cbxCategoria.ItemsSource = categories;
            cbxJuego.ItemsSource = gamesCollection;
            cbxJuego.SelectedIndex = 0;
        }

        private void ToggleMenu(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            svMain.IsPaneOpen = !svMain.IsPaneOpen;
        }

        private async Task LoadImagesAsyc(string pathImages)
        {
            await Dispatcher.UIThread.InvokeAsync(() => IconCards.Clear());

            await Task.Run(() =>
            {
                var imagenes = Directory.EnumerateFiles(pathImages).OrderBy(f => f, StringComparer.OrdinalIgnoreCase);
                foreach (var image in imagenes)
                {
                    if (filterByMods)
                    {
                        string dirTemp = Path.Combine(AppHelper.PathMods, "Characters", Path.GetFileNameWithoutExtension(image));
                        if (Directory.Exists(dirTemp))
                        {
                            bool IsNotEmpty = Directory.EnumerateDirectories(dirTemp).Any();
                            if (!IsNotEmpty)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (AppHelper.ValidImgExt.Any(image.Contains))
                    {
                        ImageCardModel imgCardTemp = new ImageCardModel()
                        {
                            Nombre = Path.GetFileNameWithoutExtension(image),
                            CardWidth = 75,
                            CardHeight = 75,
                            ImgPath = image
                        };
                        Dispatcher.UIThread.Post(() => IconCards.Add(imgCardTemp));
                    }
                }
            });
        }

        private async Task RefreshIcons()
        {
            if (cbxJuego?.SelectedValue is Games selectedGame)
            {
                string pathGameIcons = Path.Combine(AppHelper.BaseDir, "Assets", selectedGame.Nombre, "CharacterPortrait");

                if (Directory.Exists(pathGameIcons))
                {
                    await LoadImagesAsyc(pathGameIcons);
                }
            }
            else
            {
                Debug.WriteLine("cbxJuego es null o no es de la clase Games");
                return;
            }
        }

        private async void cbxJuego_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            cbxJuego.IsEnabled = false;
            if (cbxJuego?.SelectedValue is Games selectedGame)
            {
                AppHelper.PathMods = selectedGame.Path;
                categories.Clear();
                AppHelper.ModCards.Clear();
                foreach (Category x in selectedGame.Categories)
                {
                    categories.Add(x);
                }
                cbxCategoria.SelectedIndex = 0;
            }
            cbxJuego.IsEnabled = true;
        }

        private async void cbxCategoria_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            cbxCategoria.IsEnabled = false;
            if (cbxCategoria?.SelectedValue is Category selectedCategory)
            {
                AppHelper.CurrentCategory = selectedCategory;
                if (selectedCategory.IsDirectView)
                {
                    IconCards.Clear();
                    Debug.WriteLine("La categoria es directa = "+Path.Combine(AppHelper.PathMods, selectedCategory.Nombre));
                    await AppHelper.LoadModsAsync(Path.Combine(AppHelper.PathMods, selectedCategory.Nombre));
                }
                else
                {
                    await RefreshIcons();
                }
            }
            else
            {
                Debug.WriteLine("Categoria es null");
            }
            cbxCategoria.IsEnabled = true;
        }

        private async void btnFilter_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            btnFilter.IsEnabled = false;
            cbxJuego.IsEnabled = false;

            filterByMods = !filterByMods;
            await RefreshIcons();

            cbxJuego.IsEnabled = true;
            btnFilter.IsEnabled = true;
        }
        private async void OnDrop(object? sender, DragEventArgs e)
        {
            var files = e.DataTransfer.TryGetFiles();
            foreach (var f in files)
            {
                Debug.WriteLine(f.Path.LocalPath);
            }
        }
        private void btnAgregar_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            //Debug.WriteLine(App.Configuration["AppConfig:ModPreviewWidth"]);
            //AppConfig appConfig = new AppConfig()
            //{
            //    ModPreviewWidth = 200,
            //    ModPreviewHeight = 300
            //};

            //List<Games> games = new List<Games>();
            //List<string> categories = new List<string>() { "Characters", "UI", "Others" };
            //Games game = new Games()
            //{
            //    Nombre = "GenshinImpact",
            //    Path = @"E:/XXMI-Launcher-Portable-v1.9.2/ZZMI/Mods",
            //    Categories = categories
            //};
            //games.Add(game);
            //ConfigurationModel configModel = new ConfigurationModel()
            //{
            //    AppConfig = appConfig,
            //    Games = games,
            //};
            //string jsonString = JsonConvert.SerializeObject(configModel, Formatting.Indented);
            //File.WriteAllText("C:/Users/Edge/Documents/Test/testConfig.json", jsonString);
        }
    }
}