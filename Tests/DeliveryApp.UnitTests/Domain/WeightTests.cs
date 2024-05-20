using System;
using DeliveryApp.Core.Domain.SharedKernel;
using Xunit;

namespace DeliveryApp.UnitTests.Domain;

public class WeightTests
{
    [Fact]
    public void Test_weight_equality_when_equal()
    {
        Weight weight1 = new Weight(kilograms: 20);
        Weight weight2 = new Weight(kilograms: 20);

        Assert.True(weight1.Equals(weight2));
    }

    [Fact]
    public void Test_weight_equality_when_equal2()
    {
        Weight weight1 = new Weight(kilograms: 20);
        Weight weight2 = new Weight(kilograms: 20);

        Assert.True(weight1 == weight2);
    }

    [Fact]
    public void Test_weight_equality_when_not_equal()
    {
        Weight weight1 = new Weight(kilograms: 20);
        Weight weight2 = new Weight(kilograms: 15);

        Assert.False(weight1.Equals(weight2));
    }

    [Fact]
    public void Test_weight_less_than()
    {
        Weight weight1 = new Weight(kilograms: 10);
        Weight weight2 = new Weight(kilograms: 12);

        Assert.True(weight1 < weight2);
    }

    [Fact]
    public void Test_weight_greater_than()
    {
        Weight weight1 = new Weight(kilograms: 15);
        Weight weight2 = new Weight(kilograms: 10);

        Assert.True(weight1 > weight2);
    }

    [Fact]
    public void Test_weight_incorrect_param()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Weight(kilograms: 0));
    }
}