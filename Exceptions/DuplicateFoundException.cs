namespace ArtisanELearningSystem.Exceptions
{
    public class DuplicateFoundException : Exception
    {
        public DuplicateFoundException() : base() { }

        public DuplicateFoundException(string message) : base(message) { }

        public DuplicateFoundException(string message, Exception innerException) : base(message, innerException) { }

    }
}
