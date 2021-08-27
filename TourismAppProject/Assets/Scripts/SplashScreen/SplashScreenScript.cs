using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenScript : MonoBehaviour
{
    public static int SceneNumber;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneNumber == 0)
        {
            StartCoroutine(ToSplashTwo());
        }
        if (SceneNumber == 1)
        {
            StartCoroutine(ToMainMenu());
        }
        ClassIdHolder.firstTime = false;
    }

    IEnumerator ToSplashTwo()
    {
        yield return new WaitForSeconds(3);
        SceneNumber = 1;
        SceneManager.LoadScene(9);
    }

    IEnumerator ToMainMenu()
    {
        yield return new WaitForSeconds(3);
        SceneNumber = 0;
        SceneManager.LoadScene(0);
    }
}