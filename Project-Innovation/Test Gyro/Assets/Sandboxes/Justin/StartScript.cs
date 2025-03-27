using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Start Button Clicked!"); // Check if this prints in the Console
        SceneManager.LoadScene("TestingScene1 1"); // Replace "GameScene" with your actual scene name
    }
}
