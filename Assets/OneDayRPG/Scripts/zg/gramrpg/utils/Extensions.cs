using System.Collections.Generic;

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    /// <summary>
    /// Pop item.
    /// </summary>
    public static T PopFirstItem<T>(this IList<T> ts)
    {
        if (ts.Count > 0)
        {
            var item = ts[0];
            ts.RemoveAt(0);
            return item;
        }
        return default(T);
    }

    /// <summary>
    /// Get random element
    /// </summary>
    public static T Random<T>(this IList<T> ts)
    {
        return ts[UnityEngine.Random.Range(0, ts.Count)];
    }
}