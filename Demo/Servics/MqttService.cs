using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Data;
using Demo.Hubs;
using Demo.Models;

namespace Demo.Servics
{
    public interface IMqttService
    {
        public Task SubscribeAsync(string topic, int qos = 1);
        public Task PublishAsync(string topic, string msg);
        public bool IsConnectedAsync();
    }
    public class MqttService : IMqttService
    {
        private string clientId = StorageSingleton.Instance.AppSettings.MQTT_ClientID == "RandomUID" ? Guid.NewGuid().ToString() : StorageSingleton.Instance.AppSettings.MQTT_ClientID;
        private string mqttURI = StorageSingleton.Instance.AppSettings.MQTT_URL;//"54.235.73.25";
        private string mqttUser = StorageSingleton.Instance.AppSettings.MQTT_UserName;//"";
        private string mqttPassword = StorageSingleton.Instance.AppSettings.MQTT_Pass;//"";
        private int mqttPort = Convert.ToInt32(StorageSingleton.Instance.AppSettings.MQTT_Port);//1883;
        private bool mqttSecure = StorageSingleton.Instance.AppSettings.MQTT_Secure;//false;
        private IManagedMqttClient client;
        private readonly IHubContext<DevicesHub> _hubContext;

        public MqttService(IHubContext<DevicesHub> hubContext)
        {
#if true
            _hubContext = hubContext;
            var messageBuilder = new MqttClientOptionsBuilder().WithClientId(clientId)/*.WithCredentials(mqttUser, mqttPassword)*/.WithTcpServer(mqttURI, mqttPort).WithCleanSession();

            var options = mqttSecure
              ? messageBuilder
                .WithTls()
                .Build()
              : messageBuilder
                .Build();

            var managedOptions = new ManagedMqttClientOptionsBuilder().WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
              .WithClientOptions(options)
              .Build();

            client = new MqttFactory().CreateManagedMqttClient();
            client.StartAsync(managedOptions);

            client.UseConnectedHandler(ClientConnectedHandler);
            //client.UseDisconnectedHandler(ClientDisconnectedHandler);
            client.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);
            client.UseApplicationMessageReceivedHandler(ClientMessageReceivedHandler);            
#endif
        }

        private async void ClientMessageReceivedHandler(MqttApplicationMessageReceivedEventArgs arg)
        {
            try
            {
                string topic = arg.ApplicationMessage.Topic;
                if (string.IsNullOrWhiteSpace(topic) == false)
                {
                    string payload = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
                    Console.WriteLine($"Topic: {topic}. Message Received: {payload}");
                    if(topic == "/iot/zolertia/reply")
                    {
                        if(payload == "on\0")
                        {
                            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "/iot/zolertia/replay", "on");
                        }
                        else if(payload == "off\0")
                        {
                            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "/iot/zolertia/replay", "off");
                        }
                    }
                    else if(topic == "/iot/sensor/reply")
                    {
                        if(payload.Contains("Activated_bme280_temperature_sensor"))
                            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "/iot/sensor/replay", "on");
                        else if(payload.Contains("Deactivated_bme280_temperature_sensor"))
                            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "/iot/sensor/replay", "off");
                    }
                    else if (topic == "/iot/zolertia/data")
                    {
                        if(payload == "{serialerror}\0")
                        {

                        }
                        else
                        {
                            StorageSingleton.Instance.DeviceData = JsonConvert.DeserializeObject<DeviceData>(payload);
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }            
        }

        private void ClientDisconnectedHandler(MqttClientDisconnectedEventArgs arg)
        {
            Console.WriteLine("Disconnected from MQTT Brokers.");            
        }
        private void OnConnectingFailed(ManagedProcessFailedEventArgs e)
        {
            Console.WriteLine("OnConnectingFailed from MQTT Brokers.");
        }
        private void ClientConnectedHandler(MqttClientConnectedEventArgs arg)
        {
            Console.WriteLine("Connected successfully with MQTT Brokers.");
            _ = SubscribeAsync("/iot/zolertia/reply", 1);
            _ = SubscribeAsync("/iot/sensor/reply", 1);
            _ = SubscribeAsync("/iot/zolertia/data", 1);
        }

        public bool IsConnectedAsync()
        {
            return client.IsConnected;
        }
        public async Task SubscribeAsync(string topic, int qos = 1)
        {
            await client.SubscribeAsync(new TopicFilterBuilder()
            .WithTopic(topic)
            .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
            .Build());
        }

        public async Task PublishAsync(string topic,string msg)
        {
            await client.PublishAsync(topic, msg);
        }

    }
}
