using Newtonsoft.Json;
using OpenTracing;

namespace Bridesmaid.Providers.Business.Helpers;

public static class TracerHelper
{
    public static void Log(this ITracer tracer, object value)
    {
        ISpan activeSpan = tracer.ActiveSpan;
        activeSpan?.Log(new List<KeyValuePair<string, object>>()
        {
            new(nameof (value), (object) JsonConvert.SerializeObject(value))
        });
    }
}