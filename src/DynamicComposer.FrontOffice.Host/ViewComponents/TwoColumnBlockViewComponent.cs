using DynamicComposer.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DynamicComposer.FrontOffice.Host.ViewComponents;

public sealed class TwoColumnBlockViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(TwoColumnBlockDto block) => View(block);
}
