using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using System.Windows;

namespace View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void SetTheme(string name)
        {
            var resourceDictionary = new ResourceDictionary() { Source = new Uri($"Themes/{name}.xaml", UriKind.Relative) };
            var resources = new ResourceDictionary();
            resources.MergedDictionaries.Add(resourceDictionary);

            this.Resources = resourceDictionary;
        }

        public IEnumerable<string> Themes
        {
            get
            {
                yield return "Dark";
                yield return "Light";
            }
        }
    }
}
