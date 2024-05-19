using System.Collections.Generic;
using DeliveryApp.Core.Domain.SharedKernel;
using Xunit;

namespace DeliveryApp.UnitTests.Domain;

public class LocationTests
{
    [Theory]
    [MemberData(nameof(Data))]
    public void TestLocationCalculation(Location a, Location b, int expectedDistance)
    {
        Assert.True(a.CalculateDistance(b) == expectedDistance);
    }

    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new Location(1, 1), new Location(2, 2), 2 },
            new object[] { new Location(1, 1), new Location(1, 1), 0 },
            new object[] { new Location(1, 4), new Location(1, 3), 1 }
        };
}
