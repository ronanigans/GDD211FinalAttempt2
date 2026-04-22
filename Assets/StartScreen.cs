using UnityEngine;
using UnityEngine.SceneManagement; 

public class StartScreen : MonoBehaviour
{
    // Function to load the main game scene
    public void OnPlayButton()
    {
        // Load the scene with build index 1 (your game scene)
        SceneManager.LoadScene(1);
    }

}