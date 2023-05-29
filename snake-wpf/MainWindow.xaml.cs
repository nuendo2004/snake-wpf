using snake_wpf.src;
using System;
using System.Collections.Generic;
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

namespace snake_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int Rows = 15, Cols = 15;
        private readonly GameState _GameState;
        private readonly Dictionary<GridValue, ImageSource> GridImageMap = new Dictionary<GridValue, ImageSource>
        {
            {GridValue.Empty, Graphic.EmptyImage },
            {GridValue.Snake, Graphic.BodyImage },
            {GridValue.Food, Graphic.FoodImage },
        };

        private readonly Image[,] GameImages;
        public MainWindow()
        {
            InitializeComponent();
            GameImages = SetupGrid();
            _GameState = new GameState(Rows, Cols);
        }

        private async Task GameLoop()
        {
            while (!_GameState.IsGameOver) 
            {
                await Task.Delay(_GameState.GameSpeend);
                _GameState.Move();
                Draw();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Draw();
            await GameLoop();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_GameState.IsGameOver) { return; }
            switch (e.Key)
            {
                case Key.Up:
                    _GameState.ChangeDirection(Direction.Up); break;
                case Key.W:
                    _GameState.ChangeDirection(Direction.Up); break;
                case Key.Down:
                    _GameState.ChangeDirection(Direction.Down); break;
                case Key.S:
                    _GameState.ChangeDirection(Direction.Down); break;
                case Key.Left:
                    _GameState.ChangeDirection(Direction.Left); break;
                case Key.A:
                    _GameState.ChangeDirection(Direction.Left); break;
                case Key.Right:
                    _GameState.ChangeDirection(Direction.Right); break;
                case Key.D:
                    _GameState.ChangeDirection(Direction.Right); break;
            }
        }

        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[Rows, Cols];
            GameGrid.Rows = Rows; GameGrid.Columns = Cols;

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Graphic.EmptyImage
                    };
                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }
            return images;
        }

        private void DrawGrid()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0;c < Cols; c++)
                {
                    GridValue value = _GameState.Grid[r, c];
                    GameImages[r, c].Source = GridImageMap[value];
                }
            }
        }

        private void Draw()
        {
            DrawGrid();
        }
    }
}
