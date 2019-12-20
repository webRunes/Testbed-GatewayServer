using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Demo.Models;
using Demo.Servics;

namespace Demo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public readonly IMqttService _mqttService;
                

        public IndexModel(ILogger<IndexModel> logger, IMqttService mqttService)
        {
            _logger = logger;
            _mqttService = mqttService;
        }

        public void OnGet()
        {

        }

        public async Task<ActionResult> OnPostPostOperationAsync([FromBody] UserRequest operation)
        {
            if(operation.dev == "Mote")
                await _mqttService.PublishAsync("/iot/zolertia/cmd", operation.opr);
            else
            {
                if(operation.opr == "on")
                    await _mqttService.PublishAsync("/iot/sensor/cmd", "ACTIVATETEMPSEN");
                else
                    await _mqttService.PublishAsync("/iot/sensor/cmd", "DEACTIVATETEMPSEN");
            }
                
            return new JsonResult("Ok");
        }
    }
}
