using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

using System.IO;
using System.Text;


namespace Alert_Light
{
    public class Program
    {
        public static void Main()
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream dataStream = null;
            StreamReader reader = null;

            OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);
            OutputPort lights = new OutputPort(Pins.GPIO_PIN_D0, false);
            InputPort button = new InputPort(Pins.ONBOARD_BTN, false, Port.ResistorMode.Disabled);

            lights.Write(true);
            Thread.Sleep(500);
            lights.Write(false);
            Thread.Sleep(500);
            lights.Write(true);
            Thread.Sleep(500);
            lights.Write(false);
            Thread.Sleep(500);

            while (true)
            {
                if (GetData("http://alert-light.azurewebsites.net/status", request, response, dataStream, reader).Equals("1"))
                {
                    lights.Write(true);
                }
                else
                {
                    lights.Write(false);
                }
                Thread.Sleep(3000);
            }
        }

        public static string GetData(string url, WebRequest rec, WebResponse resp, Stream stream, StreamReader stRe)
        {
            rec = WebRequest.Create(url);
            resp = rec.GetResponse();//if called on too often, may stop working
            stream = resp.GetResponseStream();
            stRe = new StreamReader(stream);
            string answer = stRe.ReadToEnd();

            resp.Close();//these might be closing after the method ends (unnessesary?)
            stream.Close();
            stRe.Close();

            return answer;

        }

    }
}
