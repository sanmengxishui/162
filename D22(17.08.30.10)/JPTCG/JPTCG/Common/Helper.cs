using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JPTCG.Common
{
    class Helper
    {
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(
            typeof(DescriptionAttribute),
            false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static double GetRadianAngleBetween(DPoint pt1, DPoint pt2)
        {
            double ydiff = pt2.Y - pt1.Y;
            double xdiff = pt2.X - pt1.X;
            return Math.Atan2(ydiff, xdiff);
        }
        public static float GetDegreeFromRadian(float radian)
        {
            return 180f * radian / 3.14159f;
        }

        public static void ApplyRotation(ref System.Drawing.PointF pt, float angle, System.Drawing.PointF center)
        {
            float scale = 1;
            double tempX = pt.X - center.X;
            double tempY = pt.Y - center.Y;
            pt.X -= center.X;
            pt.Y -= center.Y;

            double cosf = Math.Cos(angle);
            double sinf = Math.Sin(angle);

            pt.X = (float)((tempX * cosf) - (tempY * sinf));
            pt.X *= scale;

            pt.Y = (float)((tempY * cosf) + (tempX * sinf));
            pt.Y *= scale;

            pt.Y += center.Y;
            pt.X += center.X;
        }

        public static float GetRadianFromDegree(float deg)
        {
            //if (deg > 90) deg = 180 - deg;
            //if (deg <= -180 && deg <= -90) deg = 180 + deg;
            return 3.14159f * deg / 180f;
        }

        public static List<string> GetAllComPortNumber()
        {
            List<string> comPortNum = new List<string>();

            string[] availablePorts = new string[] { };
            availablePorts = SerialPort.GetPortNames();

            if (availablePorts.Length <= 0) return comPortNum;

            //int num = 0; string numStr = "";
            for (int i = 0; i < availablePorts.Length; i++)
            {
                comPortNum.Add(availablePorts[i]);
                //numStr = availablePorts[i].ToUpper().Replace("COM", "");
                //if (int.TryParse(numStr, out num))
                //    comPortNum.Add(num);
            }

            return comPortNum;
        }

        public static string LocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }
    }
}
