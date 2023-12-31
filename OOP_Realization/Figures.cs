using System.ComponentModel.DataAnnotations;

namespace OOP_Realization
{
    public abstract class BaseFigure
    {
        public BaseFigure(string data) => DataFigure = data;
        public Figure TypeFigure;
        protected private string DataFigure;
        protected private double PerimetrFigure;
        protected private double AreaFigure;
        protected private bool IsCreate = false;
        public void Create()
        {
            SetSizeFromData();
            SetPerimetrFigure();
            SetAreaFigure();
            IsCreate = true;
        }
        public abstract string GetInformationFigure();
        protected abstract void SetSizeFromData();
        protected abstract void SetPerimetrFigure();
        protected abstract void SetAreaFigure();
    }
    public class Rectangle : BaseFigure
    {
        public Rectangle(string data) : base(data) 
        {
            TypeFigure = Figure.Rectangle;
        }
        public double Width { get; private set; }
        public double Height { get; private set; }
        protected override void SetSizeFromData()
        {
            string[] sides = DataFigure.Split(", ");
            if (sides.Length != 2)
                throw new ArgumentOutOfRangeException("Данные вышли за предел массива");
            Width = double.Parse(sides[0]);
            Height = double.Parse(sides[1]);

            if (Width == Height)
                TypeFigure = Figure.Square;
        }
        protected override void SetAreaFigure()
        {
            AreaFigure = (Width * 2) * (Height * 2);
        }
        protected override void SetPerimetrFigure()
        {
            PerimetrFigure = (Width * 2) + (Height * 2);
        }
        public override string GetInformationFigure()
        {
            return IsCreate
                    ? $"Тип фигуры: {TypeFigure}\n"
                    + $"Ширина: {Math.Round(Width, 2)}см\n"
                    + $"Высота: {Math.Round(Height, 2)}см\n"
                    + $"Периметр: {Math.Round(PerimetrFigure, 2)}см\n"
                    + $"Площадь: {Math.Round(AreaFigure, 2)}см в квадрате\n"
                    : "Фигура не была создана.\n";
        }
    }
    public class Square : BaseFigure
    {
        public Square(string data) : base(data)
        {
            TypeFigure = Figure.Square;
        }
        public double SideSize { get; private set; }
        protected override void SetSizeFromData()
        {
            SideSize = double.Parse(DataFigure);
        }
        protected override void SetAreaFigure()
        {
            AreaFigure = Math.Pow(SideSize, 2);
        }
        protected override void SetPerimetrFigure()
        {
            PerimetrFigure = SideSize * 4;
        }
        public override string GetInformationFigure()
        {
            return IsCreate
                    ? $"Тип фигуры: {TypeFigure}\n"
                    + $"Стороны фигуры: {Math.Round(SideSize, 2)}см\n"
                    + $"Периметр: {Math.Round(PerimetrFigure, 2)}\n"
                    + $"Площадь: {Math.Round(AreaFigure, 2)}см в квадрате\n"
                    : "Фигура не была создана.\n";
        }
    }
    public class Round : BaseFigure
    {
        public Round(string data) : base(data)
        {
            TypeFigure = Figure.Round;
        }
        public double Radius { get; private set; }
        protected override void SetSizeFromData()
        {
            Radius = double.Parse(DataFigure);
        }
        protected override void SetAreaFigure()
        {
            AreaFigure = Math.PI * Math.Pow(Radius, 2);
        }
        protected override void SetPerimetrFigure()
        {
            PerimetrFigure = 2 * Math.PI * Radius;
        }
        public override string GetInformationFigure()
        {
            return IsCreate
                    ? $"Тип фигуры: {TypeFigure}\n"
                    + $"Радиус: {Math.Round(Radius, 2)}см\n"
                    + $"Периметр: {Math.Round(PerimetrFigure, 2)}\n"
                    + $"Площадь: {Math.Round(AreaFigure, 2)}см в квадрате\n"
                    : "Фигура не была создана.\n";
        }
    }
    public class Trinagle : BaseFigure
    {
        public Trinagle(string data) : base(data) 
        {
            TypeFigure = Figure.Trinagle;
        }
        public double LeftSide { get; private set; }
        public double RightSide { get; private set; }
        public double BaseSide { get; private set; }
        private Rectangle _typeRectangle;
        protected override void SetSizeFromData()
        {
            string[] sides = DataFigure.Split(", ");
            if (sides.Length != 3)
                throw new ArgumentOutOfRangeException("Данные вышли за предел массива");
            LeftSide = double.Parse(sides[0]);
            RightSide = double.Parse(sides[1]);
            BaseSide = double.Parse(sides[2]);
            SetTypeTrinagle();
        }
        protected override void SetAreaFigure()
        {
            AreaFigure = LeftSide * RightSide * BaseSide;
        }
        protected override void SetPerimetrFigure()
        {
            PerimetrFigure = LeftSide + RightSide + BaseSide;
        }
        private void SetTypeTrinagle()
        {
            _typeRectangle =
                LeftSide == RightSide && RightSide == BaseSide
                ? Rectangle.Equilateral
                : LeftSide == RightSide
                ? Rectangle.Isosceles 
                : Rectangle.Versatile;
        }
        public override string GetInformationFigure()
        {
            return IsCreate
                    ? $"Тип фигуры: {TypeFigure}\n"
                    + $"Левая сторона: {Math.Round(LeftSide, 2)}см\n"
                    + $"Правая сторона: {Math.Round(RightSide, 2)}см\n"
                    + $"Основание: {Math.Round(BaseSide, 2)}см\n"
                    + $"Тип треугольника: {_typeRectangle}\n"
                    + $"Периметр: {Math.Round(PerimetrFigure, 2)}см\n"
                    + $"Площадь: {Math.Round(AreaFigure, 2)}см в квадрате\n"
                    : "Фигура не была создана.\n";
        }
        public enum Rectangle
        {
            [Display(Name = "Равносторонний")]
            Equilateral,
            [Display(Name = "Равнобедренный")]
            Isosceles,
            [Display(Name = "Разносторонний")]
            Versatile
        }
    }
    public enum Figure
    {
        [Display(Name = "Квадрат")]
        Square,
        [Display(Name = "Прямоугольник")]
        Rectangle,
        [Display(Name = "Треугольник")]
        Trinagle,
        [Display(Name = "Круг")]
        Round
    }
}
