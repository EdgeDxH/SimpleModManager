using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModManager_Avalonia.Clases
{
    public class AppConfig
    {
        public int ModPreviewWidth { get; set; }
        public int ModPreviewHeight { get; set; }
    }
    public class Games
    {
        public string Nombre { get; set; }
        public string Path { get; set; }
        public List<Category> Categories { get; set; }
    }
    public class Category
    {
        public string Nombre { get; set; }
        public bool IsDirectView { get; set; }
    }
    public class ConfigurationModel
    {
        public AppConfig AppConfig { get; set; }
        public List<Games> Games { get; set; }
    }
}
