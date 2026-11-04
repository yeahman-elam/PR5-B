using UnityEngine;

public class Cursor : MonoBehaviour
{
    void OnTriggerStay(Collider other) //make a checker for the cursor itself.... i think it cant check because the light is a child of the object that handles the script
    {
        if (!GetComponentInParent<HandlePlanet>().DoAgent)
        {
            if (other.gameObject.name == "LIGHT_COLLIDER" && Round.instance.gamePlaying)
            {
                if (GetComponentInParent<HandlePlanet>().charge == 0) { GetComponentInParent<HandlePlanet>().shiner.Play(); }
                GetComponentInParent<HandlePlanet>().charge += 1 * Time.deltaTime;
            }
        }
        else
        {
            if (other.CompareTag("Target"))
            {
                GetComponentInParent<HandlePlanet>().charge += 1 * Time.deltaTime;
                print("cpu hit light!! | " + GetComponentInParent<HandlePlanet>().charge);
            }
        }
    }
}
