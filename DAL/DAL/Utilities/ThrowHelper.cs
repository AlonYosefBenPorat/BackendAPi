namespace DAL.Utilities
{
    public static class ThrowHelper
    {
        public static void ThrowArgumentNullException(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        public static void ThrowIfNull<T>(T argument, string paramName) where T : class
        {
            if (argument == null)
            {
                ThrowArgumentNullException(paramName);
            }
        }
    }
}
