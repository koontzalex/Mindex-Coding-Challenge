using CodeChallenge.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeChallenge.Config
{
    public static class WebApplicationBuilderExt
    {
        public static void UseEmployeeInfoDB(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<EmployeeInfoContext>(options =>
            {
                options.UseInMemoryDatabase(EmployeeInfoContext.EMPLOYEE_INFO_DB_NAME);
            });
        }
    }
}
