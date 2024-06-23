using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using FitRectangle.Enums;
using FitRectangle.Models;
using NLog;

namespace FitRectangle
{
    public class FitRectangleViewModel : INotifyPropertyChanged
    {
        #region ONPROPERTYCHANGED 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public FitRectangleViewModel()
        {
            LogManager.GetCurrentClassLogger().Info("Start FitRectangle");

            CountGreenRectangle = 5;
            IsGenerateSecondaryRectanglesInsideMainRectangles = true;
            IsIgnoredPointsInsideMainRectangle = false;
            IsOutlineSecondaryRectangles = false;
            IgnoreGreen = false;
            IgnoreViolet = false;
            IgnorePink = false;

        }

        #region Properties

        public Canvas Canvas { get; set; }

        /// <summary> Главный прямоугольник </summary>
        private Rectangle MaiRectangle { get; set; }
        /// <summary> Коллекция второстепенных прямоугольников </summary>
        private List<Rectangle> SecondaryRectangles { get; set; } = new();

        private int _countGreenRectangle;
        /// <summary> Счетчик зеленых прямоугольников </summary>
        public int CountGreenRectangle
        {
            get => _countGreenRectangle;
            set
            {
                _countGreenRectangle = value;
                NotifyPropertyChanged(nameof(CountGreenRectangle));
            }
        }

        private int _countPinkRectangle;
        /// <summary> Счетчик розовых прямоугольников </summary>
        public int CountPinkRectangle
        {
            get => _countPinkRectangle;
            set
            {
                _countPinkRectangle = value;
                NotifyPropertyChanged(nameof(CountPinkRectangle));
            }
        }

        private int _countVioletRectangle;
        /// <summary> Счетчик фиолетовых прямоугольников </summary>
        public int CountVioletRectangle
        {
            get => _countVioletRectangle;
            set
            {
                _countVioletRectangle = value;
                NotifyPropertyChanged(nameof(CountVioletRectangle));
            }
        }

        private bool _ignoreGreen;
        /// <summary> Признак игнорирования зеленых прямоугольников </summary>
        public bool IgnoreGreen
        {
            get => _ignoreGreen;
            set
            {
                _ignoreGreen = value;
                NotifyPropertyChanged(nameof(IgnoreGreen));

                LogManager.GetCurrentClassLogger().Info(_ignoreGreen
                    ? $"Включен режим игнорирования прямоугольников зеленого цвета"
                    : $"Отключен режим игнорирования прямоугольников зеленого цвета");
            }
        }

        private bool _ignorePink;
        /// <summary> Признак игнорирования розовых прямоугольников </summary>
        public bool IgnorePink
        {
            get => _ignorePink;
            set
            {
                _ignorePink = value;
                NotifyPropertyChanged(nameof(IgnorePink));

                LogManager.GetCurrentClassLogger().Info(_ignoreGreen
                    ? $"Включен режим игнорирования прямоугольников розового цвета"
                    : $"Отключен режим игнорирования прямоугольников розового цвета");
            }
        }

        private bool _ignoreViolet;
        /// <summary> Признак игнорирования фиолетовых прямоугольников </summary>
        public bool IgnoreViolet
        {
            get => _ignoreViolet;
            set
            {
                _ignoreViolet = value;
                NotifyPropertyChanged(nameof(IgnoreViolet));

                LogManager.GetCurrentClassLogger().Info(_ignoreGreen
                    ? $"Включен режим игнорирования прямоугольников фиолотеового цвета"
                    : $"Отключен режим игнорирования прямоугольников фиолетового цвета");
            }
        }

        private bool _isOutlineSecondaryRectangles;
        /// <summary> Признак того, что главный прямоугольник очерчивает массив второстепенных </summary>
        public bool IsOutlineSecondaryRectangles
        {
            get => _isOutlineSecondaryRectangles;
            set
            {
                _isOutlineSecondaryRectangles = value;
                NotifyPropertyChanged(nameof(IsOutlineSecondaryRectangles));
            }
        }


