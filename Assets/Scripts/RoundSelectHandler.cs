using UnityEngine;
using TMPro;

public class RoundSelectHandler : MonoBehaviour
{
    public GameObject[] obj = new GameObject[4];
    public GameObject readier;
    public static RoundSelectHandler instance;
    public int activeLen;
    public bool update = false;

    public bool fullReady;
    public bool roundStarted;

    public GameObject transition;

    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (update)
        {
            Resort();
            int readiness = 0;
            for (int i = 0; i < 4; i ++)
            {
                if (roundStarted) { obj[i].GetComponent<SelectSlot>().shake = 1.5f; Invoke("DoTransition", .5f); SetupGamehandler(); }

                if (obj[i].GetComponent<SelectSlot>().slotUse != -2)
                {
                    activeLen = i + 1;
                    if (obj[i].GetComponent<SelectSlot>().ready || obj[i].GetComponent<SelectSlot>().slotUse == -1) { readiness += 1; }
                }
                else { readiness += 1; }
            }

            if (readiness == 4 && activeLen >= 2) { fullReady = true; } else { fullReady = false; }
            if (!fullReady) { readier.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color32(205, 205, 205, 255); readier.transform.GetChild(1).gameObject.GetComponent<TMP_shake>().shakeMagnitude = 0; }
            else { readier.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255); readier.transform.GetChild(1).gameObject.GetComponent<TMP_shake>().shakeMagnitude = .5f; }

            update = false;
        }
    }

    void Resort()
    {
        for (int i = 0; i < 4; i++)
        {
            if (obj[i].GetComponent<SelectSlot>().slotUse != -2 && obj[Mathf.Clamp(i - 1, 0, 4)].GetComponent<SelectSlot>().slotUse == -2)
            {
                var prevId = obj[i].GetComponent<SelectSlot>();
                var copy_cpuLevel = prevId.cpuLevel;
                var copy_slotUse = prevId.slotUse;

                obj[Mathf.Clamp(i - 1, 0, 4)].GetComponent<SelectSlot>().cpuLevel = copy_cpuLevel;
                obj[Mathf.Clamp(i - 1, 0, 4)].GetComponent<SelectSlot>().slotUse = copy_slotUse;

                prevId.slotUse = -2;
                prevId.reset = true;
            }
        }
    }

    void DoTransition()
    {
        transition.GetComponent<Transition>().active = true;
    }

    void SetupGamehandler()
    {
        for (int i = 0; i < 4; i++)
        {
            GameHandler.instance.gamepadSetup[i] = obj[i].GetComponent<SelectSlot>().slotUse;
        }
    }
}