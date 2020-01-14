using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Demo.Data;
using Demo.Models;
using Demo.Servics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Demo.WebApis
{
    [Route("api/[controller]")]
    public class DeviceController : Controller
    {
        public readonly IMqttService _mqttService;
        public DeviceController(IMqttService mqttService)
        {
            _mqttService = mqttService;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        // GET api/<controller>/5
        [HttpGet("GetSensorValue")]
        public JsonResult GetSensorValue()
        {
            return new JsonResult(StorageSingleton.Instance.DeviceData);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<JsonResult> PostAsync([FromBody]UserRequest operation)
        {
            if (operation.dev == "Mote")
            {
                await _mqttService.PublishAsync("/iot/zolertia/cmd", operation.opr);
                if(operation.opr == "off")                
                    StorageSingleton.Instance.DeviceData.IsEnabled = false;
                else
                    StorageSingleton.Instance.DeviceData.IsEnabled = true;

            }
            else
            {
                if (operation.opr == "on")
                    await _mqttService.PublishAsync("/iot/sensor/cmd", "ACTIVATETEMPSEN");
                else
                    await _mqttService.PublishAsync("/iot/sensor/cmd", "DEACTIVATETEMPSEN");
            }

            return new JsonResult(operation);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
