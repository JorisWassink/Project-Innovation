using UnityEngine;
using UnityEngine.SceneManagement;

public class ToLevelScript : MonoBehaviour
{
    public void ToNextLevel()
    {
        GetComponent<AudioSource>().Play();
        
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "Welcome")
        {
            Invoke("LoadInfo", 0.1f);
        }

        if (currentScene.name == "Info")
        {
            Invoke("Load1", 0.1f);
        }

        if (currentScene.name == "Level1")
        {
            Invoke("LoadWin", 0.1f);
        }

        //if (currentScene.name == "Level2")
        //{
        //    Invoke("Load3", 0.1f);
        //}

        //if (currentScene.name == "Level3")
        //{
        //    Invoke("LoadWin", 0.1f);
        //}

        if (currentScene.name == "Win")
        {
            Invoke("LoadWelcome", 0.1f);
        }
    }

    public void LoadInfo()
    {
        SceneManager.LoadScene("Info");
    }
    
    public void Load1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Load2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void Load3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void LoadWin()
    {
        SceneManager.LoadScene("Win");
    }

    public void LoadWelcome()
    {
        SceneManager.LoadScene("Welcome");
    }

    public void restartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
