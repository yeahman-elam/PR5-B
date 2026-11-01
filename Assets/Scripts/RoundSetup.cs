using UnityEngine;

public class RoundSetup : MonoBehaviour
{
    public GameObject[] presets;

    void Start()
    {
        var count = 0;
        for (int i = 0; i < 4; i++)
        {
            if (GameHandler.instance.gamepadSetup[i] != -2) { count += 1; }
        }
        Instantiate(presets[count-1], new Vector3(0, 0, 0), Quaternion.identity);
    }
}
