
/*Se usa para enviar una solicitud de inicio de sesión al servidor.
Contiene los datos del usuario: correo electrónico(email) y contraseña(password).
Se serializa a JSON antes de enviarla en la solicitud HTTP.*/
[System.Serializable]
public class LoginRequest
    {
        public string email;
        public string password;
    }

