using DynamicComposer.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DynamicComposer.FrontOffice.Host.ViewComponents;

public sealed class ContentBlockViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ContentBlockDto block) => View(block);
}
