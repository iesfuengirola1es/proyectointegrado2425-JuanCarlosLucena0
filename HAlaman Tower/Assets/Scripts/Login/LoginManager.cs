using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        // Muestra y los paneles
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

    private void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    private void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    // Recoge el email y la contraseña en el login
    private void HandleLogin()
    {
        string email = loginEmailInput.text;
        string password = loginPasswordInput.text;

        Debug.Log($"Intentando iniciar sesión con Email: {email} y Contraseña: {password}");
        // Aquí llamaremos a la API para validar los datos en el servidor.
    }

    // Recoge usuario, email y contraseña en al creación
    private void HandleRegister()
    {
        string username = registerUsernameInput.text;
        string email = registerEmailInput.text;
        string password = registerPasswordInput.text;

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


}
