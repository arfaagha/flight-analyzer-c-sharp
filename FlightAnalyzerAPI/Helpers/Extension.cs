namespace FlightAnalyzerAPI.Helpers
{
    public static class Extension
    {
        public static T ChangeType<T>(this object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}
