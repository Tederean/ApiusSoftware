namespace Tederean.Apius.Extensions
{

  public static class EnumerableExtensions
  {

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable) where T : class
    {
      return enumerable.Where(entry => entry is not null)!;
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable) where T : struct
    {
      return enumerable.Where(entry => entry.HasValue).Select(entry => entry!.Value);
    }
  }
}
