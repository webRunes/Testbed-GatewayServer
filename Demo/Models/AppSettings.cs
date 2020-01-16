using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class AppSettings
    {
        #region AWS configs
        public string AWS_S3_DataStore_Bucket { get; set; }
        public string AWS_S3_DataStore_FolderPath { get; set; }

        public string AWS_S3_SecretsKey { get; set; }
        public string AWS_S3_SecretsSecret { get; set; }
        #endregion

        #region Mqtt configs
        public string MQTT_URL { get; set; }
        public string MQTT_Port { get; set; }
        public bool MQTT_Secure { get; set; }
        public string MQTT_ClientID { get; set; }
        public string MQTT_UserName { get; set; }
        public string MQTT_Pass { get; set; }

    #endregion

    public AppSettings()
    {
        AWS_S3_DataStore_Bucket = "";
    }


        public void SetAppSettings(IConfiguration configuration)
        {
            AWS_S3_DataStore_Bucket = configuration.GetSection("AWS:S3:DataStore:Bucket").Value;
            AWS_S3_DataStore_FolderPath = configuration.GetSection("AWS:S3:DataStore:FolderPath").Value;
            AWS_S3_SecretsKey = configuration.GetSection("AWS:S3:SecretsKey").Value;
            AWS_S3_SecretsSecret = configuration.GetSection("AWS:S3:SecretsSecret").Value;


            MQTT_URL = configuration.GetSection("MQTT:URL").Value;
            MQTT_Port = configuration.GetSection("MQTT:Port").Value;
            MQTT_Secure = Convert.ToBoolean(configuration.GetSection("MQTT:Secure").Value);
            MQTT_ClientID = configuration.GetSection("MQTT:ClientID").Value;
            MQTT_UserName = configuration.GetSection("MQTT:UserName").Value;
            MQTT_Pass = configuration.GetSection("MQTT:Pass").Value;
        }
    }
}
