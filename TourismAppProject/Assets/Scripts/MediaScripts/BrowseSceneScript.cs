using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrowseSceneScript : MonoBehaviour
{
    public string image;
    public string video;
    public string principal;

    /// <summary>
    /// Metodo para cargar la ventana de imagnes, video y principal de opciones
    /// </summary>
    public void LoadSceneImage()
    {
        SceneManager.LoadScene(image);
    }
    public void LoadSceneVideo()
    {
        SceneManager.LoadScene(video);
    }
    public void LoadScenePrincipal()
    {
        SceneManager.LoadScene(principal);
    }
}