        private bool _isGenerateSecondaryRectanglesInsideMainRectangles;
        /// <summary> Признак генерации второстепенных прямоугольников только внутри главного </summary>
        public bool IsGenerateSecondaryRectanglesInsideMainRectangles
        {
            get => _isGenerateSecondaryRectanglesInsideMainRectangles;
            set
            {
                _isGenerateSecondaryRectanglesInsideMainRectangles = value;
                NotifyPropertyChanged(nameof(IsGenerateSecondaryRectanglesInsideMainRectangles));

                LogManager.GetCurrentClassLogger().Info(_isGenerateSecondaryRectanglesInsideMainRectangles
                    ? $"Включен режим генерации второстепенных прямоугольников только внутри главного прямоугольника"
                    : $"Отключен режим генерации второстепенных прямоугольников только внутри главного прямоугольника");
            }
        }

        private bool _isignoredPointsInsideMainRectangle;
        /// <summary> Признак игнорирования вершин снаружи главного прямоугольника при очерчивании </summary>
        public bool IsIgnoredPointsInsideMainRectangle
        {
            get => _isignoredPointsInsideMainRectangle;
            set
            {
                _isignoredPointsInsideMainRectangle = value;
                NotifyPropertyChanged(nameof(IsIgnoredPointsInsideMainRectangle));

                LogManager.GetCurrentClassLogger().Info(_isignoredPointsInsideMainRectangle
                    ? $"Включен режим игнорирования вершин снаружи главного пррямоугольника при очерчивании"
                    : $"Отключен режим игнорирования вершин снаружи главного пррямоугольника при очерчивании");
            }
        }
        #endregion

        #region Commands

        private RelayCommand сommandOutlineSecondaryRectangles;
        /// <summary> Очертить массив второстепенных прямогольников</summary>
        public RelayCommand CommandOutlineSecondaryRectangles
        {
            get
            {
                return сommandOutlineSecondaryRectangles ??= new RelayCommand(obj =>
                {
                    if (IsOutlineSecondaryRectangles)
                    {
                        InitMainRectangle();
                        DrawRectangles();

                        LogManager.GetCurrentClassLogger().Info($"Главный прямоугольник получил координаты исходного состояния Точка 1: ({MaiRectangle.TopLeft.X}, {MaiRectangle.TopLeft.Y}). Точка 2: ({MaiRectangle.BotRight.X}, {MaiRectangle.BotRight.Y})");
                    }
                    else
                    {
                        OutlineSecondaryRectangles();
                    }
                });
            }
        }

        private RelayCommand commandInitSecondaryRectangles;
        /// <summary> Генерация вторичных прямоугольников</summary>
        public RelayCommand CommandInitSecondaryRectangles
        {
            get
            {
                return commandInitSecondaryRectangles ??= new RelayCommand(obj =>
                {
                    InitMainRectangle();
                    InitSecondaryRectangles();
                    DrawRectangles();
                });
            }
        }

        private RelayCommand commandIncCountRectangles;
        /// <summary> Команда инкремента количества генерируемых прямоугольников по цевтам</summary>
        public RelayCommand CommandIncCountRectangles
        {
            get
            {
                return commandIncCountRectangles ??= new RelayCommand(obj =>
                {
                    if (obj is EColors color)
                    {
                        if (color == EColors.Green)
                        {
                            CountGreenRectangle++;
                            LogManager.GetCurrentClassLogger().Info($"Увеличено количество генерируемых зеленых прямоугольников до : {CountGreenRectangle}");
                        }
                        else if (color == EColors.Pink)
                        {
                            CountPinkRectangle++;
                            LogManager.GetCurrentClassLogger().Info($"Увеличено количество генерируемых розовых прямоугольников до : {CountPinkRectangle}");
                        }
                        else if (color == EColors.Violet)
                        {
                            CountVioletRectangle++;
                            LogManager.GetCurrentClassLogger().Info($"Увеличено количество генерируемых фиолетовых прямоугольников до : {CountVioletRectangle}");
                        }
                    }
                });
            }
        }

        private RelayCommand commandDecCountRectangles;
        /// <summary> Команда инкремента количества генерируемых прямоугольников по цевтам</summary>
        public RelayCommand CommandDecCountRectangles
        {
            get
            {
                return commandDecCountRectangles ??= new RelayCommand(obj =>
                {
                    if (obj is EColors color)
                    {
                        if(CountGreenRectangle + CountPinkRectangle + CountVioletRectangle == 1) return;

                        if (color == EColors.Green && CountGreenRectangle > 0)
                        {
                            CountGreenRectangle--;
                            LogManager.GetCurrentClassLogger().Info($"Уменьшено количество генерируемых зеленых прямоугольников до : {CountGreenRectangle}");
                        }
                        else if (color == EColors.Pink && CountPinkRectangle > 0)
                        {
                            CountPinkRectangle--;
                            LogManager.GetCurrentClassLogger().Info($"Уменьшено количество генерируемых розовых прямоугольников до : {CountPinkRectangle}");
                        }
                        else if (color == EColors.Violet && CountVioletRectangle > 0)
                        {
                            CountVioletRectangle--;
                            LogManager.GetCurrentClassLogger().Info($"Уменьшено количество генерируемых фиолетовых прямоугольников до : {CountPinkRectangle}");
                        }
                    }
                });
            }
        }



