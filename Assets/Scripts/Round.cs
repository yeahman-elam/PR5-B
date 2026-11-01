using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Round : MonoBehaviour
{
    public float myTime;
    public GameObject timer;
    public TextMeshProUGUI[] pointTally;
    public int[] points = new int[4];
    [HideInInspector] public int winnerSlot;

    public static Round instance;
    private float startTimer = 3;
    public bool gamePlaying = false;

    public GameObject obj_countdown;

    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        var prevStartTime = Mathf.Floor(startTimer);
        startTimer -= Time.deltaTime; 
        if (startTimer > 0 || myTime <= 0) 
        {
            if (Mathf.Floor(startTimer) != prevStartTime && startTimer <= 3 && startTimer > 0)
            {
                var countdown = Instantiate(obj_countdown, new Vector3(0, 0, 0), Quaternion.identity, timer.transform.parent);
                countdown.GetComponent<Countdown>().value = Mathf.FloorToInt(startTimer);
            }
            gamePlaying = false; return; 
        }
        if (Mathf.Floor(startTimer) != prevStartTime && startTimer > -1 && startTimer <= 0)
        {
            print("GO!");
            var countdownA = Instantiate(obj_countdown, new Vector3(-45f, 0, 0), Quaternion.identity, timer.transform.parent);
            countdownA.GetComponent<Countdown>().value = 3;
            var countdownB = Instantiate(obj_countdown, new Vector3(45f, 0, 0), Quaternion.identity, timer.transform.parent);
            countdownB.GetComponent<Countdown>().value = 4;
        }

        gamePlaying = true;
        myTime -= Time.deltaTime;
        timer.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = LeadingZero(Mathf.CeilToInt(myTime), 3);
    }

    public static void UpdatePoints(int slot, int points)
    {
        var r = Round.instance;

        int prevWinner = r.winnerSlot;
        r.points[slot] += points;
        r.winnerSlot = Round.UpdateWinner();

        r.pointTally[slot].text = ""+r.points[slot];

        if (prevWinner != r.winnerSlot) { Round.UpdateColor(r.winnerSlot+1); }
    }

    public static int UpdateWinner()
    {
        var r = Round.instance;

        var retList = new List<int>();
        for (int i = 0; i < r.points.Length; i++) { retList.Add(r.points[i]); }
        retList.Sort();

        bool pickedUp = false; int slot = -1;
        for (int i = 0; i < r.points.Length; i++) 
        {
            if (r.points[i] == retList[3]) 
            {
                if (pickedUp){ return -1; } 
                pickedUp = true; slot = i;
            } 
        }
        return slot;
    }

    public static void UpdateColor(int slot)
    {
        Color32 myCol;
        switch (slot)
        {
            default:
                myCol = new Color32(255, 255, 255, 255); break;
            case (1):
                myCol = new Color32(255, 0, 0, 255); break;
            case (2):
                myCol = new Color32(0, 255, 255, 255); break;
            case (3):
                myCol = new Color32(255, 255, 0, 255); break;
            case (4):
                myCol = new Color32(0, 255, 0, 255); break;
        }

        Round.instance.timer.transform.GetChild(0).gameObject.GetComponent<Image>().color = myCol;
    }

    public string LeadingZero(int number, int zeros)
    {
        var output = ""; var iterator = 0;
        while (iterator < zeros - number.ToString().Length) { output += "0"; iterator++; }
        output += number.ToString();
        return output;
    }
}
