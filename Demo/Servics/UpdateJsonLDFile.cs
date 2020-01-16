using Demo.Data;
using Demo.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Servics
{
    interface IUpdateJsonLDFile
    {

    }
    public class UpdateJsonLDFile : IUpdateJsonLDFile
    {
        public UpdateJsonLDFile()
        {
            PerformUpdates();
            Thread t = new Thread(NewThread);
            t.Start();
        }

        public void NewThread()
        {
            bool taskperformed = false;
            while (true)
            {
                if(DateTime.Now.Minute == 0)
                {
                    if (!taskperformed)
                    {                        
                        taskperformed = true;
                        PerformUpdates();
                    }                    
                }
                else
                {
                    if (taskperformed)
                    {
                        taskperformed = false;
                    }
                }
                //PerformUpdates();
                System.Threading.Thread.Sleep(1000 * 58);
                
            }
        }

        void PerformUpdates()
        {
            string text;
            Console.WriteLine("Going to perform Updates on S3");
            //AmazonService.UploadFileAsync();
            AmazonService.DownloadFile();
            var fileStream = new FileStream(AmazonService.GetSensorFileLocalPath(), FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }

            var jsonldstrmatched = StringUtil.getBetween(text, "<script type=\"application/ld+json\">", "</script>");

            if (jsonldstrmatched.stringMatched != "")
            {
                var cur = DateTime.Now.ToString("yyyy-MM-ddT:HH:mm:ss");
                var jstr = JsonLD.Util.JSONUtils.FromString(jsonldstrmatched.stringMatched);
                if (StorageSingleton.Instance.DeviceData != null)
                {
                    string tempnow = StorageSingleton.Instance.DeviceData.Temperature.ToString();
                    string baterynow = StorageSingleton.Instance.DeviceData.BatteryVal.ToString();
                    string ModeSet = "";
                    if (StorageSingleton.Instance.DeviceData.IsEnabled)
                    {
                        ModeSet = "Enable";
                        tempnow = ((Convert.ToDecimal(tempnow)) / 1000).ToString();
                        baterynow = ((Convert.ToDecimal(baterynow)) / 1000).ToString();
                    }
                    else
                    {
                        tempnow = "0.0";
                        baterynow = "0.0";
                        ModeSet = "Disable";
                    }                    
                        
                    DateTime jsonnow = DateTime.ParseExact(jstr.ElementAt(7).ElementAt(0).ElementAt(0).ElementAt(1).ElementAt(0).ElementAt(23).ToString(), "yyyy-MM-ddT:HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    if (!(jsonnow.Year == DateTime.Now.Year && jsonnow.Month == DateTime.Now.Month && jsonnow.Day == DateTime.Now.Day && jsonnow.Hour == DateTime.Now.Hour))
                    {

                        jstr.ElementAt(7).ElementAt(0).ElementAt(0).ElementAt(1).ElementAt(0).ElementAt(0).Remove();
                        jstr.ElementAt(7).ElementAt(0).ElementAt(0).ElementAt(1).ElementAt(0).ElementAt(22).AddAfterSelf(cur);

                        jstr.ElementAt(7).ElementAt(0).ElementAt(0).ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(0).ElementAt(3).ElementAt(0).ElementAt(0).Remove();
                        jstr.ElementAt(7).ElementAt(0).ElementAt(0).ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(0).ElementAt(3).ElementAt(0).ElementAt(22).AddAfterSelf(baterynow);




                        jstr.ElementAt(7).ElementAt(0).ElementAt(1).ElementAt(1).ElementAt(0).ElementAt(0).Remove();
                        jstr.ElementAt(7).ElementAt(0).ElementAt(1).ElementAt(1).ElementAt(0).ElementAt(22).AddAfterSelf(cur);

                        jstr.ElementAt(7).ElementAt(0).ElementAt(1).ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(0).ElementAt(3).ElementAt(0).ElementAt(0).Remove();
                        jstr.ElementAt(7).ElementAt(0).ElementAt(1).ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(0).ElementAt(3).ElementAt(0).ElementAt(22).AddAfterSelf(tempnow);




                        jstr.ElementAt(7).ElementAt(0).ElementAt(2).ElementAt(1).ElementAt(0).ElementAt(0).Remove();
                        jstr.ElementAt(7).ElementAt(0).ElementAt(2).ElementAt(1).ElementAt(0).ElementAt(22).AddAfterSelf(cur);

                        jstr.ElementAt(7).ElementAt(0).ElementAt(2).ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(0).ElementAt(2).ElementAt(0).ElementAt(0).Remove();
                        jstr.ElementAt(7).ElementAt(0).ElementAt(2).ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(0).ElementAt(2).ElementAt(0).ElementAt(22).AddAfterSelf(ModeSet);
                        //var str = jstr.ElementAt(7).ElementAt(0).ElementAt(2).ElementAt(1).ElementAt(0);


                        string jsonldstr = JsonLD.Util.JSONUtils.ToPrettyString(jstr);

                        string htmlStr = jsonldstrmatched.beforeStringMatched + jsonldstr + jsonldstrmatched.afterStringMatched;

                        using (StreamWriter writer = new StreamWriter(AmazonService.GetSensorFileLocalPath()))
                        {
                            writer.Write(htmlStr);
                        }



                        AmazonService.UploadFile();
                    }
                }

            }

        }
    }
}
