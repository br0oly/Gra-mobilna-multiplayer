using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoading : MonoBehaviour
{
    public void ButtonPlayPressed()
    {
        SceneManager.LoadScene("GP-Scene");
    }
}
