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

        private enum Direction { Up, Down, Left, Right };
        private enum Animal { Fish, Shark };

        private Grid grid;
        private String filePath;
        private int[] fishLocation = new int[2] { 19, 19 };
        private int[] sharkLocation = new int[2] { 19, 20 };
        private TextBlock info;
        private int[] gogglesLocation = {8,10};

        private TextBlock shark;
        private TextBlock fish;
        private String[] textGrid;
        public Maze() {
            InitializeComponent();
            CreateGrid();
            textGrid = InsertWalls();
            AddBottomButtons();
            InstantiateCharacter("marlin", fishLocation);
            InstantiateCharacter("shark", sharkLocation);
            TextBlock backgroundShape = new TextBlock();
            ImageBrush myBrush = new ImageBrush();
            Image image = new Image();
            BitmapImage bi = new BitmapImage(new Uri(MAZEFILE1 + @"\goggles.png"));
            image.Source = bi;
            myBrush.ImageSource = image.Source;
            backgroundShape.Background = myBrush;
            int i = 0, j = 0;
            Random rnd = new Random();
            while (textGrid[i][j] != 'o')
            {
                i = rnd.Next(1, 20);
                j = rnd.Next(1, 20);
            }
            gogglesLocation[0] = i;
            gogglesLocation[1] = j;
            Grid.SetColumn(backgroundShape, i + 1);
            Grid.SetRow(backgroundShape, j + 1);
            this.grid.Children.Add(backgroundShape);
        }

        public Maze(String character) {
            InitializeComponent();
            CreateGrid();
            textGrid = InsertWalls();
            AddBottomButtons();
            InstantiateCharacter(character.ToLower(), fishLocation);
            InstantiateCharacter("shark", sharkLocation);
            TextBlock backgroundShape = new TextBlock();
            ImageBrush myBrush = new ImageBrush();
            Image image = new Image();
            BitmapImage bi = new BitmapImage(new Uri(MAZEFILE1 + @"\goggles.png"));
            image.Source = bi;
            myBrush.ImageSource = image.Source;
            backgroundShape.Background = myBrush;
            int i = 0, j = 0;
            Random rnd = new Random();
            while (textGrid[i][j] != 'o')
            {
                i = rnd.Next(1, 20);
                j = rnd.Next(1, 20);
            }
            Grid.SetColumn(backgroundShape, i + 1);
            Grid.SetRow(backgroundShape, j + 1);
            this.grid.Children.Add(backgroundShape);
            AddCharacterMovement();
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

        private string WallType(string[] lines, int x, int y)
        {
            //TODO switch statements for clarity
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
                            return "four";
                        }
                        else
                        {
                            return "three_L";
                        }
                    }
                    else
                    {
                        if (left)
                        {
                            return "three_D";
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
                        if (left)
                        {
                            return "three_R";
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
                            return "three_U";
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

        private String[] InsertWalls() {
            string[] lines;
            try
            {
                lines = System.IO.File.ReadAllLines(MAZEFILE2 + @"\maze_path.txt");
                this.filePath = MAZEFILE2;
            }
            catch (DirectoryNotFoundException){
                lines = System.IO.File.ReadAllLines(MAZEFILE1 + @"\maze_path.txt");
                this.filePath = MAZEFILE1;
            }
            for(int i = 0; i < lines.Length; i++) {
                string line = lines[i];
                for(int j = 0; j < line.Length; j++) {
                    TextBlock backgroundShape = new TextBlock();
                    ImageBrush myBrush = new ImageBrush();
                    Image image = new Image();
                    switch (line[j]) {
                        case WALL:
                            switch(WallType(lines, j, i))
                            {
                                case "horizontal":
                                    BitmapImage bi = new BitmapImage();
                                    bi.BeginInit();
                                    bi.UriSource = new Uri(this.filePath + @"\wall_straight.png");
                                    bi.Rotation = Rotation.Rotate90;
                                    bi.EndInit();
                                    image.Source = bi;
                                    break;
                                case "vertical":
                                    image.Source = new BitmapImage(new Uri(filePath + @"\wall_straight.png"));
                                    break;
                                case "corner_UR":
                                    image.Source = new BitmapImage(new Uri(this.filePath + @"\wall_corner.png"));
                                    break;
                                case "corner_UL":
                                    BitmapImage bi1 = new BitmapImage();
                                    bi1.BeginInit();
                                    bi1.UriSource = new Uri(this.filePath + @"\wall_corner.png");
                                    bi1.Rotation = Rotation.Rotate270;
                                    bi1.EndInit();
                                    image.Source = bi1;
                                    break;
                                case "corner_DR":
                                    BitmapImage bi2 = new BitmapImage();
                                    bi2.BeginInit();
                                    bi2.UriSource = new Uri(this.filePath + @"\wall_corner.png");
                                    bi2.Rotation = Rotation.Rotate90;
                                    bi2.EndInit();
                                    image.Source = bi2;
                                    break;
                                case "corner_DL":
                                    BitmapImage bi3 = new BitmapImage();
                                    bi3.BeginInit();
                                    bi3.UriSource = new Uri(this.filePath + @"\wall_corner.png");
                                    bi3.Rotation = Rotation.Rotate180;
                                    bi3.EndInit();
                                    image.Source = bi3;
                                    break;
                                case "empty":
                                    image.Source = new BitmapImage(new Uri(this.filePath + @"\empty.png"));
                                    break;
                                case "three_U":
                                    BitmapImage bi4 = new BitmapImage();
                                    bi4.BeginInit();
                                    bi4.UriSource = new Uri(filePath + @"\wall_split.png");
                                    bi4.Rotation = Rotation.Rotate90;
                                    bi4.EndInit();
                                    image.Source = bi4;
                                    break;
                                case "three_D":
                                    BitmapImage bi5 = new BitmapImage();
                                    bi5.BeginInit();
                                    bi5.UriSource = new Uri(filePath + @"\wall_split.png");
                                    bi5.Rotation = Rotation.Rotate270;
                                    bi5.EndInit();
                                    image.Source = bi5;
                                    break;
                                case "three_L":
                                    image.Source = new BitmapImage(new Uri(filePath + @"\wall_split.png"));
                                    break;
                                case "three_R":
                                    BitmapImage bi6 = new BitmapImage();
                                    bi6.BeginInit();
                                    bi6.UriSource = new Uri(filePath + @"\wall_split.png");
                                    bi6.Rotation = Rotation.Rotate180;
                                    bi6.EndInit();
                                    image.Source = bi6;
                                    break;
                                case "four":
                                    image.Source = new BitmapImage(new Uri(filePath + @"\wall_four.png"));
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case EMPTY:
                            image.Source = new BitmapImage(new Uri(this.filePath + @"\empty.png"));
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
            return lines;
        }

        private void AddBottomButtons() {
            Button button = new Button();
            Grid.SetColumn(button, 2);
            Grid.SetRow(button, 0);
            Grid.SetColumnSpan(button, 3);
            button.Click += this.ReturnToMenu;
            button.Content = "Return To Menu";
            button.FontSize = 16;
            button.Height = 40;
            this.grid.Children.Add(button);
            info = new TextBlock();
            Grid.SetColumn(info, 6);
            Grid.SetRow(info, 0);
            Grid.SetColumnSpan(info, 10);
            info.Text = "Find the Goggles, Then Escape";
            info.FontSize = 20;
            info.Height = 40;
            this.grid.Children.Add(info);
        }

        private void InstantiateCharacter(String fish, int[] location) {
            TextBlock backgroundShape = new TextBlock();
            ImageBrush myBrush = new ImageBrush();
            Image image = new Image();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(this.filePath + "\\" + fish + ".png");
            bi.EndInit();
            image.Source = bi;
            myBrush.ImageSource = image.Source;
            backgroundShape.Background = myBrush;
            Grid.SetColumn(backgroundShape, location[0]);
            Grid.SetRow(backgroundShape, location[1]);
            this.grid.Children.Add(backgroundShape);
            if(fish == "shark") {
                this.shark = backgroundShape;
            } else {
                this.fish = backgroundShape;
            }
        }

        private Boolean Eaten()
        {
            if(fishLocation[0] == sharkLocation[0] && fishLocation[1] == sharkLocation[1])
            {
                LoseGame();
                return true;
            }
            return false;
        }

        private void LoseGame()
        {
            info.Text = "Sorry, The Shark Ate You";
        }

        private void AddCharacterMovement() {
            EventManager.RegisterClassHandler(typeof(Control), Keyboard.KeyUpEvent, new KeyEventHandler(KeyUp), true);
        }

        private new void KeyUp(object sender, KeyEventArgs e) {
            if(e.Key == Key.W) {
                MoveUpDown(Animal.Fish, Direction.Up);
            } else if(e.Key == Key.S){
                MoveUpDown(Animal.Fish, Direction.Down);
            } else if(e.Key == Key.A){
                MoveLeftRight(Animal.Fish, Direction.Left);
            } else if(e.Key == Key.D){
                MoveLeftRight(Animal.Fish, Direction.Right);
            }
            Grid.SetColumn(this.fish, this.fishLocation[0]);
            Grid.SetRow(this.fish, this.fishLocation[1]);
        }

        private void MoveUpDown(Animal animal, Direction direction) {
            if(animal == Animal.Fish) {
                if((direction == Direction.Up) && (this.fishLocation[1] != 1)) {
                    this.fishLocation[1]--;
                } else if((direction == Direction.Down) && (this.fishLocation[1] != 20)) {
                    this.fishLocation[1]++;
                }
            } else {
                if((direction == Direction.Up) && (this.sharkLocation[1] != 1)) {
                    this.sharkLocation[1]--;
                } else if((direction == Direction.Down) && (this.sharkLocation[1] != 20)) {
                    this.sharkLocation[1]++;
                }
            }
        }

        private void MoveLeftRight(Animal animal, Direction direction) {
            if(animal == Animal.Fish) {
                if((direction == Direction.Left) && (this.fishLocation[0] != 1)) {
                    this.fishLocation[0]--;
                } else if((direction == Direction.Right) && (this.fishLocation[0] != 20)) {
                    this.fishLocation[0]++;
                }
            } else {
                if((direction == Direction.Left) && (this.sharkLocation[0] != 1)) {
                    this.sharkLocation[0]--;
                } else if((direction == Direction.Right) && (this.sharkLocation[0] != 20)) {
                    this.sharkLocation[0]++;
                }
            }
        }

        private void ReturnToMenu(object sender, RoutedEventArgs e) {
            Welcome welcomePage = new Welcome();
            this.NavigationService.Navigate(welcomePage);
        }
    }
}
