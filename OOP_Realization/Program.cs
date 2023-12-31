namespace OOP_Realization
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BaseFigure[] figures = new BaseFigure[]
            {
                new Square("6,3"),
                new Trinagle("12,2, 5,6, 3"),
                new Trinagle("9, 9, 9"),
                new Round("15,3"),
                new Square("10"),
                new Rectangle("3,4, 6,2")
            };
            
            foreach (var figure in figures)
            {
                try
                {
                    figure.Create();
                } 
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally 
                {
                    Console.WriteLine(figure.GetInformationFigure());
                }
            }
        }
    }
}