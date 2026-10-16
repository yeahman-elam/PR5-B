using UnityEngine;
using UnityEngine.InputSystem;

public class HandlePlanet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartupPlanet();
    }

    // Update is called once per frame
    void Update()
    {
        var kb = Keyboard.current;
        if (kb.rKey.wasPressedThisFrame) { StartupPlanet(); }

        Heuristic();
    }

    public void Heuristic()
    {
        //var continuous = actionsOut.ContinuousActions;
        var kb = Keyboard.current;
        if (kb == null) return;

        var rot = transform.localRotation;
        float zoutput = (kb.aKey.isPressed ? 1f : 0f) - (kb.dKey.isPressed ? 1f : 0f);
        float xoutput = (kb.wKey.isPressed ? 1f : 0f) - (kb.sKey.isPressed ? 1f : 0f);
        transform.localRotation = Quaternion.Euler(rot.x + xoutput, rot.y, rot.z + zoutput);
    }

    void StartupPlanet()
    {
        transform.localRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }
}
