using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.Adapters.Postgres.EntityConfigurations.CourierAggregate
{
    class TransportEntityTypeConfiguration : IEntityTypeConfiguration<Transport>
    {
        public void Configure(EntityTypeBuilder<Transport> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("transports");

            entityTypeBuilder.HasKey(entity => entity.Id);

            entityTypeBuilder
                .Property(entity => entity.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();         

            entityTypeBuilder
                .Property(entity => entity.Speed)
                .HasColumnName("speed")
                .IsRequired();

            entityTypeBuilder
                .Property(entity => entity.Capacity)
                .HasColumnName("capacity")
                .IsRequired()
                .HasConversion(x => x.Kilograms, x => new Weight(x));

            entityTypeBuilder
                .Property(entity => entity.Type)
                .HasColumnName("type")
                .IsRequired()
                .HasConversion(x => x.ToString(), x => Enum.Parse<Transport.TransportType>(x));

            entityTypeBuilder.HasData(Transport.All);
        }
    }
}