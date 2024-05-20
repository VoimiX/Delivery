namespace DeliveryApp.Core.Domain.SharedKernel;

public record Weight
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
}
