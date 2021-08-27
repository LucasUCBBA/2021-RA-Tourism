using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UrlManager : MonoBehaviour
{
    //variable para almacenar los links de los videos
    string video1, video2, video3;

    
    //Instanciacion de los botones
    public Button btn1, btn2, btn3;
    // Start is called before the first frame update
    void Start()
    {
        //Obtenemos el id y con el se obtienen los links de los videos del lugar
        StartCoroutine(GetVideoLink(ClassIdHolder.id));
        //Asiganmos los eventos a cada uno de los botones
        btn1.onClick.AddListener(VideoUno);
        btn2.onClick.AddListener(VideoDos);
        btn3.onClick.AddListener(VideoTres);
        
    }
    /// <summary>
    /// Obtenemos los links de la base de datos
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IEnumerator GetVideoLink(int id)

    {

        WWWForm form = new WWWForm();

        form.AddField("id", id);

        WWW www = new WWW("https://tourismappar.000webhostapp.com/get_video_link.php", form);

        yield return www;

        if(www.text != "" || www.text != "nope")
        {
            string[] links=www.text.Split('|');
            video1 = links[0];
            video2 = links[1];
            video3 = links[2];
        }

    }
    /// <summary>
    /// Redireccion al video1
    /// </summary>
    public void VideoUno()
    {
        Application.OpenURL(video1);
    }
    /// <summary>
    /// Redirecciona al video2
    /// </summary>
    public void VideoDos()
    {
        Application.OpenURL(video2);
    }
    /// <summary>
    /// redirecciona al video3
    /// </summary>
    public void VideoTres()
    {
        Application.OpenURL(video3);
    }
   
}
