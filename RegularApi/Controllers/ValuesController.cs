using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RegularApi.RabbitMq.Templates;

namespace RegularApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get([FromServices] IRabbitMqTemplate rabbitTemplate)
        {
            rabbitTemplate.SendMessage(exchange: "regular-deployer-exchange",
                queue: "com.dorefactor.deploy.command", 
                message: "hola mundo!");

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