        #endregion

        #region Metods

        private void OutlineSecondaryRectangles()
        {
            var newTopLeft = new Point(Canvas.ActualWidth +1, Canvas.ActualHeight+ 1);
            var newBotRight = new Point(-1, -1);

            foreach (var secondaryRectangle in SecondaryRectangles)
            {
                //Проверка на игнорироване прямоугольников опеределенного цвета
                if((IgnoreGreen && secondaryRectangle.Color == EColors.Green) ||
                   (IgnorePink && secondaryRectangle.Color == EColors.Pink) ||
                   (IgnoreViolet && secondaryRectangle.Color == EColors.Violet)) continue;

                if(secondaryRectangle.TopLeft.X < newTopLeft.X && 
                   (!IsIgnoredPointsInsideMainRectangle || 
                    IsIncludeAnyListPointsToRectangle(new List<Point>(){secondaryRectangle.TopLeft, new (secondaryRectangle.TopLeft.X, secondaryRectangle.BotRight.Y )}, MaiRectangle))) 
                    newTopLeft.X = secondaryRectangle.TopLeft.X;

                if(secondaryRectangle.TopLeft.Y < newTopLeft.Y &&
                   (!IsIgnoredPointsInsideMainRectangle || 
                    IsIncludeAnyListPointsToRectangle(new List<Point>() { secondaryRectangle.TopLeft, new(secondaryRectangle.BotRight.X, secondaryRectangle.TopLeft.Y) }, MaiRectangle))) 
                    newTopLeft.Y = secondaryRectangle.TopLeft.Y;

                if (secondaryRectangle.BotRight.X > newBotRight.X &&
                    (!IsIgnoredPointsInsideMainRectangle || 
                     IsIncludeAnyListPointsToRectangle(new List<Point>() { secondaryRectangle.BotRight, new(secondaryRectangle.BotRight.X, secondaryRectangle.TopLeft.Y) }, MaiRectangle))) 
                    newBotRight.X = secondaryRectangle.BotRight.X;

                if (secondaryRectangle.BotRight.Y > newBotRight.Y &&
                    (!IsIgnoredPointsInsideMainRectangle || 
                     IsIncludeAnyListPointsToRectangle(new List<Point>() { secondaryRectangle.BotRight, new(secondaryRectangle.TopLeft.X, secondaryRectangle.BotRight.Y) }, MaiRectangle))) 
                    newBotRight.Y = secondaryRectangle.BotRight.Y;
            }

            if (newTopLeft.X > Canvas.ActualWidth) newTopLeft.X = MaiRectangle.TopLeft.X;
            if (newTopLeft.Y > Canvas.ActualHeight) newTopLeft.Y = MaiRectangle.TopLeft.Y;
            if (newBotRight.X < 0) newBotRight.X = MaiRectangle.BotRight.X;
            if (newBotRight.Y < 0) newBotRight.Y = MaiRectangle.BotRight.Y;

            MaiRectangle.TopLeft = newTopLeft;
            MaiRectangle.BotRight = newBotRight;

            IsOutlineSecondaryRectangles = true;

            LogManager.GetCurrentClassLogger().Info($"Главный прямоугольник в результате очерчивания получил координаты: Точка 1: ({MaiRectangle.TopLeft.X}, {MaiRectangle.TopLeft.Y}). Точка 2: ({MaiRectangle.BotRight.X}, {MaiRectangle.BotRight.Y})");
        }

        /// <summary> Проверяет, входит ли хотя бы одна точка из списка в прямоугольник</summary>
        public bool IsIncludeAnyListPointsToRectangle(List<Point> points, Rectangle rectangle)
        {
            foreach (var point in points)    
            {
                if(rectangle.TopLeft.X <= point.X && rectangle.TopLeft.Y <= point.Y && rectangle.BotRight.X >= point.X && rectangle.BotRight.Y >= point.Y)
                    return true;
            }
            return false;
        }

