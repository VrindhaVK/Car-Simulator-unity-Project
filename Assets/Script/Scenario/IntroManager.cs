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

    [Header("Keyboard Test Buttons")]
    public KeyCode englishKeyboardKey = KeyCode.E;
    public KeyCode germanKeyboardKey = KeyCode.G;
    public KeyCode skipIntroKey = KeyCode.S;

    [Header("Steering Wheel Buttons")]
    public KeyCode englishWheelButton = KeyCode.JoystickButton0;
    public KeyCode germanWheelButton = KeyCode.JoystickButton1;
    public KeyCode skipWheelButton = KeyCode.JoystickButton2;

    private bool languageSelected = false;
    private Coroutine introRoutine;

    void Start()
    {
        DisableDriving();

        if (languagePanel != null)
            languagePanel.SetActive(true);

        UpdateLanguageText();
    }

    void Update()
    {
        if (!languageSelected)
        {
            if (Input.GetKeyDown(englishKeyboardKey) || Input.GetKeyDown(englishWheelButton))
            {
                languageSelected = true;
                introRoutine = StartCoroutine(PlayIntro(englishIntro));
            }

            if (Input.GetKeyDown(germanKeyboardKey) || Input.GetKeyDown(germanWheelButton))
            {
                languageSelected = true;
                introRoutine = StartCoroutine(PlayIntro(germanIntro));
            }
        }

        if (languageSelected && (Input.GetKeyDown(skipIntroKey) || Input.GetKeyDown(skipWheelButton)))
        {
            SkipIntro();
        }
    }

    private void UpdateLanguageText()
    {
        if (languageText == null) return;

        languageText.text =
            "<size=130%><b>SELECT LANGUAGE</b></size>\n" +
            "<size=110%>SPRACHE WÄHLEN</size>\n\n" +
            "<b>English</b>\n" +
            "Keyboard: E | Wheel: Button 1\n\n" +
            "<b>Deutsch</b>\n" +
            "Keyboard: G | Wheel: Button 2\n\n" +
            "<size=80%>Skip Intro: S | Wheel Button 3</size>";
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

    private void SkipIntro()
    {
        if (introRoutine != null)
            StopCoroutine(introRoutine);

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        if (languagePanel != null)
            languagePanel.SetActive(false);

        EnableDriving();

        Debug.Log("Intro skipped");
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