using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CommentScript : MonoBehaviour
{
    //Definimos variables
    public InputField commentInput;
    public Button commentButton;
    public Text commentMessage;
    string token, idClient="0";
    // Start is called before the first frame update
    /// <summary>
    /// Start
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        commentButton.onClick.AddListener(CommentButtonOnClick);
        //Hagarramos el id del cliente
        token = PlayerPrefs.GetString("LoginToken");
        WWWForm form = new WWWForm();
        form.AddField("loginToken", token);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/get_idClient.php", form);
        yield return www;
        idClient = www.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// metodo del boton Inserción comentario
    /// </summary>
    void CommentButtonOnClick()
    {
        //Metodo del boton que llama al metodo para insetar el comentario
        if (idClient!="0")
        {
            commentMessage.text = "";
            //Regex rgx = new Regex(@"^[a-zA-Z ]{1,60}$");
            Regex rgx = new Regex(@"^[a-zA-Z0-9\s]*$");

            string comment = commentInput.text, idTouristSpot = PlayerPrefs.GetString("SpotID"); 
            if (rgx.IsMatch(comment))
            {
                StartCoroutine(RegisterComment(comment, idClient, idTouristSpot));
                commentInput.text = "";
            }
            else
            {
                commentMessage.text = "Solo se permiten letras, por favor";
            }
        }
        else
        {
            commentMessage.text = "Debe estar logeado";
        }
        
    }

    /// <summary>
    /// MEtodo RegisterComment
    /// </summary>
    /// <param name="comment"></param>
    /// <param name="idClient"></param>
    /// <param name="idTuristSpot"></param>
    /// <returns></returns>
    IEnumerator RegisterComment(string comment, string idClient, string idTuristSpot)
    {
        // Metodo para insertar comentarios
        WWWForm form = new WWWForm();
        form.AddField("comment", comment);
        form.AddField("idClient", idClient);
        form.AddField("idTuristSpot", idTuristSpot);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/insert_comment.php", form);
        yield return www;
        if (www.text== "Por favor no use lenguaje ofensivo")
        {
            commentMessage.text = www.text;
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(5);
        }

        
    }
}
