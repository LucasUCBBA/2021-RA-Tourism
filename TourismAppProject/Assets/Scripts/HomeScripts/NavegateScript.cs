using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavegateScript : MonoBehaviour
{
    //variables para trabajar con los mapas 
    private string urlMap = ""; //recibira el linck con la API del mapa
    public RawImage imgmap; //contenedor para alojar el Mapa
    public RectTransform logoApp; // logo de la app
    public RectTransform buttonAr; // boton para entrar al AR

    //carialbles para el manejo de los ampas en su diferentes ejes
    public int zoom = 10;
    public double ejeY = 0;
    public double ejeX = 0;
    public double resEjeX = 0;
    public double resEjeY = 0;
    public string latt, logg, latUser, logUser;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Get_Map"); // llama para la ejecucion del metodo que traue los recursos del mapa
    }

    public void Zoom_In_Button()
    {
        zoom++;
        StartCoroutine("Get_Map");
    }

    public void Zoom_Out_Button()
    {
        if (zoom >= 0)
        {
            zoom--;
        }
        StartCoroutine("Get_Map");
    }

    /// <summary>
    /// Metodos que ejecutan diferentes ordenes segun sus parametros para la 
    /// manipulacion del mapa general
    /// </summary>
    #region  Metodos para el Manejo de los ejes del Mapa
    public void Move_X_Axis_To_Right()
    {
        double res = 0.001;
        ejeX = res + resEjeX;
        resEjeX = ejeX;
        StartCoroutine("Get_Map");
    }
    public void Move_X_Axis_To_Left()
    {
        double res = -0.001;
        ejeX = resEjeX + res;
        resEjeX = ejeX;
        StartCoroutine("Get_Map");
    }
    public void Move_Y_Axis_Top()
    {
        double res = 0.001;
        ejeY = res + resEjeY;
        resEjeY = ejeY;
        StartCoroutine("Get_Map");
    }
    public void Move_Y_Axis_Bottom()
    {
        double res = -0.001;
        ejeY = resEjeY + res;
        resEjeY = ejeY;
        StartCoroutine("Get_Map");
    }
    #endregion


    /// <summary>
    /// Metodo para la captacion de los marcadores y la ubicaion del Usuarion en el Mapa
    /// </summary>
    /// <returns></returns>
    private IEnumerator Get_Map()
    {
        Input.location.Start(); //inicializacion de la ubicacion mediante las coordenas del celular

        string latitud = Input.location.lastData.latitude.ToString();
        latUser = latitud;

        string longitud = Input.location.lastData.longitude.ToString();
        logUser = longitud;
     
        //parametros obtenidos de los metodos de los ejes X , Y para mandar al Mapa estatico
        double pOrigenX = -66.156397;
        double OriX = ejeX + pOrigenX;
        string resOriX = OriX.ToString();
        resOriX = resOriX.Replace(",", ".");

        double pOrigenY = -17.396189;
        double OriY = ejeY + pOrigenY;
        string resOriY = OriY.ToString();
        resOriY = resOriY.Replace(",", ".");
                      
        urlMap = "https://maps.geoapify.com/v1/staticmap?style=osm-carto&width=600&height=400&center=lonlat:" + resOriX + "," + resOriY + "&zoom=" + zoom + "&marker=lonlat:-66.15696907766927,-17.39406057217893;type:awesome;color:red;size:x-large;icon:landmark|" +
        "lonlat:-66.13496497147685,-17.384519040992913;type:awesome;color:red;size:x-large;icon:landmark|lonlat:-66.15579063411896,-17.38824799127599;type:awesome;color:red;size:x-large;icon:landmark|lonlat:" + longitud + "," + latitud + ";type:awesome;color:%2319b8fc;size:large&apiKey=69ddfb1f86844681a87f152ead49a07e";
        WWW www = new WWW(urlMap);
        yield return www;

        imgmap.texture = www.texture;
    }

    /// <summary>
    /// Metodo que captura los latitud y longut de Usuario, tambien recibe del scrip buttonScrip la latitud y longitud del lugar y nos hace una redireccion de como llegar al Lugar
    /// </summary>
    public void Go_To_Place()
    {
        string lt, lg, ltU, lgU;
        lt = latt;
        lg = logg;
        ltU = latUser;
        lgU = logUser;

        Application.OpenURL("https://www.google.com.bo/maps/dir/" + ltU + "," + lgU + "/" + lt + "," + lg + "/@-17.3826693,-66.178493,13.64z/data=!4m5!4m4!1m1!4e1!1m0!3e0?hl=es");
    }

    /// <summary>
    /// Metodo Para regresar al Menu Unicio donde se encuentran todos los Marcadores con la ubicacion del Usiario
    /// </summary>
    public void menuInicio()
    {
        StartCoroutine("Get_Map");
        logoApp.gameObject.SetActive(true);
        buttonAr.gameObject.SetActive(false);
    }
}