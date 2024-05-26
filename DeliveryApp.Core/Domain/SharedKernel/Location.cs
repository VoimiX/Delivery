
using CSharpFunctionalExtensions;

namespace DeliveryApp.Core.Domain.SharedKernel;

public class Location : ValueObject
{
    public Location(int x, int y)
    {
        const int min = 1;
        const int max = 10;

        if (x < min || x > max)
            throw new ArgumentOutOfRangeException($"Параметр {nameof(x)} должен быть в переделах от {min} до {max}");

        if (y < min || y > max)
            throw new ArgumentOutOfRangeException($"Параметр {nameof(y)} должен быть в переделах от {min} до {max}");

        X = x;
        Y = y;
    }

    private Location()
    {
    }

    public int X { get; }
    public int Y { get; }

    public int DistanceTo(Location otherLocation)
    {
        if (otherLocation == null)
            throw new ArgumentNullException(nameof(otherLocation));

        return Math.Abs(X - otherLocation.X) + Math.Abs(Y - otherLocation.Y);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}
