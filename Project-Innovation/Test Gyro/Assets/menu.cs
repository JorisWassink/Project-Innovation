using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public void GoTo(string name)
    {
        SceneManager.LoadScene(name);
    }
}
