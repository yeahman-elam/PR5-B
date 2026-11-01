using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectSlot : MonoBehaviour
{
    public int mySlot;
    public int slotUse = -2;

    public bool ready;
    public int cpuLevel = 1;

    public GameObject[] text;
    public GameObject backer;
    public GameObject[] cpu;

    public Sprite[] spr;

    public bool reset;
    public float shake;

    private int prevSlot;
    private Vector3 startPos;

    public bool canSelect;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (slotUse != -2)
        {
            SetupVisual(slotUse);
            if (slotUse != -1) 
            {
                text[0].SetActive(false); cpu[0].SetActive(false); cpu[1].transform.GetChild(0).gameObject.SetActive(false); SetupText("PRESS A BUTTON TO JOIN", "READY");
                cpu[1].transform.GetChild(1).gameObject.SetActive(ready);
            }
            else
            {
                SetupText("\nCPU", "CPU LEVEL"); cpu[1].transform.GetChild(0).gameObject.SetActive(true);
                cpu[1].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "" + cpuLevel;
                cpu[1].transform.GetChild(1).gameObject.SetActive(false);
                cpu[0].gameObject.SetActive(true);
            }
            reset = true;
        }
        else
        {
            if (reset)
            {
                SetupVisual(slotUse);
                SetupText("PRESS A BUTTON TO JOIN", "");
                cpu[1].transform.GetChild(0).gameObject.SetActive(false);
                cpu[1].transform.GetChild(1).gameObject.SetActive(false);
                cpu[0].gameObject.SetActive(true);
                ready = false; cpuLevel = 1;
            }
            reset = false;
        }

        canSelect = (mySlot <= RoundSelectHandler.instance.activeLen);
        if (!canSelect) { cpu[0].gameObject.SetActive(false); SetupText("", ""); reset = true; }

        cpu[0].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = spr[slotUse == -1 ? 1 : 0];
        if (slotUse != prevSlot) { shake = 2f; }
        prevSlot = slotUse;
    }

    void FixedUpdate()
    {
        shake *= -0.8f;
        transform.localPosition = new Vector3(startPos[0] + (shake * Mathf.Sign(Random.Range(-1, 1))), startPos[1] + (shake * Mathf.Sign(Random.Range(-1, 1))), 0);
    }

    void SetupVisual(int slot)
    {
        Color32 outputCol;
        switch (slot)
        {
            case (-2):
                outputCol = new Color32(100, 100, 100, 255); break;
            case (-1):
                outputCol = new Color32(75, 75, 75, 255); break;
            default:
                outputCol = new Color32(255, 100, 100, 255); break;
            case (1):
                outputCol = new Color32(100, 255, 255, 255); break;
            case (2):
                outputCol = new Color32(255, 255, 100, 255); break;
            case (3):
                outputCol = new Color32(100, 255, 100, 255); break;
        }

        backer.GetComponent<Image>().color = outputCol;
    }

    void SetupText(string set1, string set2)
    {
        text[0].GetComponent<TextMeshProUGUI>().text = set1; text[1].GetComponent<TextMeshProUGUI>().text = set2;
    }
}
