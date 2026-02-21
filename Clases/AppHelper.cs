using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModManager_Avalonia.Clases
{
    public static class AppHelper
    {
        public static ObservableCollection<ImageCardModel> ModCards { get; set; } = new ObservableCollection<ImageCardModel>();
        public static string BaseDir = AppContext.BaseDirectory;
        public static string PathMods = "";
        public static string[] ValidImgExt = [".png", ".jpg", ".jpeg", ".webp"];
        public static Category CurrentCategory { get; set; }

        public static void LoadMods(string pathMods)
        {
            ModCards.Clear();
            string[] dirMods = Directory.EnumerateDirectories(pathMods).OrderBy(f => f, StringComparer.OrdinalIgnoreCase).ToArray();
            foreach (string dirMod in dirMods)
            {
                string pathPreview = Path.Combine(BaseDir,"Assets","App","NoImage.png");
                bool isModEnable = false;

                var previewTemp = Directory.EnumerateFiles(dirMod).FirstOrDefault(f =>
                {
                    string nombreImagen = Path.GetFileNameWithoutExtension(f);
                    string ext = Path.GetExtension(f);
                    return nombreImagen.Equals("preview", StringComparison.OrdinalIgnoreCase) && ValidImgExt.Contains(ext);
                });
                if (previewTemp != null)
                {
                    pathPreview = previewTemp;
                }
                string nombreMod = Path.GetFileName(dirMod);
                if (dirMod.ToUpper().Contains("DISABLED"))
                {
                    nombreMod = nombreMod.Replace("DISABLED_","");
                }
                else
                {
                    isModEnable = true;
                }
                ImageCardModel imgCardTemp = new ImageCardModel()
                {
                    Nombre = nombreMod,
                    DirMod = dirMod,
                    CardWidth = 350,
                    CardHeight = 350,
                    ImgPath = pathPreview,
                    IsSelectable = true,
                    IsSelected = isModEnable
                };
                ModCards.Add(imgCardTemp);
            }
        }

        public static async Task LoadModsAsync(string pathMods)
        {
            await Dispatcher.UIThread.InvokeAsync(() => ModCards.Clear());

            await Task.Run(() =>
            {
                var dirMods = Directory.EnumerateDirectories(pathMods).OrderBy(f => f, StringComparer.OrdinalIgnoreCase);

                foreach (var dirMod in dirMods)
                {
                    string pathPreview = Path.Combine(BaseDir, "Assets", "App", "NoImage.png");
                    bool isModEnable = false;

                    var previewTemp = Directory.EnumerateFiles(dirMod).FirstOrDefault(f =>
                    {
                        string nombreImagen = Path.GetFileNameWithoutExtension(f);
                        string ext = Path.GetExtension(f);
                        return nombreImagen.Equals("preview", StringComparison.OrdinalIgnoreCase) && ValidImgExt.Contains(ext);
                    });
                    if (previewTemp != null)
                    {
                        pathPreview = previewTemp;
                    }
                    string nombreMod = Path.GetFileName(dirMod);
                    if (dirMod.ToUpper().Contains("DISABLED"))
                    {
                        nombreMod = nombreMod.Replace("DISABLED_", "");
                    }
                    else
                    {
                        isModEnable = true;
                    }
                    ImageCardModel imgCardTemp = new ImageCardModel()
                    {
                        Nombre = nombreMod,
                        DirMod = dirMod,
                        CardWidth = 350,
                        CardHeight = 350,
                        ImgPath = pathPreview,
                        IsSelectable = true,
                        IsSelected = isModEnable
                    };

                    Dispatcher.UIThread.Post(() => ModCards.Add(imgCardTemp));
                }
            });
        }
    }
}
