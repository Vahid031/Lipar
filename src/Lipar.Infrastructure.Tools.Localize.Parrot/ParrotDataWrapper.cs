using System;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Tools.Localize.Parrot;

public abstract class ParrotDataWrapper : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        await this.DisposeAsync();
    }

    public abstract string Get(string key, string culture);

}


