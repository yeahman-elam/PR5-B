using UnityEngine;

public class AgentrangeBonus : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            GetComponentInParent<HandlePlanetAgent>().agentAddBonus = 0.5f;
        }
    }
}
