using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ARScript : MonoBehaviour
{
    public void Return_To_Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Go_To_Media()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(7);
    }

    public void Go_To_Validation()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }
}