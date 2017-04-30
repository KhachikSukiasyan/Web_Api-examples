using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Windows;


namespace Server.Controllers
{

    public class MainController : ApiController
    {
        // GET: api/Main
        public List<Point> Get(string funcName, double A, double B)
        {
            List<Point> result = new List<Point>();

            double syntheticX = 0.8;
            double syntheticY = 6;

            double algA = -60;
            double algB = 60;
            double algDx = (algB - algA) / 1000;

            Point tempPoint = new Point();

            if (funcName == "Sin")
            {
                for (double i = algA; i < algB; i += algDx)
                {
                    tempPoint.X = i;
                    tempPoint.Y = Math.Sin(i * 1 / A * syntheticX) * B * syntheticY;
                    result.Add(tempPoint);
                }
            }
            else
              if (funcName == "Cos")
             {
                 for (double i = algA; i < algB; i += algDx)
                 {
                    tempPoint.X = i;
                    tempPoint.Y = Math.Cos(i * 1 / A * syntheticX) * B * syntheticY;
                    result.Add(tempPoint);
                 }
             }
            return result;
        }
    }
}
