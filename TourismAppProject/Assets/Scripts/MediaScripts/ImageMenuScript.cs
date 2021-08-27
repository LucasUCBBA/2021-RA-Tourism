using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageMenuScript : MonoBehaviour
{
    /// <summary>
    /// Instancia de los contenedores de imagenes
    /// </summary>
    public RawImage img1, img2, img3;

    // Start is called before the first frame update
    void Start()
    {
        //Llama a la clase donde se optiene el id del lugar turistico
        StartCoroutine(SetImage(ClassIdHolder.id));
    }
    
    /// <summary>
    /// Asigna las imagenes segun el id haciendo el llamado al Host
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IEnumerator SetImage(int id)
    {
        WWW www1 = new WWW("https://tourismappar.000webhostapp.com/photos/"+id+".1.jpg");
        yield return www1;
        img1.texture= www1.texture;
        WWW www2 = new WWW("https://tourismappar.000webhostapp.com/photos/"+id+".2.jpg");
        yield return www2;
        img2.texture= www2.texture;
        WWW www3 = new WWW("https://tourismappar.000webhostapp.com/photos/"+id+".3.jpg");
        yield return www3;
        img3.texture= www3.texture;
    }
}