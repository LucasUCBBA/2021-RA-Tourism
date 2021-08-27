using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class AlertScript : MonoBehaviour
{
    public double distancia;
    double lat1 = -17.389700, lat2 = -17.393534, long1 = -66.155487, long2 = -66.145418;
    public List<Tuple<double, double>> spotList = new List<Tuple<double, double>>();
   
    // Start is called before the first frame update
    IEnumerator Start()
    {
        //llamanso a la base de datos
        //obtenemos latitud y longitud de todos los lugares turísticos registrados en la base de datos
        WWW www = new WWW("https://tourismappar.000webhostapp.com/get_spot_lat_long.php");
        yield return www;
        string[] latLongArray = www.text.Split('|'); //separa la latitud y longitud de cada lugar mediante la barra |
        for (int i = 0; i < latLongArray.Length; i += 2)
        {
            //llenamos la lista spotList con las latitudes y longitudes de cada lugar
            spotList.Add(new Tuple<double, double>(double.Parse(latLongArray[i].Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(latLongArray[i + 1].Replace(',', '.'), CultureInfo.InvariantCulture)));
        }
    }
    
    static class DistanceAlgorithm
    {
        const double PIx = 3.141592653589793;
        const double RADIUS = 6378137;//el dato está en metros

        /// <summary>
        /// Convert degrees to Radians
        /// </summary>
        /// <param name="x">Degrees</param>
        /// <returns>The equivalent in radians</returns>
        public static double Radians(double x)
        {
            return x * PIx / 180;
        }

        /// <summary>
        /// Calculate the distance between two places.
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public static double DistanceBetweenPlaces(double lon1, double lat1, double lon2, double lat2)
        {
            double dlon = Radians(lon2 - lon1);
            double dlat = Radians(lat2 - lat1);
            //calculamos la distancia con la fórmula de Haversine
            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * RADIUS;
        }

    }

    public int globalCounter = 0;
    public bool inPlace = false;

    // Update is called once per frame
    void Update()
    {
        int countInPlace = 0;
        foreach (var item in spotList)//va revisando comparando con los lugares tur{isticos si hay uno cercano
        {
            long2 = item.Item2;
            lat2 = item.Item1;
            distancia = DistanceAlgorithm.DistanceBetweenPlaces(long1, lat1, long2, lat2);
           
            if (distancia < 200)//verifica si la distancia del lugar turístico es menos de 200 metros 
            {
                countInPlace++;
                //Verifica si está en el lugar
                if (inPlace == false)
                {
                    Handheld.Vibrate();//Método para que vibre el celular
                    inPlace = true;
                }
            }
        }
        globalCounter = countInPlace;
        //si la persona ya no está en el lugar la bander inPlace vuelve a estar en falso
        if (globalCounter == 0) {
            inPlace = false;
        }
    }
}