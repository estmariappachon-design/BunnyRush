using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("LVL1");
    }

    public void Salir()
    {
        Application.Quit();
    }
}