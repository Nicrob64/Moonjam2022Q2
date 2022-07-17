using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public void TryAgain()
    {
        SceneManager.LoadScene("Warehouse");
    }
}
