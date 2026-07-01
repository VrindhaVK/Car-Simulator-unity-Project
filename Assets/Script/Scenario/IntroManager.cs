using System.Collections;
using UnityEngine;
using TMPro;

public class IntroManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip englishIntro;
    public AudioClip germanIntro;

    [Header("UI")]
    public GameObject languagePanel;
    public TMP_Text languageText;

    [Header("Driving Control")]
    public MonoBehaviour[] drivingScriptsToDisable;

    [Header("Steering Wheel Buttons")]
    public KeyCode englishButton = KeyCode.JoystickButton0;
    public KeyCode germanButton = KeyCode.JoystickButton1;

    private bool languageSelected = false;

    void Start()
    {
        DisableDriving();

        if (languagePanel != null)
            languagePanel.SetActive(true);

        if (languageText != null)
        {
            languageText.text =
                "Select Language / Sprache wählen\n\n" +
                "Steering Button 1 : English\n" +
                "Steering Button 2 : Deutsch";
        }
    }

    void Update()
    {
        if (languageSelected) return;

        if (Input.GetKeyDown(englishButton))
        {
            languageSelected = true;
            StartCoroutine(PlayIntro(englishIntro));
        }

        if (Input.GetKeyDown(germanButton))
        {
            languageSelected = true;
            StartCoroutine(PlayIntro(germanIntro));
        }
    }

    private IEnumerator PlayIntro(AudioClip clip)
    {
        if (languagePanel != null)
            languagePanel.SetActive(false);

        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitForSeconds(clip.length);
        }

        EnableDriving();
    }

    private void DisableDriving()
    {
        foreach (MonoBehaviour script in drivingScriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }
    }

    private void EnableDriving()
    {
        foreach (MonoBehaviour script in drivingScriptsToDisable)
        {
            if (script != null)
                script.enabled = true;
        }
    }
}