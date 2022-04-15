namespace Tederean.Apius.Extensions
{

  public static class MathExtensions
  {

    public static double Map(this double inputValue, double inputMinimum, double inputMaximum, double outputMinimum, double outputMaximum)
    {
      return (inputValue - inputMinimum) * (outputMaximum - outputMinimum) / (inputMaximum - inputMinimum) + outputMinimum;
    }

    public static int Clamp(this int inputValue, int minimum, int maximum)
    {
      if (inputValue < minimum)
        return minimum;

      if (inputValue > maximum)
        return maximum;

      return inputValue;
    }
  }
}
