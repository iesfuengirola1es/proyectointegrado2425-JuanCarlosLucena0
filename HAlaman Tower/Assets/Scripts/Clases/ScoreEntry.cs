//Se utiliza al recibir datos del servidor cuando se obtiene el ranking de puntajes.
[System.Serializable]
//Representa una entrada de puntuaci�n, con el puntaje (score) y el nombre del usuario (username).

public class ScoreEntry
{
    public int score;
    public string username;
}
//Contiene una lista de ScoreEntry, usada para manejar m�ltiples puntuaciones
[System.Serializable]
public class ScoreList
{
    public ScoreEntry[] scores;
}

