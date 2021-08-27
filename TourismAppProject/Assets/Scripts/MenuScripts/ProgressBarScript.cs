using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]

public class ProgressBarScript : MonoBehaviour
{
    /// <summary>
    /// Declaracion de variables de los proguess bar e imagenes
    /// </summary>

    //limite de la barra
    private int maximunPB;  

    //valores de los proguess bar
    private int currentPB1;    
    private int currentPB2;    
    private int currentPB3;    
    private int currentPB4;    
    private int currentPB5;

    //variables para las imagenes
    public Image mask1;
    public Image mask2;
    public Image mask3;
    public Image mask4;
    public Image mask5;

    public Text Calification;
    private double average;

    //almacena los datos recaudados 
    private string Cadena;

    /// <summary>
    /// Logica para la puntuacion y registrarlo a la BDD
    /// </summary>
    /// <param name="CasePB"></param>
    /// <returns></returns>
    IEnumerator SelectAllScore()
    {
        string idTouristSpot = PlayerPrefs.GetString("SpotID"); ;

        WWWForm form = new WWWForm();
        form.AddField("ID", idTouristSpot);

        WWW www = new WWW("https://tourismappar.000webhostapp.com/average_score.php", form);
        yield return www;
        Cadena = www.text;

        char delimitador = '|';
        string[] valores = Cadena.Split(delimitador);

        //llenado de los valores en cada variable respectiva
        //maximunPB para el valor maximo de los mproguess bar
        maximunPB = int.Parse(valores[0]);
        //valor de cada proguess bar
        currentPB1 = int.Parse(valores[1]);
        currentPB2 = int.Parse(valores[2]);
        currentPB3 = int.Parse(valores[3]);
        currentPB4 = int.Parse(valores[4]);
        currentPB5 = int.Parse(valores[5]);

        //es la formula para el promedio de todos los datos
        average =(5 * currentPB5 + 4 * currentPB4 + 3 * currentPB3 + 2 * currentPB2 + 1 * currentPB1) / (currentPB5 + currentPB4 + currentPB3 + currentPB2 + currentPB1);
        //media total
        Calification.text = average.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelectAllScore());
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill1();
        GetCurrentFill2();
        GetCurrentFill3();
        GetCurrentFill4();
        GetCurrentFill5();
    }

    /// <summary>
    /// Logica para el llenado de los proguess bar
    /// </summary>

    #region
    void GetCurrentFill1()
    {
        float fillAmount = (float)currentPB1 / (float)maximunPB;
        mask1.fillAmount = fillAmount;
    }

    void GetCurrentFill2()
    {
        float fillAmount = (float)currentPB2 / (float)maximunPB;
        mask2.fillAmount = fillAmount;
    }

    void GetCurrentFill3()
    {
        float fillAmount = (float)currentPB3 / (float)maximunPB;
        mask3.fillAmount = fillAmount;
    }

    void GetCurrentFill4()
    {
        float fillAmount = (float)currentPB4 / (float)maximunPB;
        mask4.fillAmount = fillAmount;
    }

    void GetCurrentFill5()
    {
        float fillAmount = (float)currentPB5 / (float)maximunPB;
        mask5.fillAmount = fillAmount;
    }
    #endregion
}