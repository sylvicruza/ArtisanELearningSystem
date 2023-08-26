namespace ArtisanELearningSystem.Exceptions
{
    public class EnrollmentException : Exception
    {
        public EnrollmentException() : base() { }

        public EnrollmentException(string message) : base(message) { }

        public EnrollmentException(string message, Exception innerException) : base(message, innerException) { }
    }
}
