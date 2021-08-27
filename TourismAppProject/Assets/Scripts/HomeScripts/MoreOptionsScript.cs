using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoreOptionsScript : MonoBehaviour
{
    /// <summary>
    /// Instanciamos todas las variables necesarias para poder trabajar con los maspas
    /// </summary>
    #region    Variables y Objetos para el uso de los menus
    public RectTransform subMenu; // en este objeto se descargara el mapa
    public RectTransform subMenuPlaceOptions; // en este se descargara el mapa por lugar específico
    #endregion

    float posFinal;
    bool openMenu = true;
    public float time = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        posFinal = Screen.width / 2;
        subMenu.position = new Vector3(-posFinal, subMenu.position.y, 0);
    }

    IEnumerator Move(float time, Vector3 posInit, Vector3 posFin)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            subMenu.position = Vector3.Lerp(posInit, posFin, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        subMenu.position = posFin;
    }

    void Move_Menu(float time, Vector3 posInit, Vector3 posFin)
    {
        StartCoroutine(Move(time, posInit, posFin));
    }

    public void Sub_Menu_Button()
    {
        int sign = 1;
        if (!openMenu)
            sign = -1;

        Move_Menu(time, subMenu.position, new Vector3(sign * posFinal, subMenu.position.y, 0));
        openMenu = !openMenu;
    }

    public void Login_Button()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void AR_Button()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(11);
    }
}