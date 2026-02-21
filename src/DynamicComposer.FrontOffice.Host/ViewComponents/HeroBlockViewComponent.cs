using DynamicComposer.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DynamicComposer.FrontOffice.Host.ViewComponents;

public sealed class HeroBlockViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(HeroBlockDto block) => View(block);
}
