using UnityEngine;
using TMPro;

public class IntersectionManager : MonoBehaviour
{
    public NPCController npc;
    public RightLookChecker rightLookChecker;

    public GameObject assessmentPanel;
    public TMP_Text titleText;
    public TMP_Text messageText;

    public Color passColor = Color.green;
    public Color failColor = Color.red;

    public float speedLimitKmh = 30f;

    private bool assessmentActive = false;
    private bool npcInIntersection = false;
    private bool speedViolation = false;
    private bool rightOfWayViolation = false;
    private bool observationViolation = false;

    public void StartAssessment()
    {
        assessmentActive = true;

        if (rightLookChecker != null)
            rightLookChecker.StartChecking();
    }

    public void StartNPC()
    {
        if (!assessmentActive) return;

        if (npc != null)
            npc.StartDriving();
    }

    public void CheckSpeed(float currentSpeedKmh)
    {
        if (!assessmentActive) return;

        if (currentSpeedKmh > speedLimitKmh)
            speedViolation = true;
    }

    public void NPCEnteredIntersection()
    {
        npcInIntersection = true;
    }

    public void NPCExitedIntersection()
    {
        npcInIntersection = false;
    }

    public void PlayerEnteredIntersection()
    {
        if (!assessmentActive) return;

        if (rightLookChecker != null)
        {
            rightLookChecker.StopChecking();

            if (!rightLookChecker.HasLookedRight())
                observationViolation = true;
        }

        if (npcInIntersection)
            rightOfWayViolation = true;
    }

    public void FinishAssessment()
    {
        assessmentActive = false;
    }

    public bool HasSpeedViolation()
    {
        return speedViolation;
    }

    public bool HasRightOfWayViolation()
    {
        return rightOfWayViolation;
    }

    public bool HasObservationViolation()
    {
        return observationViolation;
    }

    public void ShowMessage(string title, string message, bool passed)
    {
        if (assessmentPanel != null)
            assessmentPanel.SetActive(true);

        if (titleText != null)
        {
            titleText.text = title;
            titleText.color = passed ? passColor : failColor;
        }

        if (messageText != null)
            messageText.text = message;
    }
}