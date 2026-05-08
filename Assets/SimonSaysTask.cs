using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SimonSaysTask : MonoBehaviour
{
    public List<RawImage> buttonImages; // Drag your 5 UI Images here
    public Color flashColor = Color.white;
    public PlayerScript playerScript;

    private List<int> sequence = new List<int>();
    private int playerStep = 0;
    private bool acceptingInput = false;

    void OnEnable() { StartGame(); }

    public void StartGame()
    {
        sequence.Clear();
        playerStep = 0;
        AddStep();
    }

    void AddStep()
    {
        sequence.Add(Random.Range(0, buttonImages.Count));
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        acceptingInput = false;
        yield return new WaitForSeconds(0.5f);
        foreach (int index in sequence)
        {
            yield return StartCoroutine(FlashRoutine(buttonImages[index]));
            yield return new WaitForSeconds(0.3f);
        }
        acceptingInput = true;
    }

    IEnumerator FlashRoutine(RawImage img)
    {
        Color original = img.color;
        img.color = flashColor;
        yield return new WaitForSeconds(0.4f);
        img.color = original;
    }

    public void OnImageClick(int index) // Call this via Event Trigger
    {
        if (!acceptingInput) return;

        if (index == sequence[playerStep])
        {
            StartCoroutine(FlashRoutine(buttonImages[index]));
            playerStep++;
            if (playerStep >= sequence.Count)
            {
                if (sequence.Count >= 5) { playerScript.RegisterConnection(); }
                else { playerStep = 0; Invoke("AddStep", 1f); }
            }
        }
        else { StartGame(); } // Reset on mistake
    }
}