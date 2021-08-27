using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MediaSceneScript : MonoBehaviour
{
    public Button btnImage, btnVideo,btnReturn;

    // Start is called before the first frame update
    void Start()
    {
        //Asignamos la funcionalidad de los onClicks a los botones para redirigir a las ventas de imagenes, video y mapa
        btnImage.onClick.AddListener(SceneImage);
        btnVideo.onClick.AddListener(SceneVideo);
        btnReturn.onClick.AddListener(SceneMap);
    }

    /// <summary>
    /// Metodo para redireccionar a las ventanas de imagenes, videos y a la ventana principal del mapa
    /// </summary>
    public void SceneImage()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(8);
    }
    public void SceneVideo()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(6);

    }
    public void SceneMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    }
}