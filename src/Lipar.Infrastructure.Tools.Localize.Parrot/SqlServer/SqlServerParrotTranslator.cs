using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.AspNetCore.Http;

namespace Lipar.Infrastructure.Tools.Localize.Parrot.SqlServer;

public class SqlServerParrotTranslator : ParrotTranslator
{
    public SqlServerParrotTranslator(LiparOptions liparOptions, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _localizer = SqlServerParrotDataWrapper.CreateFactory(liparOptions);
    }
}
