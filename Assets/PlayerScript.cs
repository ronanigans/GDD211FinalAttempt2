using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    //character controller
    public CharacterController Cc;
    public Transform cameraTransform;
    public float Gravity;
    public float WalkSpeed;
    private float yspeed;
    //score
    private float score = 0;
    [SerializeField] TextMeshProUGUI scoreScavenged;
    //timer
    private float timer = 60;
    [SerializeField] TextMeshProUGUI timeLeft;
    //mini game tags & objects
    public string scrap = "Scrap";
    public GameObject weldingGame;
    public string wires = "Wires";
    public GameObject wireGame;
    public string keypad = "Keypad";
    public GameObject keypadGame;
    //Buttons
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;
    public GameObject button5;
    public string heating = "Heating";
    public GameObject heatGame;
    //public GameObject weldingTool;
    private bool inGame = false; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
    }
     private void HandleRaycasting() {
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit)) {
                if(hit.collider.CompareTag(scrap)){
                    score++;
                   PlayMiniGame(scrap);
                } else if (hit.collider.CompareTag(wires)){
                    score++;
                   PlayMiniGame(wires);
                } else if (hit.collider.CompareTag(keypad)){
                    score++;
                    PlayMiniGame(keypad);
                } else if (hit.collider.CompareTag(heating)){
                    score++;
                    PlayMiniGame(heating);
                }


            }
        }
     }

     private void PlayMiniGame(string game) {
        inGame = true;
        if(game == "Scrap"){
            weldingGame.SetActive(true);
        } else if (game == "Wires") {
            wireGame.SetActive(true);
        } else if (game == "Keypad") {
            keypadGame.SetActive(true);
           /* if (Input.GetKeyDown(KeyCode.Q)){
                button1.SetActive(true);
            } */
        } else if (game == "Heating") {
            heatGame.SetActive(true);
        }

     }

    // Update is called once per frame
    void Update()
    {
        if(!inGame){
            timer -= Time.deltaTime;
            if (timer <= 0) {
                SceneManager.LoadScene(2);
            }
            timeLeft.text = timer.ToString();
            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = false;
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) );
            cameraTransform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), 0, 0));
            Vector3 move = Vector3.zero;
            //Apply walk vector
            move += Input.GetAxis("Vertical") * transform.forward;
            move += Input.GetAxis("Horizontal") * transform.right;
            move = move.normalized * WalkSpeed;
            //Apply gravity
            move += new Vector3(0, yspeed, 0);

            scoreScavenged.text = score.ToString();
            Cc.Move(move * Time.deltaTime);
            HandleRaycasting();
        }
        if (inGame) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //transform.position = Input.mousePosition;
            if(Input.GetKeyDown("escape")){
                weldingGame.SetActive(false);
                wireGame.SetActive(false);
                keypadGame.SetActive(false);
                heatGame.SetActive(false);
               // weldingTool.SetActive(false);
                inGame = false;
            }
        }
    }
}
