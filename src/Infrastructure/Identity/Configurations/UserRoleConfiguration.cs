using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "ac8401df-977c-4c0d-8007-8df88460700c",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "b37aadad-1f36-42ef-88da-15b3cb40a238",
                    UserId = "9e224968-33e4-4652-b7b7-8574d048cdb9"
                }
            );
        }
    }
}