using DeliveryApp.Core.Domain.CourierAggregate;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain
{
    public class TransportTests
    {
        [Fact]
        public void Test_transport_list()
        {
            Transport.All.Should().NotBeNullOrEmpty();
        }
    }
}