        public void InitSecondaryRectangles()
        {
            SecondaryRectangles.Clear();

            for (var i = 0; i < CountGreenRectangle; i++)
                InitSecondaryRectanglesByColor(EColors.Green);

            for (var i = 0; i < CountPinkRectangle; i++)
                InitSecondaryRectanglesByColor(EColors.Pink);

            for (var i = 0; i < CountVioletRectangle; i++)
                InitSecondaryRectanglesByColor(EColors.Violet);
        }

        private void InitSecondaryRectanglesByColor(EColors color)
        {
            var secRecPosition = GetPositionForSecondaryRectangle();
            var secRectangle = new Rectangle(secRecPosition.Item1, secRecPosition.Item2, ERectangleType.Secondary, color);
            SecondaryRectangles.Add(secRectangle);

            LogManager.GetCurrentClassLogger().Info($"Генерация второстепенного прямоугольинка c координатами: Точка 1: ({secRectangle.TopLeft.X}, {secRectangle.TopLeft.Y}). Точка 2: ({secRectangle.BotRight.X}, {secRectangle.BotRight.Y})");
        }

        private (Point,Point) GetPositionForSecondaryRectangle()
        {
            var rand = new Random();

            //Определяем прямоугольник для вписывания второстепеного
            var mainRectangle = IsGenerateSecondaryRectanglesInsideMainRectangles
                ? MaiRectangle
                : new Rectangle(new Point(0, 0), new Point(Canvas.ActualWidth, Canvas.ActualHeight), ERectangleType.None,
                    EColors.Transparent);

            //Ограничиваем точку размерами главного прямоугольника
            var topLeftPoint = new Point(
                rand.Next(
                    (int)Math.Ceiling(mainRectangle.TopLeft.X), 
                    (int) Math.Floor(mainRectangle.BotRight.X)),
                rand.Next(
                    (int)Math.Ceiling(mainRectangle.TopLeft.Y), 
                    (int)Math.Floor(mainRectangle.BotRight.Y)));

            //Ограничиваем вторую точку размером прямоугольника и положением первой точки 
            var bottomRightPoint = new Point(
                rand.Next(
                    (int)Math.Ceiling(topLeftPoint.X),
                    (int)Math.Floor(mainRectangle.BotRight.X)),
                rand.Next(
                    (int)Math.Ceiling(topLeftPoint.Y),
                    (int)Math.Floor(mainRectangle.BotRight.Y)));

            return (topLeftPoint, bottomRightPoint);
        }

        public void InitMainRectangle()
        {
            var mainRectanglePosition = GetPositionMainRectangle(Canvas.ActualHeight, Canvas.ActualWidth);

            //Главный прямоугольник
            MaiRectangle = new Rectangle(mainRectanglePosition.Item1, mainRectanglePosition.Item2, ERectangleType.Main, EColors.Transparent);

            LogManager.GetCurrentClassLogger().Info($"Генерация главного прямоугольинка c координатами: Точка 1: ({MaiRectangle.TopLeft.X}, {MaiRectangle.TopLeft.Y}). Точка 2: ({MaiRectangle.BotRight.X}, {MaiRectangle.BotRight.Y})");
        }

        /// <summary> Отрисовка прямоугольников </summary>
        public void DrawRectangles()
        {
            Canvas.Children.Clear();

            foreach (var secondaryRectangle in SecondaryRectangles)
                Canvas.Children.Add(secondaryRectangle.Border);

            Canvas.Children.Add(MaiRectangle.Border);
            IsOutlineSecondaryRectangles = false;
        }

        /// <summary> Расчет положения главного прямоугольника относительно размера canvas </summary>
        private (Point, Point) GetPositionMainRectangle(double canvasHeight, double canvasWidth)
        {
            //Высота главного прямоугольника равна половине высоты канваса
            var height = canvasHeight / 2;
            //Ширины главного прямоугольника равно половине ширины канваса
            var width = canvasWidth / 2;

            //Вычисляем координаты расположения главного прямоугольника
            var topLeftPoint = new Point(canvasWidth / 4, canvasHeight/ 4);
            var bottomRightPoint = new Point((canvasWidth / 4) + width, canvasHeight / 4 + height);

            return (topLeftPoint, bottomRightPoint);
        }

        #endregion
    }
}
