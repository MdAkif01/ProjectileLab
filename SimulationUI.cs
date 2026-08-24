using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSimulation : MonoBehaviour
{
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}