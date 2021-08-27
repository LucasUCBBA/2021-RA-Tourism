using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class LoginScript : MonoBehaviour
{
    /// <summary>
    /// Nombre de la aplicacion: 
    /// Nombre del desarrollador: Sergio Rodrigo Iriarte Zamorano
    /// Nombre del desarrollador: Jhonatan Levi Sanga Balcazar
    /// Fecha de Creacion: ?
    /// </summary>
    
    //<copyright file = "LoginScript.cs" company = "">
    //
    //</copyright>
    //<author>Sergio Rodrigo Iriarte Zamorano</author>

    #region Login Window
    [Header("Login Window")]
    public GameObject loginWindow;
    public Button loginButton, registerButton, remindButton, confirmButton;
    public InputField emailInput, passwordInput;
    public Text loginMessage, errorEmailMessage, errorPasswordMessage;
    private IEnumerator showToastCoroutine;
    #endregion

    #region User Window
    [Header("User Window")]
    public GameObject userWindow;
    public GameObject confirmButtonField;
    public Button updateUserButton, logoutButton, deleteButton, goBackButton, deleteAccountButton, goChangePassButton;
    public InputField nameInput, firstSurnameInput, secondSurnameInput;
    public Canvas blackScreen;
    public Text dateOfBirthMessage, errorNameMessage, errorFirstSurnameMessage, errorSecondSurnameMessage;
    bool updatingUser;
    #endregion

    #region Bottom Bar
    [Header("Bottom Bar")]
    public Button goBackToMainScreenButton;
    #endregion

    #region variables globales para el cambio de contraseña
    string email = "";
    string codigoVerificacion = "";
    #endregion

    private bool isDeleteUserWindowOpen = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        //Dar accion a los distintos botones de la ventana
        loginButton.onClick.AddListener(Login_Button_OnClick);
        registerButton.onClick.AddListener(Register_Button_OnClick);
        remindButton.onClick.AddListener(Remind_Button_OnClick);
        confirmButton.onClick.AddListener(Confirm_Button_OnClick);
        goChangePassButton.onClick.AddListener(ChangePassword_Button_OnClick);
        passwordInput.inputType = InputField.InputType.Password;

        updateUserButton.onClick.AddListener(Update_User_OnClick);
        logoutButton.onClick.AddListener(Logout_Button_OnClick);
        deleteButton.onClick.AddListener(Delete_Button_OnClick);
        deleteAccountButton.onClick.AddListener(Delete_Account_Button_OnClick);
        goBackButton.onClick.AddListener(Go_Back_To_Main_Screen_Button_OnClick);

        goBackToMainScreenButton.onClick.AddListener(Go_Back_To_Main_Screen_Button_OnClick);

        //Configurar el tamaño de la pantalla negra que se carga cuando deseamos eliminar un usuario
        float canvasX = userWindow.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        float canvasY = userWindow.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.y;
        blackScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasX, canvasY);
        blackScreen.enabled = false;

        //Condicional que controla si usuario esta logueado o no, para mostrar la ventana de login o actualizar/eliminar cuenta
        if (PlayerPrefs.GetString("LoginToken") != "") {
            //Mostrar la ventana del usuario
            loginWindow.SetActive(false);
            userWindow.SetActive(true);

            //Crear una instancia de WWW para comunicar con la base de datos, para obtener cierta informacion de un usuario
            WWWForm form = new WWWForm();
            form.AddField("loginToken", PlayerPrefs.GetString("LoginToken"));
            WWW www = new WWW("https://tourismappar.000webhostapp.com/get_user_data.php", form);
            yield return www;
            
            //Recibir los datos concatenados de la base de datos para luego separarles y usarles en los lugares correctos
            string[] userDataStringArray = www.text.Split('|');
            nameInput.text = userDataStringArray[0];
            firstSurnameInput.text = userDataStringArray[1];
            secondSurnameInput.text = userDataStringArray[2];
            string[] dateOfBirthStringArray = userDataStringArray[3].Split('-');
            dateOfBirthMessage.text = dateOfBirthStringArray[2] + " de " + Get_Month_In_String(dateOfBirthStringArray[1]) + ", " + dateOfBirthStringArray[0];

            updatingUser = false;
        }
        else
        {
            //Mostrar la ventana del login
            loginWindow.SetActive(true);
            userWindow.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Condicional que revisa si el usuario le dio al boton de atras en celular, o escape en PC, para salirse de la ventana
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call<bool>("moveTaskToBack", true);
            }
            else
            {
                Application.Quit();
            }
        }
    }

    /// <summary>
    /// metodo del boton Login con sus respectivas validaciones
    /// </summary>

    void Login_Button_OnClick()
    {
        errorEmailMessage.text = "";
        errorPasswordMessage.text = "";
        String expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        bool flag = true;
        string email = emailInput.text, password = passwordInput.text;
        if (email=="")
        {
            errorEmailMessage.text = "El correo es requerido";
            flag = false;
        }
        if (!Regex.IsMatch(email, expresion))
        {
            errorEmailMessage.text = "Ingrese un correo valido";
            flag = false;
        }
        if (password=="")
        {
            errorPasswordMessage.text = "La contraseña es requerida";
            flag = false;
        }
        if (flag)
        {
            StartCoroutine(Login(email, password));
        }
    }

    /// <summary>
    /// Evento que se activa al hacer click al botoon "olvidaste tu contraseña"
    /// </summary>
    void Remind_Button_OnClick()
    {
        errorEmailMessage.text = "";
        if (emailInput.text != "")
        {
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(emailInput.text);//verificar que el formato del correo sea el correcto
            }
            catch (System.Exception)
            {
                errorEmailMessage.text = "Formato incorrecto.";
            }
            email = emailInput.text;//capturar el email

            Send_Mail(email);//enviar un mensaje al correo con el codigo de verificacion

            loginMessage.text="Revise el correo que ingresó";

            confirmButtonField.SetActive(true);//activar el boton para confirmar el cambio de contraseña
        }
        else
        {
            errorEmailMessage.text = "Ingrese el correo.";
        }
    }

    /// <summary>
    /// Evento que se activa al hacer click en el boton de confimacion de cambio de contraseña
    /// </summary>
    void Confirm_Button_OnClick()
    {
        if (emailInput.text == codigoVerificacion)//el codigo ingresado y el generado son los mismos?
        {
            if(passwordInput.text!="" && passwordInput.text.Length >= 8 && passwordInput.text.Length <= 20)//validacion de la contraseña
            {
                StartCoroutine(Update_Pswd_And_Send_Email(email, passwordInput.text));//enviar el email de confirmacion y cambiar la contraseña
                errorPasswordMessage.text = "";
            }
            else
            {
                errorPasswordMessage.text="contraseña invalida o es menor a 8 caracteres";
            }
        }
        else
        {
            errorEmailMessage.text = "El código es incorrecto";
        }


        

    }

    /// <summary>
    /// Este metodo nos lleva a la venta de registrar usuario
    /// </summary>
    void Register_Button_OnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    /// <summary>
    /// Este metodo nos lleva a la ventana principal, o se sale de la opcion de eliminar usuario
    /// </summary>
    void Go_Back_To_Main_Screen_Button_OnClick()
    {
        if(!isDeleteUserWindowOpen)
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        else
        {
            isDeleteUserWindowOpen = false;
            blackScreen.enabled = false;
        }
    }

    /// <summary>
    /// Este metodo revisa si se esta insertando los datos correctos de un usuario, para luego actualizarlo
    /// </summary>
    void Update_User_OnClick()
    {
        Regex rgx = new Regex(@"^[ a-zA-Z]{1,60}$");
        if (updatingUser) {
            bool errorFlag = false;

            if (nameInput.text != "")
            {
                if (!rgx.IsMatch(nameInput.text))
                {
                    errorFlag = true;
                    errorNameMessage.text = "Nombre erroneo.";
                }
            }
            else
            {
                errorFlag = true;
                errorNameMessage.text = "Nombre requerido.";
            }

            if (firstSurnameInput.text != "")
            {
                if (!rgx.IsMatch(firstSurnameInput.text))
                {
                    errorFlag = true;
                    errorFirstSurnameMessage.text = "Apellido erroneo.";
                }
            }
            else
            {
                errorFlag = true;
                errorFirstSurnameMessage.text = "Apellido requerido.";
            }

            if (secondSurnameInput.text != "")
            {
                if (!rgx.IsMatch(secondSurnameInput.text))
                {
                    errorFlag = true;
                    errorSecondSurnameMessage.text = "Apellido erroneo.";
                }
            }

            if (!errorFlag)
            {
                StartCoroutine(Update_User(nameInput.text, firstSurnameInput.text, secondSurnameInput.text));
                updateUserButton.GetComponentInChildren<Text>().text = "Actualizar Cuenta";
                updatingUser = false;
            }
        } else {
            updateUserButton.GetComponentInChildren<Text>().text = "Confirmar Actualización";
            updatingUser = true;
            nameInput.readOnly = firstSurnameInput.readOnly = secondSurnameInput.readOnly = false;
        }
    }

    /// <summary>
    /// Este metodo cierra sesion del usuario logueado, y le dirige a la ventana principal
    /// </summary>
    void Logout_Button_OnClick()
    {
        PlayerPrefs.SetString("LoginToken", "");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    void ChangePassword_Button_OnClick()
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
                UnityEngine.SceneManagement.SceneManager.LoadScene(12);
            }
        }
        catch (UnityException ex) { Application.Quit(); }
    }
    

    /// <summary>
    /// Este metodo muestra una alerta para eliminar usuario o no
    /// </summary>
    void Delete_Button_OnClick()
    {
        isDeleteUserWindowOpen = true;
        blackScreen.enabled = true;
    }

    /// <summary>
    /// Este metodo inicia el metodo para eliminar el usuario
    /// </summary>
    void Delete_Account_Button_OnClick()
    {
        StartCoroutine(Delete_User());
    }

    /// <summary>
    /// Llama al metodo Send adjuntando un numero aleatorio de 6 cifras
    /// </summary>
    /// <param name="email">Correo electronico al que se enviara el mensaje</param>
    void Send_Mail(string email)
    {
        
        System.Random random=new System.Random();
        codigoVerificacion=random.Next(100000,999999) + "";//generar numeroa de 6 cifras

        Send(email, codigoVerificacion);//llamar al metodo Send
    }
    /// <summary>
    /// Actualiza la contraseña del usuario y le envia un correo hacendole saber que se ha actualizado
    /// </summary>
    /// <param name="email">Correo electronico al que se enviara el mensaje</param>
    /// <param name="newPswd">La nueva contraseña</param>
    /// <returns></returns>
    IEnumerator Update_Pswd_And_Send_Email(string email, string newPswd)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("pswd", newPswd);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/update_password.php", form);//llamada a la base de datos
        yield return www;
        if(www.text=="success")//consulta en la base de datos exitosa
        {
            Send_Success_Email(email, newPswd);//enviar el correo

            loginMessage.text="Se actualizó correctamente";//hacer saber el cambio de contraseña mediante la interfaz
            codigoVerificacion="";
            passwordInput.text="";
            emailInput.text="";
            email="";
        
            confirmButtonField.SetActive(false);//desactivar el boton de confirmacion
        }
        else
        {
        loginMessage.text="nope";
        }
    }
    /// <summary>
    /// Envia un correo electronico al email especificado con el codigo de verificacion para el cambio de contraseña
    /// </summary>
    /// <param name="email">Correo electronico al que se enviara el mensaje</param>
    /// <param name="verCod">El codigo de verificacion</param>
    public static void Send(string email, string verCod) {
 
        //configuracion del objeto mail para el envio de correo
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("tirusmoapp@gmail.com");
        mail.To.Add(email);
        mail.Subject = "Recordar contraseña";
        mail.Body = "Introduzca el codigo " + verCod+" en el campo de Correo Electronico y su nueva contraseña en el campo de contraseña, luego presione Confirmar";
 
        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
        smtp.Port = 587;
        smtp.Credentials = new System.Net.NetworkCredential("tirusmoapp@gmail.com", "turismo2021") as ICredentialsByHost;
        smtp.EnableSsl = true;
 
        ServicePointManager.ServerCertificateValidationCallback =
                delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
                    return true;
                };
        smtp.Send(mail);//envio de correo
    }
    /// <summary>
    /// envia al correo especificado la infomacion de que su contraseña se ha actualizado
    /// </summary>
    /// <param name="email">Correo al que se enviara el mensaje</param>
    /// <param name="newPswd">La nueva contraseña</param>
    public static void Send_Success_Email(string email, string newPswd) {
 
        //configuracion del objeto mail para el envio de correo
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("tirusmoapp@gmail.com");
        mail.To.Add(email);
        mail.Subject = "Recordar contraseña";
        mail.Body = "Su nueva contraseña es " + newPswd;
 
        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
        smtp.Port = 587;
        smtp.Credentials = new System.Net.NetworkCredential("tirusmoapp@gmail.com", "turismo2021") as ICredentialsByHost;
        smtp.EnableSsl = true;
 
        ServicePointManager.ServerCertificateValidationCallback =
                delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
                    return true;
                };
        smtp.Send(mail);//envio del correo
    }

    /// <summary>
    /// Este metodo es el que recibe los datos del correo y contraseña, para verificar que su cuenta existe en la base de datos, y loguearle
    /// </summary>
    /// <param name="email">Correo dado por el usuario</param>
    /// <param name="password">Contraseña dado por el usuario</param>
    /// <returns>Retorna un mensaje de la base de datos, confirmando que se ejecuto la consulta, o lanzar error</returns>
    IEnumerator Login(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/login.php", form);
        yield return www;
        Show_Toast(www.text, 10);
        if(www.text == "Bienvenido")
        {
            System.Random random = new System.Random();
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string token = new string(Enumerable.Repeat(chars, 40).Select(s => s[random.Next(s.Length)]).ToArray());
            form.AddField("token", token);
            www = new WWW("https://tourismappar.000webhostapp.com/insert_login_token.php", form);
            yield return www;
            PlayerPrefs.SetString("LoginToken", www.text);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    /// <summary>
    /// Este metodo actualiza el usuario en la base de datos. En este caso, el nombre completo del usuario.
    /// </summary>
    /// <param name="firstName">El nombre del usuario</param>
    /// <param name="firstSurname">El primer apellido del usuario</param>
    /// <param name="secondSurname">El segundo apellido del usuario (no requerido)</param>
    /// <returns>Retorna un mensaje de la base de datos, confirmando que se ejecuto la consulta, o lanzar error</returns>
    IEnumerator Update_User(string firstName, string firstSurname, string secondSurname)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginToken", PlayerPrefs.GetString("LoginToken"));
        form.AddField("firstName", firstName);
        form.AddField("firstSurname", firstSurname);
        form.AddField("secondSurname", secondSurname);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/update_user.php", form);
        yield return www;
    }

    /// <summary>
    /// Este metodo elimina el usuario de la base de datos (eliminacion logica)
    /// </summary>
    /// <returns>Retorna un mensaje de la base de datos, confirmando que se ejecuto la consulta, o lanzar error</returns>
    IEnumerator Delete_User()
    {
        WWWForm form = new WWWForm();
        form.AddField("loginToken", PlayerPrefs.GetString("LoginToken"));
        WWW www = new WWW("https://tourismappar.000webhostapp.com/delete_user.php", form);
        yield return www;
        if(www.text == "1")
        {
            PlayerPrefs.SetString("LoginToken", "");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    /// <summary>
    /// Este metodo ejecuta el metodo que muestra un mensaje recibido de la base de datos
    /// </summary>
    /// <param name="text">Mensaje recibido de la base de datos, siendo de confirmacion o de error</param>
    /// <param name="duration">Tiempo (en segundos) que tardara el mensaje en estar en la pantalla, antes de desaparecer</param>
    void Show_Toast(string text, int duration)
    {
        if (showToastCoroutine != null)
        {
            StopCoroutine(showToastCoroutine);
        }
        showToastCoroutine = Show_Toast_Coroutine_Met(text, duration);
        StartCoroutine(showToastCoroutine);
    }

    /// <summary>
    /// Este metodo carga el mensaje en la pantalla
    /// </summary>
    /// <param name="text">Mensaje recibido de la base de datos, siendo de confirmacion o de error</param>
    /// <param name="duration">Tiempo (en segundos) que tardara el mensaje en estar en la pantalla, antes de desaparecer</param>
    /// <returns></returns>
    private IEnumerator Show_Toast_Coroutine_Met(string text, int duration)
    {
        loginMessage.text = text;
        loginMessage.enabled = true;
        loginMessage.color = Color.red;

        //Fade in
        yield return Fade_In_And_Out(loginMessage, true, 0.1f);

        //Wait for the duration
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Fade out
        yield return Fade_In_And_Out(loginMessage, false, 0.5f);

        loginMessage.enabled = false;
    }

    /// <summary>
    /// Este metodo ejecuta el contador, mientras que este mostrando el mensaje, para saber cuanto tiempo falta antes de desvanecerse
    /// </summary>
    /// <param name="targetText">Un elemento UI para reemplazar los atributos de color sobre este elementos</param>
    /// <param name="fadeIn">Booleano que verifica si debe mostrar el mensaje, u ocultarlo</param>
    /// <param name="duration">El contador, para determinar cuanto tiempo mas falta para quitar el mensaje</param>
    /// <returns></returns>
    IEnumerator Fade_In_And_Out(Text targetText, bool fadeIn, float duration)
    {
        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }

        Color currentColor = Color.clear;
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }

    /// <summary>
    /// Metodo que reemplaza el mes dado en formato de numero, a su valor textual 
    /// </summary>
    /// <param name="monthInInt">El numero del mes</param>
    /// <returns></returns>
    string Get_Month_In_String(string monthInInt)
    {
        string monthInString = "";
        switch (int.Parse(monthInInt))
        {
            case 1: monthInString = "enero"; break;
            case 2: monthInString = "febrero"; break;
            case 3: monthInString = "marzo"; break;
            case 4: monthInString = "abril"; break;
            case 5: monthInString = "mayo"; break;
            case 6: monthInString = "junio"; break;
            case 7: monthInString = "julio"; break;
            case 8: monthInString = "agosto"; break;
            case 9: monthInString = "septiembre"; break;
            case 10: monthInString = "octubre"; break;
            case 11: monthInString = "noviembre"; break;
            case 12: monthInString = "diciembre"; break;
        }
        return monthInString;
    }
}