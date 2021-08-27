using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCommentsScript : MonoBehaviour
{
    public Text commentMessage;

    /// <summary>
    /// Metodo start lectura de comentarios
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        //Agarra los comentarios de la base de datos 
        WWWForm form = new WWWForm();

        string idTouristSpot = PlayerPrefs.GetString("SpotID"); ;

        form.AddField("TouristSpotID", idTouristSpot);

        WWW www = new WWW("https://tourismappar.000webhostapp.com/get_comment.php", form);
        yield return www;
        string get_comments = "";
        string[] commentsa = www.text.Split('|');
        for (int i = 0; i < commentsa.Length; i++)
        {
            get_comments = get_comments + commentsa[i] + "\n";
        }
        commentMessage.text = get_comments;
    }
}