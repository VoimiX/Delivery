using CSharpFunctionalExtensions;

namespace DeliveryApp.Core.Domain.SharedKernel;

public class Weight : ValueObject
{
    public Weight(int kilograms)
    {
        if (kilograms <= 0) 
            throw new ArgumentOutOfRangeException($"Параметр {nameof(kilograms)} не может быть равен 0 или меньше нуля");

        Kilograms = kilograms;
    }

    private Weight()
    {
    }

    public int Kilograms { get; }

    public static bool operator > (Weight a, Weight b) => a.Kilograms > b.Kilograms;
    public static bool operator < (Weight a, Weight b) => a.Kilograms < b.Kilograms;
    public static bool operator >= (Weight a, Weight b) => a.Kilograms >= b.Kilograms;
    public static bool operator <=(Weight a, Weight b) => a.Kilograms <= b.Kilograms;   

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Kilograms;
    }
}
