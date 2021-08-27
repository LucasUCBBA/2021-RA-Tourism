using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(Calification_Button_Click);
    }

    // Update is called once per frame
    public void Calification_Button_Click()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }
}