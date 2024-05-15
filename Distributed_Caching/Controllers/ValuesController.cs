using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed_Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IDistributedCache _distributedCache;

        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }



         [HttpGet("set")]
        public async Task<IActionResult> set(string name,string surname)
        {
            await _distributedCache.SetStringAsync("name", name, options: new()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(30),
                SlidingExpiration=TimeSpan.FromSeconds(5)
                

            }); ;
        await _distributedCache.SetAsync("surname",Encoding.UTF8.GetBytes(surname));
            return Ok();

        }
        [HttpGet("get")]
        public async Task<IActionResult> get()
        {
          var name =  await _distributedCache.GetStringAsync("name");
          var surnamee= await _distributedCache.GetAsync("surname");
          var surname=Encoding.UTF8.GetString(surnamee);
                      
             return Ok(new
             {
                 name,
                 surname,
             });

        }
    }
}
