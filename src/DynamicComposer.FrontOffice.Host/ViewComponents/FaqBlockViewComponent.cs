using DynamicComposer.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DynamicComposer.FrontOffice.Host.ViewComponents;

public sealed class FaqBlockViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(FaqBlockDto block) => View(block);
}
