using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    /// <summary>
    /// variables globales que son objetos que utilizaremos en el proceso
    /// </summary>
    
    public Button playAu;
    public Button stopAu;
    public InputField desLugar;
    public AudioSource audio;
    public string description;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScore()); // llamamos al metodo para llenar en inputText
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// accionamos el boton, para activar el audio
    /// </summary>
    public void audioplay()
    {
        StartCoroutine(StarAudio()); //llamamos al metodo de reproducir audio
        stopAu.gameObject.SetActive(true);
        playAu.gameObject.SetActive(false);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns> un string q sera leido por el audio source</returns>
    IEnumerator StarAudio()
    {        
        // Remove the "spaces" in excess
        Regex rgx = new Regex("\\s+");
        // Replace the "spaces" with "% 20" for the link Can be interpreted
        string lugarDes = desLugar.text;
        Debug.Log("" + lugarDes);
        string result = rgx.Replace(lugarDes, "%20");
        string url = "http://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=1024&client=tw-ob&q=+" + result + "&tl=Es-es";
        WWW wwwAudio = new WWW(url);
        yield return wwwAudio;
        audio.clip = wwwAudio.GetAudioClip(false, false, AudioType.MPEG);
        Debug.Log(url);

        if (audio != null && !audio.isPlaying && audio.clip.isReadyToPlay)
        {
            audio.Play();
        }
    }

    /// <summary>
    /// Metodo para cargar el Imput mediante una consulta al servidor
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadScore()
    {
        string des = "";
        string idTouristSpot = PlayerPrefs.GetString("SpotID");
        WWWForm form = new WWWForm();       
        form.AddField("idTuristSpot", idTouristSpot);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/descriptionSpots.php", form);
        yield return www;
        des = www.text;
        desLugar.text = des;     
    }


    /// <summary>
    /// metodo para parar el audio
    /// </summary>
    public void stopAudio()
    {
        audio.Stop();
        stopAu.gameObject.SetActive(false);
        playAu.gameObject.SetActive(true);
    }
}
