using Microsoft.AspNetCore.Mvc;

namespace CardManagementApis.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    public class BaseAppController : ControllerBase
    {
    }
}
