using UnityEngine;

public class AgentrangeBonus : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            GetComponentInParent<HandlePlanetAgent>().agentAddBonus = 0.25f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            GetComponentInParent<HandlePlanetAgent>().agentAddBonus = -0.3f;
        }
    }
}
