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

        private const String MAZEFILE1 = @"C:\Users\Anthony\Documents\CornHacks\MazeEsc-ah-pe\MazeEsc-ah-pe\MazeEsc-ah-pe\assets";
        private const String MAZEFILE2 = @"C:\src\MazeEsc-ah-pe\MazeEsc-ah-pe\MazeEsc-ah-pe\assets";
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
            InstantiateCharacter();
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
                ColumnDefinition col_i = new ColumnDefinition() {
                    Width = new GridLength(COLUMN_SIZE)
                };
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
                RowDefinition row_i = new RowDefinition() {
                    Height = new GridLength(ROW_SIZE)
                };
                rows.Add(row_i);
            }
            rows.Add(row21);
            foreach(RowDefinition row in rows) {
                grid.RowDefinitions.Add(row);
            }
            this.grid = grid;
            this.Content = grid;
        }

        private string wallType(string[] lines, int x, int y)
        {
            Boolean up = false, down = false, left = false, right = false;
            if(y != 0 && lines[y - 1][x] == WALL)
            {
                up = true;
            }
            if (y != lines.Length - 1 && lines[y + 1][x] == WALL)
            {
                down = true;
            }
            if (x != 0 && lines[y][x - 1] == WALL)
            {
                left = true;
            }
            if (x != lines[y].Length - 1 && lines[y][x + 1] == WALL)
            {
                right = true;
            }
            if (up)
            {
                if (right)
                {
                    if (down)
                    {
                        if (left)
                        {
                            return "empty";
                        }
                        else
                        {
                            return "vertical";
                        }
                    }
                    else
                    {
                        if (left)
                        {
                            return "horizontal";
                        }
                        else
                        {
                            return "corner_UR";
                        }
                    }
                }
                else
                {
                    if (down)
                    {
                        return "vertical";
                    }
                    else
                    {
                        if (left)
                        {
                            return "corner_UL";
                        }
                        else
                        {
                            return "vertical";
                        }
                    }
                }
            }
            else
            {
                if (right)
                {
                    if (down)
                    {
                        if (left)
                        {
                            return "horizontal";
                        }
                        else
                        {
                            return "corner_DR";
                        }
                    }
                    else
                    {
                        return "horizontal";
                    }
                }
                else
                {
                    if (down)
                    {
                        if (left)
                        {
                            return "corner_DL";
                        }
                        else
                        {
                            return "vertical";
                        }
                    }
                    else
                    {
                        if (left)
                        {
                            return "horizontal";
                        }
                        else
                        {
                            return "empty";
                        }
                    }
                }
            }
        }

        private void InsertWalls() {
            string[] lines;
            string filePath;
            try
            {
                lines = System.IO.File.ReadAllLines(MAZEFILE2 + @"\maze_path.txt");
                filePath = MAZEFILE2;
            }
            catch (DirectoryNotFoundException){
                lines = System.IO.File.ReadAllLines(MAZEFILE1 + @"\maze_path.txt");
                filePath = MAZEFILE1;
            }
            for(int i = 0; i < lines.Length; i++) {
                string line = lines[i];
                for(int j = 0; j < line.Length; j++) {
                    TextBlock backgroundShape = new TextBlock();
                    ImageBrush myBrush = new ImageBrush();
                    Image image = new Image();
                    switch (line[j]) {
                        case WALL:
                            switch(wallType(lines, i, j))
                            {
                                case "vertical":
                                    BitmapImage bi = new BitmapImage();
                                    bi.BeginInit();
                                    bi.UriSource = new Uri(filePath + @"\wall_straight.png");
                                    bi.Rotation = Rotation.Rotate90;
                                    bi.EndInit();
                                    image.Source = bi;
                                    break;
                                case "horizontal":
                                    image.Source = new BitmapImage(new Uri(filePath + @"\wall_straight.png"));
                                    break;
                                case "corner_UR":
                                    image.Source = new BitmapImage(new Uri(filePath + @"\wall_corner.png"));
                                    break;
                                case "corner_UL":
                                    BitmapImage bi1 = new BitmapImage();
                                    bi1.BeginInit();
                                    bi1.UriSource = new Uri(filePath + @"\wall_corner.png");
                                    bi1.Rotation = Rotation.Rotate270;
                                    bi1.EndInit();
                                    image.Source = bi1;
                                    break;
                                case "corner_DR":
                                    BitmapImage bi2 = new BitmapImage();
                                    bi2.BeginInit();
                                    bi2.UriSource = new Uri(filePath + @"\wall_corner.png");
                                    bi2.Rotation = Rotation.Rotate90;
                                    bi2.EndInit();
                                    image.Source = bi2;
                                    break;
                                case "corner_DL":
                                    BitmapImage bi3 = new BitmapImage();
                                    bi3.BeginInit();
                                    bi3.UriSource = new Uri(filePath + @"\wall_corner.png");
                                    bi3.Rotation = Rotation.Rotate180;
                                    bi3.EndInit();
                                    image.Source = bi3;
                                    break;
                                case "empty":
                                    image.Source = new BitmapImage(new Uri(filePath + @"\empty.png"));
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case EMPTY:
                            image.Source = new BitmapImage(new Uri(filePath + @"\empty.png"));
                            break;
                        default:
                            break;
                    }
                    myBrush.ImageSource = image.Source;
                    backgroundShape.Background = myBrush;
                    Grid.SetColumn(backgroundShape, j + 1);
                    Grid.SetRow(backgroundShape, i + 1);
                    this.grid.Children.Add(backgroundShape);
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

            Button button = new Button();
            Grid.SetColumn(button, 2);
            Grid.SetRow(button, 21);
            Grid.SetColumnSpan(button, 3);
            button.Click += this.ReturnToMenu;
            button.Content = "Return To Menu";
            button.FontSize = 16;
            button.Height = 40;
            this.grid.Children.Add(button);
        }

        private void InstantiateCharacter() {

        }

        private void ReturnToMenu(object sender, RoutedEventArgs e) {
            Welcome welcomePage = new Welcome();
            this.NavigationService.Navigate(welcomePage);
        }
    }
}
