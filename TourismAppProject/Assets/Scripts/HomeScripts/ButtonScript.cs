using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    //Inicio de script para metodos    
    public NavegateScript navegationScrip;
    public MoreOptionsScript moreOption;

    //Objetos del menu para realizar sus respectivos vizualisaciones segun el requerimiento
    public RectTransform subMenuPlaceOptions;
    public RectTransform buttonNategation;
    public RectTransform logoApp;
    public RectTransform buttonAr;

    public InputField textoLat;
    public InputField textoLog;

    private string urlMap = "";

    public int zomt = 16;
    public RawImage imgmap;
    public string latitudePlace = "", longitudePlace = "", lat, lon, latP = "", lonP = "", latUser, logUser;

    void Start()
    {
        //Verifica que vuelve de la ventana MediaPrincipal para ubicarse en el mapa
        if (ClassIdHolder.id != 0 && imgmap != null && navegationScrip !=  null)
        {
            //Obtiene el id de la clase para dar la ubicacion correspondiente
            StartCoroutine(Place_Location(ClassIdHolder.id.ToString()));
        }
        if (ClassIdHolder.firstTime)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(10);
        }
    }

    /// <summary>
    /// Con este metodo estamos capturando el nombre del boton, el cual nos permite accionar el metodo del la ubicion del lugar seleccionado
    /// </summary>
    public void Enviar_ID()
    {
        string id = this.gameObject.name; // capturamos el nombre del Boton
        Debug.Log("--- " + id + " ---");
        StartCoroutine(Place_Location(id)); // enviamos el parametro al metodo que muestra el lugar turistico

        moreOption.Sub_Menu_Button();
        //hola, soy rodri. había un conflicto acá y no sabía con cual de estas dos líneas de código dejarle, por ende, deje los dos:
        PlayerPrefs.SetString("SpotID", id);
        ClassIdHolder.id = int.Parse(id);
    }

    /// <summary>
    /// PlaceUbication, es un metodo que permite recoger el Id del Lugar y hacerla consulta para obtener la latitud y longitud, para psteriormente ejecutar la orden
    /// </summary>
    /// <param name="val">recive el nombre de boton, que en este caso es el Id de lugar turistico</param>
    /// <returns></returns>
    IEnumerator Place_Location(string val)
    {
        WWWForm formLat = new WWWForm();
        WWWForm formLon = new WWWForm();
        formLat.AddField("id", val);
        formLon.AddField("id", val);
        WWW www1 = new WWW("https://tourismappar.000webhostapp.com/spotsLatitude.php", formLat);
        WWW www2 = new WWW("https://tourismappar.000webhostapp.com/spotsLongitude.php", formLon);
        yield return www1;
        lat = www1.text;
        Debug.Log(www1.text);
        yield return www2;
        lon = www2.text;
        Debug.Log(www2.text);

        navegationScrip.latt = lat; // envia los parametros del lugar al sript NavegationScrip, y de esta manera captura y envia para usalor en el metodo GetPlace que te redirecciona al lugar desde la localizacion del Usuario
        navegationScrip.logg = lon; // envia los parametros del lugar al sript NavegationScrip, y de esta manera captura y envia para usalor en el metodo GetPlace que te redirecciona al lugar desde la localizacion del Usuario

            
        StartCoroutine(Get_Map_Ubi(lat, lon, zomt)); //enviamos los datos obtenidos de la Latitud y la Longitud        
        subMenuPlaceOptions.gameObject.SetActive(true);
        buttonNategation.gameObject.SetActive(false);
        logoApp.gameObject.SetActive(false);
        buttonAr.gameObject.SetActive(true);
        lonP = www2.text;
        latP = www1.text;
        textoLat.text = lonP;
        textoLog.text = latP;
    }       
  
    /// <summary>
    /// Metodos para el zoom pero solo del lugar en especifico seleccionado
    /// </summary>
    public void Zoom_In_To_Place()
    {
        zomt++;
        string txtla,txtlo;
        txtla = textoLat.text;
        txtlo = textoLog.text;
        StartCoroutine(Get_Map_Ubi(txtlo, txtla, zomt));
    }
    public void Zoom_Out_Of_Place()
    {
        zomt--;
        string txtla, txtlo;
        txtla = textoLat.text;
        txtlo = textoLog.text;    
        StartCoroutine(Get_Map_Ubi(txtlo, txtla, zomt));
    }

    /// <summary>
    /// GetMapUbi, nos permitira mostrar la ubicaion especifica del lugar seleccionado
    /// </summary>
    /// <param name="lat">Parametro del lugar especifico obtenidos de la base de datos</param>
    /// <param name="log">Parametro del lugar especifico obtenidos de la base de datos</param>
    /// <returns></returns>
    IEnumerator Get_Map_Ubi(string lat, string log, int zom)
    {
        Input.location.Start();
        urlMap = "https://maps.geoapify.com/v1/staticmap?style=osm-carto&width=600&height=400&center=lonlat:" + log + "," + lat + "&zoom="+zom+"&marker=lonlat:" + log + "," + lat + ";type:awesome;color:red;size:x-large;icon:landmark&apiKey=69ddfb1f86844681a87f152ead49a07e";
        WWW www = new WWW(urlMap);
        yield return www;
        imgmap.texture = www.texture;       
    }

    
}