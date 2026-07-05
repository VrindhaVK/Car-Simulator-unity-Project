using UnityEngine;

public class FinalAssessmentTrigger : MonoBehaviour
{
    public IntersectionManager intersectionManager;
    public ChildHazardManager childHazardManager;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            triggered = true;
            ShowFinalReport();
        }
    }

    private void ShowFinalReport()
    {
        int score = 100;

        bool speedViolation = intersectionManager.HasSpeedViolation();
        bool rightOfWayViolation = intersectionManager.HasRightOfWayViolation();
        bool observationViolation = intersectionManager.HasObservationViolation();
        bool npcCollision = intersectionManager.HasNpcCollisionViolation();
        bool childCollision = childHazardManager.HasChildCollision();

        string violations = "";
        string strengths = "";
        string recommendations = "";

        if (speedViolation)
        {
            score -= 20;
            violations += "- Exceeded residential speed limit (30 km/h)\n";
            recommendations += "- Maintain speeds below 30 km/h in residential areas.\n";
        }
        else strengths += "- Maintained safe residential speed\n";

        if (rightOfWayViolation)
        {
            score -= 30;
            violations += "- Failed to yield according to Right-before-Left rule\n";
            recommendations += "- Always check and yield to traffic approaching from the right.\n";
        }
        else strengths += "- Correctly yielded according to Right-before-Left rule\n";

        if (observationViolation)
        {
            score -= 10;
            violations += "- Failed to observe traffic from the right\n";
            recommendations += "- Perform a visual check before entering unsignalled intersections.\n";
        }
        else strengths += "- Observed traffic from the right\n";

        if (npcCollision)
        {
            score -= 40;
            violations += "- Collision with priority vehicle\n";
            recommendations += "- Avoid collisions with vehicles having right of way.\n";
        }
        else strengths += "- Avoided collision with priority vehicle\n";

        if (childCollision)
        {
            score -= 40;
            violations += "- Collision with child pedestrian\n";
            recommendations += "- Anticipate hidden pedestrians near parked vehicles.\n";
        }
        else strengths += "- Avoided collision with child pedestrian\n";

        strengths += "- Maintained lane position\n";

        if (score < 0) score = 0;

        bool passed = violations == "";

        string title = passed ? "ASSESSMENT PASSED" : "ASSESSMENT FAILED";

        string message =
            "<size=125%><b>Final Score: " + score + " / 100</b></size>\n\n";

        if (violations != "")
        {
            message += "<color=#FF5555><b>Violations</b></color>\n";
            message += violations + "\n";
        }

        message += "<color=#55FF55><b>Strengths</b></color>\n";
        message += strengths + "\n";

        message += "<color=#FFD700><b>Recommendations</b></color>\n";
        message += recommendations != "" ? recommendations : "Excellent defensive driving performance.";

        intersectionManager.ShowMessage(title, message, passed);
    }
}