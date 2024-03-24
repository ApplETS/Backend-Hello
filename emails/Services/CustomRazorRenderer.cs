using FluentEmail.Core.Interfaces;
using FluentEmail.Razor;
using RazorLight;
using System.Reflection;

namespace api.emails.Services;

public class CustomRazorRenderer : ITemplateRenderer
{
    private readonly RazorLightEngine _engine;

    public CustomRazorRenderer(string root = null)
    {
        _engine = new RazorLightEngineBuilder()
            .SetOperatingAssembly(Assembly.GetCallingAssembly())
            .UseEmbeddedResourcesProject(typeof(EmailsUtils).Assembly, "api.emails.Views")
            .UseMemoryCachingProvider()
            .Build();
    }

    public Task<string> ParseAsync<T>(string template, T model, bool isHtml = true)
    {
        dynamic? viewBag = (model as IViewBagModel)?.ViewBag;
        return _engine.CompileRenderStringAsync<T>(RazorRenderer.GetHashString(template), template, model, viewBag);
    }

    string ITemplateRenderer.Parse<T>(string template, T model, bool isHtml)
    {
        return ParseAsync(template, model, isHtml).GetAwaiter().GetResult();
    }

}
