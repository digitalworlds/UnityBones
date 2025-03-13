using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu:MonoBehaviour
{
    public void GoToScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
    public void QuitApp(){
        Application.Quit();
        Debug.Log("It has quit!");
    }
}
