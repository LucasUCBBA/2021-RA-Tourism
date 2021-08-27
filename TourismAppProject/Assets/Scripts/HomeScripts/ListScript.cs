using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ListScript : MonoBehaviour
{
    #region Variables
    public GameObject g;
    string idString;
    #endregion

    // Start is called before the first frame update  
    void Start() 
    {
        //Activa el metodo places()
        StartCoroutine(Places());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //Condicional que revisa si el usuario le dio al boton de atras en celular, o escape en PC, para salirse de la ventana
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call<bool>("moveTaskToBack", true);
            }
            else
            {
                Application.Quit();
            }
        }
    }

    /// <summary>
    /// Este metodo se conecta con el host y busca la cantidad de lugares registrados en la base de datos
    /// Luego utlizando un ciclo for genera un objeto con el tipo de variable gameobject para poder generar un boton con los datos encontrados en otra consulta con la base datos
    /// </summary>
    IEnumerator Places()
    {
        WWW www = new WWW("https://tourismappar.000webhostapp.com/spotID.php");
        yield return www;
        GameObject buttonTemplate = transform.GetChild(0).gameObject;
        int quantity = int.Parse(www.text);
        for (int i = 1; i <= quantity; i++)
        {
            g = Instantiate(buttonTemplate, transform);
            g.name = i.ToString();            
            g.AddComponent<ButtonScript>();
            WWWForm form = new WWWForm();
            form.AddField("id", i);
            WWW www1 = new WWW("https://tourismappar.000webhostapp.com/spots.php", form);
            yield return www1;
            idString = www1.text;        
            Debug.Log(www1.text);
            Debug.Log("--- " + g.tag + " ---");
            g.transform.GetChild(0).GetComponent<Text>().text =idString;
        }
        Destroy(buttonTemplate);
    }
}