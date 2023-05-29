using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_wpf.src
{
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; }
        public Direction CurrentDir { get; private set; }
        public int Score { get; private set; }
        public bool IsGameOver { get; private set; }
        public int GameSpeend { get; private set; } = 500;

        private readonly LinkedList<Position> Snake = new LinkedList<Position>();
        private readonly Random random = new Random();

        public void SetGameSpeed(int speed)
        {
            GameSpeend = speed;
        }
        public GameState(int rows, int cols)
        {
            Rows = rows; Cols = cols;
            Grid = new GridValue[Rows, Cols];
            CurrentDir = Direction.Right;
            Score = 0;
            SpawnSnake();
            SpawnFood();
        }

        public void SpawnSnake()
        {
            int row = random.Next(2, Rows - 3);
            int col = random.Next(2, Cols - 3);
            for (int i = col; i < col + 4; i++)
            {
                Grid[row, col] = GridValue.Snake;
                Snake.AddFirst(new Position(row, col));
            }
        }

        private IEnumerable<Position> GetEmptyCell()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (Grid[i, j] == GridValue.Empty)
                    {
                        yield return new Position(i, j);
                    }
                }
            }
        }
        private void SpawnFood()
        {
            List<Position> emptyList = new List<Position>(GetEmptyCell());
            if (emptyList.Count == 0) return;

            Position pos = emptyList[random.Next(emptyList.Count)];
            Grid[pos.Row, pos.Column] = GridValue.Food;
        }

        public void StartGame()
        {
            SpawnFood();
            SpawnSnake();
        }

        public Position GetHead()
        {
            return Snake.First.Value;
        }

        public Position GetTail()
        {
            return Snake.Last.Value;
        }

        public IEnumerable<Position> GetSnakePostion()
        {
            return Snake;
        }

        private void AddHead(Position newHead)
        {
            Snake.AddFirst(newHead);
            Grid[newHead.Row, newHead.Column] = GridValue.Snake;
        }

        private void RemoveTail()
        {
            Position tail = Snake.Last.Value;
            Grid[tail.Row, tail.Column] = GridValue.Empty;
            Snake.RemoveLast();
        }
        
        public void ChangeDirection(Direction direction)
        {
            CurrentDir = direction;
        }

        public bool IsOutOfBounce(Position pos)
        {
            return pos.Row < 0 || pos.Column < 0 || pos.Row >= Rows || pos.Column >= Cols;
        }

        private GridValue NextGrid(Position newHeadPos)
        {
            if (IsOutOfBounce(newHeadPos))
            {
                return GridValue.OutSide;
            }
            // in case of current head.next is the current tail, the snake will not die
            if (newHeadPos == GetTail())
            {
                return GridValue.Empty;
            }

            return Grid[newHeadPos.Row, newHeadPos.Column];
        }

        public void Move()
        {
            Position newPos = GetHead().translate(CurrentDir);
            GridValue nextMove = NextGrid(newPos);
            if (nextMove == GridValue.OutSide || nextMove == GridValue.Snake)
            {
                IsGameOver = true;
            }
            else if (nextMove == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newPos);
            }
            else
            {
                AddHead(newPos);
                Score++;
                SpawnFood();
            }    
        }
    }
}
