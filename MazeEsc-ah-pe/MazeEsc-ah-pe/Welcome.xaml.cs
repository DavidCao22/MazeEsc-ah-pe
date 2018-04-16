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

        private const String WELCOME_BACKGROUND_LOCATION = @"./assets/welcome2.png";
        private readonly ArrayList CHARACTERS = new ArrayList{"Marlin", "Dory"};
        private const int NUM_COLS = 4;
        private const int NUM_ROWS = 5;

        private Grid grid;
        private String selectedCharacter;
        private Maze.Difficulty selectedDifficulty;
        private Boolean[] readyToPlay;

        public Welcome() {
            readyToPlay = new Boolean[]{ false, false };
            InitializeComponent();
            CreateGrid();
            CreateCharacterList();
            CreateDifficultyList();
            CreateButtons();
            SetBackground();
        }

        private void CreateGrid() {
            Grid grid = new Grid();
            ArrayList columns = new ArrayList();
            ArrayList rows = new ArrayList();

            // set up columns
            for(int i = 0; i < NUM_COLS; i++) {
                ColumnDefinition col_i = new ColumnDefinition();
                columns.Add(col_i);
                grid.ColumnDefinitions.Add(col_i);
            }
            // set up rows
            for(int i = 0; i < NUM_ROWS; i++) {
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
                Content = "Choose Character",
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
            list.SelectionChanged += CharacterSelect;
            this.grid.Children.Add(border);
            this.grid.Children.Add(list);
        }

        private void CreateDifficultyList() {
            // set up label
            Label label = new Label() {
                Foreground = new SolidColorBrush(Colors.White),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = "Choose Difficulty",
                FontSize = 24,
                Height = 50
            };
            Border border = new Border() {
                Height = 35,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E07A30")),
                Child = label
            };
            Grid.SetColumn(border, 2);
            Grid.SetRow(border, 2);

            // set up list
            ListBox list = new ListBox() {
                FontSize = 24
            };
            Grid.SetColumn(list, 2);
            Grid.SetRow(list, 3);
            Array difficulties = Enum.GetValues(typeof(Maze.Difficulty));
            foreach(Maze.Difficulty difficulty in difficulties) {
                ListBoxItem item_i = new ListBoxItem() {
                    Content = difficulty
                };
                list.Items.Add(item_i);
            }
            list.SelectionChanged += DifficultySelect;
            this.grid.Children.Add(border);
            this.grid.Children.Add(list);
        }

        private void CreateButtons() {
            Button button = new Button();
            Grid.SetColumn(button, 1);
            Grid.SetRow(button, 4);
            Grid.SetColumnSpan(button, 2);
            button.Click += this.StartGame;
            button.Content = "Start Game";
            button.FontSize = 24;
            button.Height = 50;
            button.Width = 150;
            button.IsEnabled = false;
            this.grid.Children.Add(button);
        }

        private void SetBackground() {
            Image image = new Image();
            try {
                image = new Image() {
                    Source = new BitmapImage(new Uri(WELCOME_BACKGROUND_LOCATION, UriKind.Relative))
                };
            } catch (DirectoryNotFoundException) { }
            ImageBrush brush = new ImageBrush() {
                ImageSource = image.Source
            };
            this.grid.Background = brush;
        }

        private void CharacterSelect(object sender, RoutedEventArgs e) {
            ListBox listbox = (ListBox)sender;
            ListBoxItem item = (ListBoxItem)listbox.SelectedItem;
            this.selectedCharacter = (String)item.Content;
            readyToPlay[0] = true;
            if(this.readyToPlay[0] && this.readyToPlay[1]) {
                foreach(Button button in FindVisualChildren<Button>(this.grid)) {
                    button.IsEnabled = true;
                }
            }
        }

        private void DifficultySelect(object sender, RoutedEventArgs e) {
            ListBox listbox = (ListBox)sender;
            ListBoxItem item = (ListBoxItem)listbox.SelectedItem;
            this.selectedDifficulty = (Maze.Difficulty) item.Content;
            readyToPlay[1] = true;
            if (this.readyToPlay[0] && this.readyToPlay[1]) {
                foreach(Button button in FindVisualChildren<Button>(this.grid)) {
                    button.IsEnabled = true;
                }
            }
        }

        private void StartGame(object sender, RoutedEventArgs e) {
            Maze mazePage = new Maze(this.selectedCharacter, this.selectedDifficulty);
            this.NavigationService.Navigate(mazePage);
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject {
            if(depObj != null) {
                for(int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if(child != null && child is T) {
                        yield return (T)child;
                    }

                    foreach(T childOfChild in FindVisualChildren<T>(child)) {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
