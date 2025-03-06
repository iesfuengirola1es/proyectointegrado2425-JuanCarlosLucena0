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
            StartCoroutine(LoginUser(email, password));
        }
        else
        {
            Debug.LogError("Por favor, completa todos los campos.");
        }

        Debug.Log($"Intentando iniciar sesión con Email: {email} y Contraseña: {password}");
        // Aquí llamaremos a la API para validar los datos en el servidor.
    }

    // Recoge usuario, email y contraseña en al creación
    private void HandleRegister()
    {
        string username = registerUsernameInput.text;
        string email = registerEmailInput.text;
        string password = registerPasswordInput.text;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        {
            StartCoroutine(RegisterUser(username, email, password));
        }
        else
        {
            Debug.LogError("Por favor, completa todos los campos.");
        }

        Debug.Log($"Registrando Usuario: {username}, Email: {email}, Contraseña: {password}");
        // Aquí llamaremos a la API para registrar la cuenta en el servidor.
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

        // Crear un JSON con los datos del usuario
        string jsonData = $"{{\"nombre\":\"{username}\",\"email\":\"{email}\",\"password\":\"{password}\"}}";
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Configurar la solicitud HTTP
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Enviar solicitud y esperar respuesta
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Registro exitoso: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error en el registro: " + request.error);
            }
        }
    }

    private IEnumerator LoginUser(string email, string password)
    {
        string url = "https://luze0oo0.pythonanywhere.com/user/login";

        // Crear el JSON con email y contraseña
        LoginRequest loginRequest = new LoginRequest { email = email, password = password };
        string jsonData = JsonUtility.ToJson(loginRequest);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Enviar la solicitud y esperar respuesta
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Login exitoso: " + responseText);

                // Convertimos la respuesta en un objeto UserData
                UserData user = JsonUtility.FromJson<UserData>(responseText);

                // Guardamos los datos del usuario en la sesión
                SaveUserSession(user);

                // Cargar la escena del juego
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            }
            else
            {
                Debug.LogError("Error al iniciar sesión: " + request.downloadHandler.text);
            }
        }
    }
    private void SaveUserSession(UserData userData)
    {
        PlayerPrefs.SetInt("user_id", userData.id);
        PlayerPrefs.SetString("username", userData.nombre);
        PlayerPrefs.SetString("email", userData.email);
        PlayerPrefs.Save();
    }

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
