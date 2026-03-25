using Microsoft.AspNetCore.Identity;
using SurveyBasket.Abstractions.Consts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SurveyBasket.Persistence.EntitiesConfigurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        var permissions = Permissions.GetAllPermissions(); // deterministic ordered list
        var adminClaims = new List<IdentityRoleClaim<string>>();

        for (int i = 0; i < permissions.Count; i++)
        {
            adminClaims.Add(new IdentityRoleClaim<string>
            {
                Id = i + 1,
                ClaimType = Permissions.Type,
                ClaimValue = permissions[i],
                RoleId = DefaultRoles.Admin.Id
            });
        }

        builder.HasData(adminClaims);
    }
}
