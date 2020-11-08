using Cells;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace View.Screens
{
    /// <summary>
    /// Interaction logic for SettingsScreen.xaml
    /// </summary>
    public partial class SettingsScreen : UserControl
    {
        public SettingsScreen()
        {
            this.SelectedTheme = Cell.Create<string>(null);
            this.SelectedTheme.ValueChanged += () => (App.Current as App).SetTheme(this.SelectedTheme.Value);

            InitializeComponent();
            
            themesComboBox.SelectedIndex = 0;
        }

        public IEnumerable<string> Themes => (App.Current as App).Themes;

        public ICell<string> SelectedTheme { get; }
    }
}
