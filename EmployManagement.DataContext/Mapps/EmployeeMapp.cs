
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EmployManagementDataBase.Models.Master;

namespace EmployManagementDataBase.Mapps
{
    public class EmployeeMapp : IEntityTypeConfiguration<Employe>
    {

        public void Configure(EntityTypeBuilder<Employe> builder)
        {
            builder.HasKey(x => x.Id);
            builder
                .HasOne(x => x.Depatment)
                .WithMany()
                .HasForeignKey(x => x.DepatmentId)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(x => x.Manager)
                .WithMany()
                .HasForeignKey(x => x.ManagerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
