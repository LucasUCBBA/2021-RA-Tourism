using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public class RegisterScript : MonoBehaviour
{
    /// <summary>
    /// Nombre de la aplicacion: 
    /// Nombre del desarrollador: Sergio Rodrigo Iriarte Zamorano
    /// Nombre del desarrollador: Jhonatan Levi Sanga Balcazar
    /// Fecha de Creacion: ?
    /// </summary>

    //<copyright file = "RegisterScript.cs" company = "">
    //
    //</copyright>
    //<author>Sergio Rodrigo Iriarte Zamorano</author>

    public InputField emailInput, passwordInput, repeatPasswordInput, nameInput, firstSurnameInput,
        secondSurnameInput, dayInput, monthInput, yearInput;
    public Dropdown countryDropdown, regionDropdown;
    public Button loginButton, registerButton;
    public Text registerMessage, emailError, passwordError, repeatpasswordError, nameError, firstSurnameError, secondSurnameError, dateOfBirthError;
    List<string> regionsStringList = new List<string>();
    
    // Start is called before the first frame update
    void Start()
    {
        //Dar el accion "onClick" a los botones del UI
        loginButton.onClick.AddListener(Login_Button_OnClick);
        registerButton.onClick.AddListener(Register_Button_OnClick);

        //Dar el accion "onValueChanged" al dropdown de pais
        countryDropdown.onValueChanged.AddListener(delegate {
            Country_Dropdown_OnValueChanged();
        });

        //Cambiar el formato de los UI de contraseña para que no se pueda ver la contraseña
        passwordInput.inputType = repeatPasswordInput.inputType = InputField.InputType.Password;

        //Carga los distintos paises y departamentos a los dropdowns de la ventana
        StartCoroutine(Load_Countries());
        StartCoroutine(Load_Regions());
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
    /// Metodo que nos lleva a la ventana del login
    /// </summary>
    void Login_Button_OnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Metodo que verifica todos los datos necesarios para registrar un usuario
    /// </summary>
    void Register_Button_OnClick()
    {
        bool errorFlag = false;
        emailError.text="";
        passwordError.text="";
        repeatpasswordError.text="";
        nameError.text="";
        firstSurnameError.text="";
        secondSurnameError.text="";
        dateOfBirthError.text="";
        registerMessage.text="";
        try //validations
        {
            #region Validaciones
            if (emailInput.text != "")
            {
                try
                {
                    System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(emailInput.text);
                }
                catch (System.Exception)
                {
                    errorFlag = true;
                    emailError.text = "Formato incorrecto.";
                }
            }else{
                errorFlag = true;
                emailError.text = "El correo requerido.";
            }

            if (passwordInput.text != "")//password
            {
                if (passwordInput.text.Length < 8)
                {
                    errorFlag = true;
                    passwordError.text="Mínimo 8 caracteres";
                }
            }else{
                errorFlag = true;
                passwordError.text="Contraseña requerida.";
            }

            if (repeatPasswordInput.text != "")//repeat password
            {
                if (repeatPasswordInput.text != passwordInput.text)
                {
                    errorFlag = true;
                    repeatpasswordError.text="Repita la contraseña.";
                }
            }else{
                errorFlag = true;
                repeatpasswordError.text="Repita la contraseña.";
            }

            
            Regex rgx = new Regex(@"^[a-zA-Z]{1,60}$");
            if (nameInput.text != "")//name
            {
                if (!rgx.IsMatch(nameInput.text))
                {
                    errorFlag = true;
                    nameError.text = "Nombre erroneo.";
                }
            }else{
                errorFlag = true;
                nameError.text = "Nombre requerido.";
            }
            
            if (firstSurnameInput.text != "")//surname
            {
                if (!rgx.IsMatch(firstSurnameInput.text))
                {
                    errorFlag = true;
                    firstSurnameError.text = "Apellido erroneo.";
                }
            }else{
                errorFlag = true;
                firstSurnameError.text="Apellido requerido.";
            }

            if (secondSurnameInput.text != "")//second surname
            {
                if (!rgx.IsMatch(secondSurnameInput.text))
                {
                    errorFlag = true;
                    secondSurnameError.text = "Apellido erroneo.";
                }
            }

            System.DateTime d;//date of birth
            if (dayInput.text.Length == 1)
                dayInput.text = "0"+dayInput.text;
            if (monthInput.text.Length == 1)
                monthInput.text = "0"+monthInput.text;
            bool chValidity = System.DateTime.TryParseExact(
            dayInput.text+"/"+monthInput.text+"/"+yearInput.text,
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out d);
            if (!chValidity)
            {
                dateOfBirthError.text="Fecha no válida.";
                errorFlag = true;
            }
            if (d.Year >= System.DateTime.Now.Year || d.Year < 1900)
            {
                dateOfBirthError.text="Año no válido.";
                errorFlag = true;
            }
            #endregion

            if (!errorFlag)
            {
                string email = emailInput.text, password = passwordInput.text, repeatPassword = repeatPasswordInput.text,
                   name = nameInput.text, firstSurname = firstSurnameInput.text, secondSurname = secondSurnameInput.text,
                   day = dayInput.text, month = monthInput.text, year = yearInput.text,
                   country = countryDropdown.options[countryDropdown.value].text, region = "";
                if (regionDropdown.options.Count > 0)
                {
                    region = regionDropdown.options[regionDropdown.value].text;
                    StartCoroutine(Register(email, password, name, firstSurname, secondSurname, day, month, year, country, region));
                }
                else
                {
                    StartCoroutine(Register(email, password, name, firstSurname, secondSurname, day, month, year, country));
                }
            }
        }
        catch (System.Exception ex)
        {
            registerMessage.text = ex.Message;
        }
    }

    /// <summary>
    /// Este metodo se ejecuta cada vez que el pais cambia en el dropdown de paises, para comprobar si la seleccion es "Bolivia", y mostrar los departamentos
    /// </summary>
    void Country_Dropdown_OnValueChanged()
    {
        if(countryDropdown.options[countryDropdown.value].text == "Bolivia")
        {
            regionDropdown.AddOptions(regionsStringList);
            regionDropdown.interactable = true;
        }
        else
        {
            regionDropdown.ClearOptions();
            regionDropdown.interactable = false;
        }
    }

    /// <summary>
    /// Este metodo registra el usuario a la base de datos, con todos los datos dados por dicho usuario
    /// </summary>
    /// <param name="email">Correo del usuario</param>
    /// <param name="password">Contraseña del usuario</param>
    /// <param name="name">Nombre del usuario</param>
    /// <param name="firstSurname">Primer apellido del usuario</param>
    /// <param name="secondSurname">Segundo apellido del usuario</param>
    /// <param name="day">Dia (Fecha de nacimiento)</param>
    /// <param name="month">Mes (Fecha de nacimiento)</param>
    /// <param name="year">Año (Fecha de nacimiento)</param>
    /// <param name="country">Pais del usuario</param>
    /// <returns>Retorna el mensaje que da la base de datos, siendo de error o de confirmacion de registrar usuario</returns>
    IEnumerator Register(string email, string password, string name, string firstSurname,
        string secondSurname, string day, string month, string year, string country)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("name", name);
        form.AddField("firstSurname", firstSurname);
        form.AddField("secondSurname", secondSurname);
        form.AddField("day", day);
        form.AddField("month", month);
        form.AddField("year", year);
        form.AddField("country", country);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/register_user.php", form);
        yield return www;
        registerMessage.text = www.text;

        Send(email, name, firstSurname, password);

        if(www.text == "Usuario registrado")
        {
            registerButton.enabled = false;
            yield return new WaitForSeconds(5);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    /// <summary>
    /// Este metodo registra el usuario a la base de datos, con todos los datos dados por dicho usuario
    /// </summary>
    /// <param name="email">Correo del usuario</param>
    /// <param name="password">Contraseña del usuario</param>
    /// <param name="name">Nombre del usuario</param>
    /// <param name="firstSurname">Primer apellido del usuario</param>
    /// <param name="secondSurname">Segundo apellido del usuario</param>
    /// <param name="day">Dia (Fecha de nacimiento)</param>
    /// <param name="month">Mes (Fecha de nacimiento)</param>
    /// <param name="year">Año (Fecha de nacimiento)</param>
    /// <param name="country">Pais del usuario</param>
    /// <param name="region">Departamento del usuario (departmento de Bolivia)</param>
    /// <returns>Retorna el mensaje que da la base de datos, siendo de error o de confirmacion de registrar usuario</returns>
    IEnumerator Register(string email, string password, string name, string firstSurname,
        string secondSurname, string day, string month, string year, string country, string region)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("name", name);
        form.AddField("firstSurname", firstSurname);
        form.AddField("secondSurname", secondSurname);
        form.AddField("day", day);
        form.AddField("month", month);
        form.AddField("year", year);
        form.AddField("country", country);
        form.AddField("region", region);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/register_user.php", form);
        yield return www;
        registerMessage.text = www.text;

        
        Send(email, name, firstSurname, password);

        if (www.text == "Usuario registrado")
        {
            registerButton.enabled = false;
            yield return new WaitForSeconds(5);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
    /// <summary>
    /// Envia al correo electronico la informacion de que la cuenta ha sido creada
    /// </summary>
    /// <param name="email">Correo electronico al que se enviara el mensaje</param>
    /// <param name="name">Nombre del usuario</param>
    /// <param name="firstname">Primer nombre</param>
    /// <param name="pswd">La contraseña</param>
    public static void Send(string email, string name, string firstname, string pswd) {
 
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("TourismAppAR@gmail.com");
        mail.To.Add(email);
        mail.Subject = "Bienvenido a TourismApp <3";
        mail.Body = "Cuenta creada para "+name+" "+firstname+" con la contraseña "+pswd;
 
        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
        smtp.Port = 587;
        smtp.Credentials = new System.Net.NetworkCredential("TourismAppAR@gmail.com", "Tourism69App69AR69") as ICredentialsByHost;
        smtp.EnableSsl = true;
 
        ServicePointManager.ServerCertificateValidationCallback =
                delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
                    return true;
                };
        smtp.Send(mail);
    }

    /// <summary>
    /// Cargar el dropdown del pais con todos los paises de la base de datos
    /// </summary>
    /// <returns>Retorna mensaje de la base de datos</returns>
    IEnumerator Load_Countries()
    {
        WWW www = new WWW("https://tourismappar.000webhostapp.com/countries.php");
        yield return www;
        List<string> countriesStringList = new List<string>();
        string[] countriesStringArray = www.text.Split('|');
        countryDropdown.ClearOptions();
        foreach (string country in countriesStringArray)
        {
            countriesStringList.Add(country);
        }
        countriesStringList.RemoveAt(countriesStringList.Count - 1);
        countryDropdown.AddOptions(countriesStringList);
    }

    /// <summary>
    /// Cargar el dropdown con todos los departamentos de la base de datos
    /// </summary>
    /// <returns></returns>
    IEnumerator Load_Regions()
    {
        WWW www = new WWW("https://tourismappar.000webhostapp.com/regions.php");
        yield return www;
        string[] regionsStringArray = www.text.Split('|');
        regionDropdown.ClearOptions();
        regionsStringList.Clear();
        foreach (string country in regionsStringArray)
        {
            regionsStringList.Add(country);
        }
        regionsStringList.RemoveAt(regionsStringList.Count - 1);
        regionDropdown.AddOptions(regionsStringList);
    }
}