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
        private GameState _GameState;
        private bool IsRunning = false;
        private readonly Dictionary<GridValue, ImageSource> GridImageMap = new Dictionary<GridValue, ImageSource>
        {
            {GridValue.Empty, Graphic.EmptyImage },
            {GridValue.Snake, Graphic.BodyImage },
            {GridValue.Food, Graphic.FoodImage },
        };

        private readonly Dictionary<Direction, int> HeadRotation = new()
        {
            {Direction.Up, 0 },
            {Direction.Right, 90 },
            {Direction.Down, 180 },
            {Direction.Left, 270 }
        };

        private readonly Image[,] GameImages;
        public MainWindow()
        {
            InitializeComponent();
            GameImages = SetupGrid();
            _GameState = new GameState(Rows, Cols);
        }

        private void DrawGrid()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    GridValue value = _GameState.Grid[r, c];
                    GameImages[r, c].Source = GridImageMap[value];
                    GameImages[r, c].RenderTransform = Transform.Identity;
                }
            }
        }

        private async Task DrawDead()
        {
            List<Position> snakeCopy = new List<Position>(_GameState.GetSnakePostion());

            for (int i = 0; i < snakeCopy.Count; i++)
            {
                Position deadSnakePos = snakeCopy[i];
                ImageSource source = (i == 0) ? Graphic.DeadHeadImage : Graphic.DeadBodyImage;
                GameImages[deadSnakePos.Row, deadSnakePos.Column].Source = source;
                await Task.Delay(50);
            }
        }

        private void Draw()
        {
            DrawGrid();
            DrawHead();
            ScoreText.Text = $"SCORE {_GameState.Score} LEVEL {_GameState.Level}";
        }

        private void DrawHead()
        {
            Position head = _GameState.GetHead();
            Image headImage = GameImages[head.Row, head.Column];
            headImage.Source = Graphic.HeadImage;
            headImage.RenderTransform = new RotateTransform(HeadRotation[_GameState.CurrentDir]);
        }

        private async Task GameLoop()
        {
            while (!_GameState.IsGameOver) 
            {
                await Task.Delay(_GameState.GameSpeed);
                _GameState.Move();
                Draw();
            }
        }

        private async Task Run()
        {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();
            _GameState = new GameState(Rows, Cols);
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

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }
            if (!IsRunning)
            {
                IsRunning = true;
                await Run();
                IsRunning = false;
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
                        Source = Graphic.EmptyImage,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };
                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }
            return images;
        }

        private async Task ShowCountDown()
        {
            for (int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(1000);
            }
        }

        private async Task ShowGameOver()
        {
            await DrawDead();
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "GAME OVER";     
            await Task.Delay(2000);
            OverlayText.Text = "PRESS ANY KEY TO RESTART";
        }

    }
}
