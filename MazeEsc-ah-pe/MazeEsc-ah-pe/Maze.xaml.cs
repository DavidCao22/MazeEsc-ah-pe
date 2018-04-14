using System;
using System.Collections;
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
    /// Interaction logic for Maze.xaml
    /// </summary>
    public partial class Maze : Page {

        private const String MAZEFILE = @"C:\src\MazeEsc-ah-pe\MazeEsc-ah-pe\MazeEsc-ah-pe\assets\maze_path.txt";
        private const int MARGIN_SIZE = 100;
        private const int COLUMN_SIZE = 40;
        private const int ROW_SIZE = 40;
        private const char WALL = 'x';
        private const char EMPTY = 'o';

        private Grid grid;

        public Maze() {
            InitializeComponent();
            CreateGrid();
            InsertWalls();
            AddBottomButtons();
        }

        private void CreateGrid() {
            Grid grid = new Grid();
            ArrayList columns = new ArrayList();
            ArrayList rows = new ArrayList();

            // set up columns
            // set up margin columns
            ColumnDefinition col0 = new ColumnDefinition();
            ColumnDefinition col21 = new ColumnDefinition();
            col0.Width = new GridLength(MARGIN_SIZE);
            col21.Width = new GridLength(MARGIN_SIZE);
            columns.Add(col0);
            // set up grid columns
            for(int i = 1; i < 21; i++) {
                ColumnDefinition col_i = new ColumnDefinition();
                col_i.Width = new GridLength(COLUMN_SIZE);
                columns.Add(col_i);
            }
            columns.Add(col21);
            foreach(ColumnDefinition col in columns) {
                grid.ColumnDefinitions.Add(col);
            }

            // set up margin rows
            RowDefinition row0 = new RowDefinition();
            RowDefinition row21 = new RowDefinition();
            row0.Height = new GridLength(MARGIN_SIZE);
            row21.Height = new GridLength(MARGIN_SIZE);
            rows.Add(row0);
            // set up grid rows
            for(int i = 1; i < 21; i++) {
                RowDefinition row_i = new RowDefinition();
                row_i.Height = new GridLength(ROW_SIZE);
                rows.Add(row_i);
            }
            rows.Add(row21);
            foreach(RowDefinition row in rows) {
                grid.RowDefinitions.Add(row);
            }
            this.grid = grid;
            this.Content = grid;
        }

        private void InsertWalls() {
            string[] lines = System.IO.File.ReadAllLines(MAZEFILE);
            for(int i = 0; i < lines.Length; i++) {
                string line = lines[i];
                for(int j = 0; j < line.Length; j++) {
                    switch(line[j]) {
                        case WALL:
                            Console.WriteLine("wall");
                            break;
                        case EMPTY:
                            Console.WriteLine("empty");
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void AddBottomButtons() {
            RowDefinition row = this.grid.RowDefinitions[21];
            ColumnDefinition col = this.grid.ColumnDefinitions[3];

            TextBlock text1 = new TextBlock();
            text1.Text = "Author Name";
            text1.Background = new SolidColorBrush(Colors.Green);
            Grid.SetColumn(text1, 0);
            Grid.SetRow(text1, 0);
            this.grid.Children.Add(text1);

        }

        private void ReturnToMenu(object sender, RoutedEventArgs e) {
            Welcome welcomePage = new Welcome();
            this.NavigationService.Navigate(welcomePage);
        }
    }
}
