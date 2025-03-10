
/*Se usa para enviar una solicitud de inicio de sesi�n al servidor.
Contiene los datos del usuario: correo electr�nico(email) y contrase�a(password).
Se serializa a JSON antes de enviarla en la solicitud HTTP.*/
[System.Serializable]
public class LoginRequest
    {
        public string email;
        public string password;
    }

