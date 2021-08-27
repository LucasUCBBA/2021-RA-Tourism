using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangePasswordScript : MonoBehaviour
{
    [SerializeField]
    public Button executeButton, backButton;
    public Text errorMessage1, errorMessage2;
    public InputField oldPassInput, newPassInput, repeatNewPassInput;

    // Start is called before the first frame update
    void Start()
    {
        executeButton.onClick.AddListener(Change_Password_Button_OnClick);
        backButton.onClick.AddListener(Go_Back_Button_OnClick);
    }

    void Change_Password_Button_OnClick()
    {
        errorMessage1.text = "";
        errorMessage2.text = "";
        bool flag = false;
        if (oldPassInput.text == "")
        {
            errorMessage1.text = "Contraseña actual requerida.";
            flag = true;
        }
        if (newPassInput.text != "")
        {
            if (newPassInput.text.Length < 8)
            {
                errorMessage2.text = "Se requiere 8 caracteres como mínimo.";
                flag = true;
            }
        }
        else
        {
            errorMessage2.text = "Nueva contraseña requerida.";
            flag = true;
        }
        if (repeatNewPassInput.text != "")//repeat password
        {
            if (repeatNewPassInput.text != newPassInput.text)
            {
                errorMessage2.text = "Las contraseñas no coinciden";
                flag = true;
            }
        }
        else
        {
            errorMessage2.text = "Repita la contraseña.";
            flag = true;
        }
        if (flag == false)
        {
            string loginToken = PlayerPrefs.GetString("LoginToken");
            Debug.Log("1:" + oldPassInput.text + " 2:" + newPassInput.text + " 3:" + loginToken);
            if (loginToken.Length >=40)
            {
                StartCoroutine(ChangePassword(oldPassInput.text, newPassInput.text, loginToken));
            }
            else errorMessage1.text = "Error: token vacio, reinicie sesión.";
        }

        IEnumerator ChangePassword(string oldPassword, string newPassword, string tokenLogin)
        {
            WWWForm form = new WWWForm();
            form.AddField("oldPassword", oldPassword);
            form.AddField("newPassword", newPassword);
            form.AddField("token", tokenLogin);
            WWW www = new WWW("https://tourismappar.000webhostapp.com/changePassword.php", form);
            yield return www;
            Debug.Log("www:" + www.text);
            if (www.text == "cambiado") errorMessage2.text = "Contraseña cambiada con éxito.";
            else
            {
                if (www.text == "rechazado") errorMessage2.text = "Contraseña actual errónea, intente de nuevo.";
                else errorMessage2.text = www.text;
            }
        }
        oldPassInput.text = "";
        newPassInput.text = "";
        repeatNewPassInput.text = "";
    }

    void Go_Back_Button_OnClick()
    {
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call<bool>("moveTaskToBack", true);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            }
        }
        catch (UnityException ex) { Application.Quit(); }
    }
}
