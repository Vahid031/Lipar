using Lipar.Infrastructure.Tools.Localize.Parrot.SqlServer;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.AspNetCore.Http;

namespace Lipar.Infrastructure.Tools.Localize.Parrot.Mongo;

public class MongoParrotTranslator : ParrotTranslator
{
    public MongoParrotTranslator(LiparOptions liparOptions, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _localizer = MongoParrotDataWrapper.CreateFactory(liparOptions);
    }
}
