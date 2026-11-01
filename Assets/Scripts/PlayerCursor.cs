using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static GameHandler;
using TMPro;

public class PlayerCursor : MonoBehaviour
{
    //favourite part of this game is that you can tell I gave up on the code after wasting nearly all of a 2 week break on this shitty ass project

    public int slot;
    private PlayerInput input;
    private bool inputSetup;
    private bool slotSet = false;

    InputAction move;
    InputAction confirm;

    private bool confirmBuffer = false;
    private bool confirmHeld = false;

    void Start()
    {
        SetupVisual(slot+1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputSetup) { LoadInput(); }
        if (RoundSelectHandler.instance.roundStarted) { return; }

        if (input != null)
        {
            Vector2 turnValue = move.ReadValue<Vector2>();
            float xoutput = (turnValue.x) * 150 * Time.deltaTime;
            float youtput = (turnValue.y) * 150 * Time.deltaTime;

            transform.position += new Vector3(xoutput, youtput, 0);

            if (confirm.IsPressed())
            {
                transform.localScale = new Vector3(0.2f, 0.2f, 1);
                confirmBuffer = true;
            }
            else
            {
                transform.localScale = new Vector3(0.35f, 0.35f, 1);
                transform.GetChild(0).localScale = new Vector3(1, 1, 1);
                transform.GetChild(1).localScale = new Vector3(1, 1, 1);

                confirmBuffer = false;
                confirmHeld = false;
            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            GetComponent<Image>().enabled = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (input == null || RoundSelectHandler.instance.roundStarted) { return; }
        
        if (confirmBuffer && !confirmHeld)
        {
            print("pressed");

            var id = other.GetComponent<SelectButtons>();
            var button = other.GetComponent<SelectButtons>().myId;
            if (id = null) { return; }

            if (other.GetComponent<SelectSlot>() != null)
            {
                if (!other.GetComponentInParent<SelectSlot>().canSelect) { return; }
            }

            switch (button)
            {
                case (GameHandler.Selection.AddPlayer):
                    if (!slotSet && !other.GetComponent<SelectButtons>().state)
                    {
                        other.GetComponent<SelectButtons>().state = true;
                        other.GetComponentInParent<SelectSlot>().slotUse = slot;
                        slotSet = true;
                        UpdateHandler();
                    }
                    break;
                case (GameHandler.Selection.AddCPU):
                    if (!other.GetComponent<SelectButtons>().state)
                    {
                        other.GetComponent<SelectButtons>().state = true;
                        other.GetComponentInParent<SelectSlot>().slotUse = -1;
                        UpdateHandler();
                    }
                    else
                    {
                        other.GetComponent<SelectButtons>().state = false;
                        other.GetComponentInParent<SelectSlot>().slotUse = -2;
                        UpdateHandler();
                    }
                    break;
                case (GameHandler.Selection.Ready):
                    if (other.GetComponentInParent<SelectSlot>().slotUse >= 0 && other.GetComponentInParent<SelectSlot>().slotUse == slot)
                    {
                        other.GetComponentInParent<SelectSlot>().ready = !other.GetComponentInParent<SelectSlot>().ready;
                        UpdateHandler();
                    }
                    else if (other.GetComponentInParent<SelectSlot>().slotUse == -1)
                    {
                        other.GetComponentInParent<SelectSlot>().cpuLevel++;
                        if (other.GetComponentInParent<SelectSlot>().cpuLevel >= 10){ other.GetComponentInParent<SelectSlot>().cpuLevel = 1; }
                    }
                    break;
                case (GameHandler.Selection.StartGame):
                    if (RoundSelectHandler.instance.fullReady)
                    {
                        RoundSelectHandler.instance.roundStarted = true;
                        UpdateHandler();
                    }
                    break;

            }
            confirmBuffer = false;
            confirmHeld = true;
        }
    }

    void UpdateHandler()
    {
        RoundSelectHandler.instance.update = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (input == null) { return; }
        confirmBuffer = false;
    }

    void LoadInput()
    {
        var g = GameHandler.instance;
        if (g == null) { inputSetup = false; return; }
        for (int i = 0; i < 4; i++)
        {
            var output = g.gamepad[slot];
            if (output != null) 
            {
                input = output; inputSetup = true; SetupControl();
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(true);
                GetComponent<Image>().enabled = true;
            }
        }
    }
    void SetupVisual(int player)
    {
        Color32 outputCol;
        switch (player)
        {
            default:
                outputCol = new Color32(255, 0, 0, 255); break;
            case (2):
                outputCol = new Color32(0, 255, 255, 255); break;
            case (3):
                outputCol = new Color32(255, 255, 0, 255); break;
            case (4):
                outputCol = new Color32(0, 255, 0, 255); break;
        }

        GetComponent<Image>().color = outputCol;
        transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "P" + (slot + 1);
    }
    void SetupControl()
    {
        if (input != null)
        {
            move = input.actions.FindAction("Move");
            confirm = input.actions.FindAction("Confirm");
        }
    }
}
