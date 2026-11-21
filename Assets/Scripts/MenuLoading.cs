using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoading : MonoBehaviour
{
    private void ButtonPlayPressed() 
    {
        LoadScene();
    }
    private void LoadScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
