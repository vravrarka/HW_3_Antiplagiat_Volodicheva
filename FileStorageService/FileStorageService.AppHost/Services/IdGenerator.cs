using System.Runtime.InteropServices;

public static class IdGenerator
{
    private static int _current = 0;

    public static int NextId()
    {
        return Interlocked.Increment(ref _current);
    }
}