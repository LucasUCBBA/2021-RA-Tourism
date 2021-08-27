using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu1Script : MonoBehaviour
{
    //Definimos variables
    public Button backButton;
    public float time = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(BackButtonOnClick);
    }

    void BackButtonOnClick()
    {
        //Redirecciona a Home
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void BUTTON_Sub_Menu()
    {
        //Redirecciona a leer comentarios
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);

    }
}
