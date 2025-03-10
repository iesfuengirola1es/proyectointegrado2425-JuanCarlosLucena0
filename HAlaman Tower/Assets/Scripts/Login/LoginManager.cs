using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject registerPanel;

    [Header("Login Fields")]
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;

    [Header("Register Fields")]
    public TMP_InputField registerUsernameInput;
    public TMP_InputField registerEmailInput;
    public TMP_InputField registerPasswordInput;

    [Header("Buttons")]
    public Button loginButton;
    public Button registerButton;
    public Button loginConfirmButton;
    public Button registerConfirmButton;
    public Button loginBackButton;
    public Button registerBackButton;

    private void Start()
    {
        // Desactiva los paneles al inicarse
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);

        // Muestra y oculta los paneles y botones
        loginButton.onClick.AddListener(() => ShowPanel(loginPanel));
        loginButton.onClick.AddListener(()=>HideLoginRegisterButtons());

        registerButton.onClick.AddListener(() => ShowPanel(registerPanel));
        registerButton.onClick.AddListener(() => HideLoginRegisterButtons());


        loginBackButton.onClick.AddListener(() => HidePanel(loginPanel));
        loginBackButton.onClick.AddListener(() => ShowLoginRegisterButtons());

        registerBackButton.onClick.AddListener(() => HidePanel(registerPanel));
        registerBackButton.onClick.AddListener(() => ShowLoginRegisterButtons());

        loginConfirmButton.onClick.AddListener(HandleLogin);
        registerConfirmButton.onClick.AddListener(HandleRegister);
    }

    // Muestra los paneles
    private void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    // Oculta los paneles
    private void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    // Recoge el email y la contraseña en el login
    private void HandleLogin()
    {
        string email = loginEmailInput.text;
        string password = loginPasswordInput.text;

        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        {
            // Llamamos a la API para validar los datos en el servidor.
            StartCoroutine(LoginUser(email, password));
        }
        else
        {
            Debug.LogError("Por favor, completa todos los campos.");
        }

        Debug.Log($"Intentando iniciar sesión con Email: {email} y Contraseña: {password}");
    }

    // Recoge usuario, email y contraseña en al creación
    private void HandleRegister()
    {
        string username = registerUsernameInput.text;
        string email = registerEmailInput.text;
        string password = registerPasswordInput.text;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        {
            // Llamamos a la API para registrar la cuenta en el servidor.
            StartCoroutine(RegisterUser(username, email, password));
        }
        else
        {
            Debug.LogError("Por favor, completa todos los campos.");
        }

        Debug.Log($"Registrando Usuario: {username}, Email: {email}, Contraseña: {password}");
    }

    // Método para ocultar los botones
    private void HideLoginRegisterButtons()
    {
        loginButton.gameObject.SetActive(false);
        registerButton.gameObject.SetActive(false);
    }

    // Método para mostrar los botones
    private void ShowLoginRegisterButtons()
    {
        loginButton.gameObject.SetActive(true);
        registerButton.gameObject.SetActive(true);
    }

    // Creacion de usuario en el servidor
    private IEnumerator RegisterUser(string username, string email, string password)
    {
        string url = "https://luze0oo0.pythonanywhere.com/user/create";
        string jsonData = $"{{\"nombre\":\"{username}\",\"email\":\"{email}\",\"password\":\"{password}\"}}";

        yield return StartCoroutine(ApiManager.SendPostRequest(url, jsonData, "Registro exitoso", "Error en el registro"));
        HidePanel(registerPanel);
        ShowLoginRegisterButtons();
    }

    //Login de usuario
    private IEnumerator LoginUser(string email, string password)
    {
        string url = "https://luze0oo0.pythonanywhere.com/user/login";
        LoginRequest loginRequest = new LoginRequest { email = email, password = password };
        string jsonData = JsonUtility.ToJson(loginRequest);

        yield return StartCoroutine(ApiManager.SendPostRequest(url, jsonData, "Login exitoso", "Error al iniciar sesión", (response) =>
        {
            UserData user = JsonUtility.FromJson<UserData>(response);
            SaveUserSession(user);
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }));
    }

    //Guarda los datos del usuario
    private void SaveUserSession(UserData userData)
    {
        PlayerPrefs.SetInt("user_id", userData.id);
        PlayerPrefs.SetString("username", userData.nombre);
        PlayerPrefs.SetString("email", userData.email);
        PlayerPrefs.Save();
    }

    //Carga los datos del usuario
    private void LoadUserSession()
    {
        if (PlayerPrefs.HasKey("user_id"))
        {
            int userId = PlayerPrefs.GetInt("user_id");
            string username = PlayerPrefs.GetString("username");
            string email = PlayerPrefs.GetString("email");

            Debug.Log($"Usuario cargado: ID {userId}, Nombre {username}, Email {email}");
        }
        else
        {
            Debug.LogError("No hay sesión de usuario guardada.");
        }
    }

    //Desloguea del juego quitando los datos
    public void Logout()
    {
        PlayerPrefs.DeleteKey("user_id");
        PlayerPrefs.DeleteKey("username");
        PlayerPrefs.DeleteKey("email");
        PlayerPrefs.Save();

        // Volver a la escena de inicio de sesión
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScene");
    }


}
