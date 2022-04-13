using System.Diagnostics.CodeAnalysis;

namespace Tederean.Apius.Extensions
{

  public static class ExceptionExtensions
  {

    public static void ThrowIfEmptyOrWhiteSpace([NotNull] this string? value, string name)
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        throw new ArgumentException("String cannot be null, empty or whitespace.", name);
      }
    }

    public static void ThrowIfNull([NotNull] this object? value, string name)
    {
      if (value is null)
      {
        throw new ArgumentNullException(name);
      }
    }
  }
}
