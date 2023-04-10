using Microsoft.AspNetCore.Mvc;
using core_asp.net.Models;
namespace core_asp.net.Controllers;

[ApiController]
[Route("home")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public ResponseModel GetMessage()
    {
        return new ResponseModel(){
            HttpStatus = 200,
            Message = "Hello world from asp.net core rest api"
        };
    }
}
