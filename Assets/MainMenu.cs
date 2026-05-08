using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreDisplay;
    public PlayerScript playerScript;
    void Start()
{
    // Access it directly using the class name
    //scoreDisplay.text = "Final Score: " + playerScript.score;
}
    // Function to load the main game scene
    public void OnPlayButton()
    {
        // Load the scene with build index 1 (your game scene)
        SceneManager.LoadScene(1);
    }

    public void doExitGame() {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

}
