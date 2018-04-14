using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

namespace MazeEsc_ah_pe {
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Page {

        private const String WELCOME_BACKGROUND_LOCATION = @"C:\src\MazeEsc-ah-pe\MazeEsc-ah-pe\MazeEsc-ah-pe\assets\welcome2.png";
        private readonly ArrayList CHARACTERS = new ArrayList{"Marlin", "Dory"};

        private Grid grid;

        public Welcome() {
            InitializeComponent();
            CreateGrid();
            CreateCharacterList();
            CreateButtons();
            SetBackground();
        }

        private void CreateGrid() {
            Grid grid = new Grid();
            ArrayList columns = new ArrayList();
            ArrayList rows = new ArrayList();

            // set up columns
            for(int i = 0; i < 3; i++) {
                ColumnDefinition col_i = new ColumnDefinition();
                columns.Add(col_i);
                grid.ColumnDefinitions.Add(col_i);
            }
            // set up rows
            for(int i = 0; i < 5; i++) {
                RowDefinition row_i = new RowDefinition();
                rows.Add(row_i);
                grid.RowDefinitions.Add(row_i);
            }
            this.grid = grid;
            this.Content = grid;
        }

        private void CreateCharacterList() {
            // set up label
            Label label = new Label() {
                Foreground = new SolidColorBrush(Colors.White),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = "Choose Your Character",
                FontSize = 24,
                Height = 50
            };
            Border border = new Border() {
                Height = 35,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E07A30")),
                Child = label
            };
            Grid.SetColumn(border, 1);
            Grid.SetRow(border, 2);

            // set up list
            ListBox list = new ListBox() {
                FontSize = 24
            };
            Grid.SetColumn(list, 1);
            Grid.SetRow(list, 3);
            foreach(String character in CHARACTERS) {
                ListBoxItem item_i = new ListBoxItem() {
                    Content = character
                };
                list.Items.Add(item_i);
            }
            Binding binding = new Binding("SelectedCharacter");
            list.SetBinding(ListBox.SelectedItemProperty, binding);
            this.grid.Children.Add(border);
            this.grid.Children.Add(list);
        }

        private void CreateButtons() {
            Button button = new Button();
            Grid.SetColumn(button, 1);
            Grid.SetRow(button, 4);
            button.Click += this.StartGame;
            button.Content = "Start Game";
            button.FontSize = 24;
            button.Height = 50;
            button.Width = 150;
            this.grid.Children.Add(button);
        }

        private void SetBackground() {
            Image image = new Image() {
                Source = new BitmapImage(new Uri(WELCOME_BACKGROUND_LOCATION))
            };
            ImageBrush brush = new ImageBrush() {
                ImageSource = image.Source
            };
            this.grid.Background = brush;
        }

        private void StartGame(object sender, RoutedEventArgs e) {
            Maze mazePage = new Maze();
            this.NavigationService.Navigate(mazePage);
        }
    }
}
