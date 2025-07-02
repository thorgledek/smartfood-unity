using UnityEngine;
using UnityEngine.SceneManagement;

public class PindahScene : MonoBehaviour
{
    public void LoadToScene(string sceneName)
    {
        Debug.Log("Load scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
