using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    // Character controller
    public CharacterController Cc;
    public Transform cameraTransform;
    public float Gravity;
    public float WalkSpeed;
    private float yspeed;

    //doors
    public GameObject closedDoor;
    public GameObject openDoor;

    // Score
    private static float score = 0;
    [SerializeField] TextMeshProUGUI scoreScavenged;

    // Timer
    private float timer = 70;
    [SerializeField] TextMeshProUGUI timeLeft;

    // Mini game tags & objects
    public string scrap = "Scrap";
    public GameObject weldingGame;
    public string wires = "Wires";
    public GameObject wireGame;
    public GameObject wireObjects;
    public string keypad = "Keypad";
    public GameObject keypadGame;
    public string heating = "Heating";
    public GameObject heatGame;

    private bool inGame = false;

    // --- Wire Game Variables ---
    public List<Wire> leftWires; // Ensure these are assigned in Inspector
    private int connectedWires = 0;

    // --- Simon Says (Keypad) Variables ---
    [Header("Simon Says Setup")]
    public List<GameObject> keypadGlows; 
    private List<int> simonSequence = new List<int>();
    private int playerStep = 0;
    private bool acceptingSimonInput = false;

    //Leave gameobject
    public GameObject leaveGame;
    public string leaver = "Leaver";

    void Start()
    {
        weldingGame.SetActive(false);
        wireGame.SetActive(false);
        keypadGame.SetActive(false);
        heatGame.SetActive(false);
        wireObjects.SetActive(false);
        foreach(GameObject glow in keypadGlows) glow.SetActive(false);
    }

    private void HandleRaycasting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit))
            {
                if (hit.collider.CompareTag(scrap)) PlayMiniGame(scrap);
                else if (hit.collider.CompareTag(wires)) PlayMiniGame(wires);
                else if (hit.collider.CompareTag(keypad)) PlayMiniGame(keypad);
                else if (hit.collider.CompareTag(heating)) PlayMiniGame(heating);
                else if (hit.collider.CompareTag(leaver)) SceneManager.LoadScene(2);
            }
        }
    }

    private void PlayMiniGame(string game)
    {
        inGame = true;
        if (game == "Scrap") weldingGame.SetActive(true);
        else if (game == "Wires")
        {
            connectedWires = 0;
            wireGame.SetActive(true);
            wireObjects.SetActive(true);
        }
        else if (game == "Keypad")
        {
            keypadGame.SetActive(true);
            StartSimonSays();
        }
        else if (game == "Heating") heatGame.SetActive(true);
    }

    // --- Simon Says Logic ---
    public void StartSimonSays()
    {
        simonSequence.Clear();
        playerStep = 0;
        foreach(GameObject glow in keypadGlows) glow.SetActive(false);
        AddSimonStep();
    }

    void AddSimonStep()
    {
        simonSequence.Add(Random.Range(0, keypadGlows.Count));
        StartCoroutine(PlaySimonSequence());
    }

    IEnumerator PlaySimonSequence()
    {
        acceptingSimonInput = false;
        yield return new WaitForSeconds(0.5f);
        foreach (int index in simonSequence)
        {
            yield return StartCoroutine(FlashButton(index));
            yield return new WaitForSeconds(0.3f);
        }
        acceptingSimonInput = true;
    }

    IEnumerator FlashButton(int index)
    {
        keypadGlows[index].SetActive(true);
        yield return new WaitForSeconds(0.4f);
        keypadGlows[index].SetActive(false);
    }

    public void OnSimonButtonClick(int index)
    {
        if (!acceptingSimonInput) return;
        if (index == simonSequence[playerStep])
        {
            StartCoroutine(FlashButton(index));
            playerStep++;
            if (playerStep >= simonSequence.Count)
            {
                if (simonSequence.Count >= 5) RegisterTaskComplete();
                else { playerStep = 0; Invoke("AddSimonStep", 1f); }
            }
        }
        else StartSimonSays();
    }

    // --- Task Management ---
    public void RegisterConnection() 
    {
        connectedWires++;
        if (connectedWires >= leftWires.Count) RegisterTaskComplete();
    }

    public void RegisterTaskComplete()
    {
        score++;
        scoreScavenged.text = score.ToString();
        Invoke("EndMiniGame", 0.5f);
        if(score >= 3)
        {
            closedDoor.SetActive(true);
            openDoor.SetActive(false);
        }
    }

    private void EndMiniGame()
    {
        // RESET WIRES
        foreach (Wire w in leftWires)
        {
            if (w != null) w.ResetWire();
        }
        connectedWires = 0;

        weldingGame.SetActive(false);
        wireGame.SetActive(false);
        keypadGame.SetActive(false);
        heatGame.SetActive(false);
        wireObjects.SetActive(false);
        inGame = false;
    }

    void Update()
    {
        if (!inGame)
        {
            timer -= Time.deltaTime;
            if (timer <= 0) SceneManager.LoadScene(2);
            timeLeft.text = timer.ToString("F0");
            scoreScavenged.text = score.ToString();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
            cameraTransform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), 0, 0));
            Vector3 move = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
            move = move.normalized * WalkSpeed;
            if (!Cc.isGrounded) yspeed -= Gravity * Time.deltaTime;
            else yspeed = -1f;
            move.y = yspeed;
            Cc.Move(move * Time.deltaTime);
            HandleRaycasting();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (Input.GetKeyDown(KeyCode.Space)) EndMiniGame();
        }
    }
}