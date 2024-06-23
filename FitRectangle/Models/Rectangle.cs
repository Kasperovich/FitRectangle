using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FitRectangle.Enums;

namespace FitRectangle.Models
{
    /// <summary> Модель прямоугольника </summary>
    public class Rectangle
    {
        public Rectangle(Point topLeft, Point botRight, ERectangleType type, EColors color)
        {
            TopLeft = topLeft;
            BotRight = botRight;
            Type = type;
            Color = color;

            InitRectangle();
        }


        #region Properties
        public Border Border { get; set; } = new();

        private Point _topLeft;
        /// <summary> Точка задающая верхний левый угол </summary>
        public Point TopLeft
        {
            get => _topLeft;
            set
            {
                _topLeft = value;
                ChangeSize();
            }
        }

        private Point _botRight;

        /// <summary> Точка задающая верхний правый нижний</summary>
        public Point BotRight
        {
            get => _botRight;
            set
            {
                _botRight = value;
                ChangeSize();
            }
        }

        /// <summary> Тип прямоугольника </summary>
        public ERectangleType Type { get; set; }

        private EColors _color;
        /// <summary> Цвет прямоугольника </summary>
        public EColors Color
        {
            get => _color;
            set
            {
                _color = value;

                if (_color == EColors.Transparent)
                    Border.Background = Brushes.Transparent;
                if (_color == EColors.Pink)
                    Border.Background = Brushes.DeepPink;
                if (_color == EColors.Green)
                    Border.Background = Brushes.Green;
                if (_color == EColors.Violet)
                    Border.Background = Brushes.DarkViolet;
            }
        }

        /// <summary> Толщина границы прямоугольника </summary>
        public double BorderTicket
        {
            get => Border.BorderThickness.Top;
            set => Border.BorderThickness = new Thickness(value, value, value, value);
        }

        /// <summary> Цвет границы прямоугольника </summary>
        public Brush BorderBrush
        {
            get => Border.BorderBrush;
            set => Border.BorderBrush = value;
        }

        #endregion


        #region Metods

        /// <summary> Инициализация прямоугольника </summary>
        private void InitRectangle()
        {
            if (Type == ERectangleType.Secondary)
            {
                Border.Opacity = 0.7;
                BorderTicket = 1;
                BorderBrush = Brushes.Black;
            }
            else if (Type == ERectangleType.Main)
            {
                BorderTicket = 3;
                BorderBrush = Brushes.Red;
            }
        }

        private void ChangeSize()
        {
            if(BotRight == null || TopLeft == null) return;

            if ((BotRight.Y - TopLeft.Y < 0) || (BotRight.X - TopLeft.X < 0)) return;
            
            Border.Margin = new Thickness(TopLeft.X, TopLeft.Y, 0, 0);
            Border.Height = BotRight.Y - TopLeft.Y;
            Border.Width = BotRight.X - TopLeft.X;
        }

        #endregion
    }
}
