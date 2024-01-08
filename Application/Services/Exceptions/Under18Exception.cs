namespace Services.Exceptions
{
    public class Under18Exception : Exception
    {
        public Under18Exception(string message, Exception inner) : base(message, inner) { }
        public Under18Exception(string message) : base(message) { }
        public Under18Exception() { }
    }
}
