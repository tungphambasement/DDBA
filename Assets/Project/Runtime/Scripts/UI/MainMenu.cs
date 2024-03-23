using UnityEngine.SceneManagement;
using UnityEngine;

public class MaineMenu : MonoBehaviour
{
    public void GoToScene(string scneneName){
        SceneManager.LoadScene(scneneName);
    }

    public void QuitApp(){
        Application.Quit();
        Debug.Log("APPLICATION HAS QUIT");
    }

}
