using UnityEngine;
using UnityEngine.InputSystem;

public class HandlePlanet : MonoBehaviour
{
    public int slot;
    GameObject cursor;
    GameObject light;

    private Color32 myCol;
    private Material myMatRef;
    public float charge = 0;

    public ParticleSystem exploder;
    public ParticleSystem shiner;
    public GameObject cam;
    private float shake;

    private bool inputSetup = false;

    private PlayerInput input;
    private Vector3 spd;

    private bool isCpu = false;

    InputAction move;

    private bool setup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartupPlanet();
        SetColors(slot);
        exploder.GetComponent<Renderer>().material.SetColor("_FireCol", myCol);

        PullGamepadPlanet();
        if (!isCpu) { move = input.actions.FindAction("Move"); }

        myMatRef = cursor.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;

        /*
        var map = GameHandler.instance.controllerSetup;
        for (int i = 0; i < map.Length; i ++)
        {
            if (i == slot - 1) { myIndex = map[i]; return; }
        }

        //print(PlayerInput.all.Length);
        */
        
    }
    void OnAwake()
    {
        myMatRef = cursor.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Round.instance.gamePlaying && !inputSetup){ PullGamepadPlanet(); }
        var kb = Keyboard.current;
        if (kb.rKey.wasPressedThisFrame) { StartupPlanet(); }

        if (charge != 0) { myMatRef.color = new Vector4(Mathf.Clamp(myCol[0] + charge, 0, 1), Mathf.Clamp(myCol[1] + charge, 0, 1), Mathf.Clamp(myCol[2] + charge, 0, 1), 1); } else { shiner.Stop(); }
        if (charge >= 1)
        {
            shiner.Stop();
            exploder.Play();
            StartupPlanet();
            myMatRef.color = myCol;
            charge = 0;

            Round.UpdatePoints(slot - 1, 1);
            shake = 0.25f;
        }

        charge -= 0.5f * Time.deltaTime;
        charge = Mathf.Clamp(charge, 0, 1);

        if (!Round.instance.gamePlaying) { spd = new Vector3(Function.Approach(spd[0], 0, 0.5f), 0, Function.Approach(spd[2], 0, .5f)); }
        transform.GetChild(0).Rotate(spd[0], 0, spd[2]);
        Heuristic();
    }

    void FixedUpdate()
    {
        shake *= -0.8f;
        cam.transform.localPosition = new Vector3(shake * Mathf.Sign(Random.Range(-1, 1)), 1.5f, shake * Mathf.Sign(Random.Range(-1, 1)));
    }

    public void Heuristic()
    {
        if (!Round.instance.gamePlaying || isCpu || !inputSetup) { return; }

        //var continuous = actionsOut.ContinuousActions;
        var myKb = Keyboard.current;
        if (myKb == null) { return; }


        Vector2 turnValue = move.ReadValue<Vector2>();
        float zoutput = (turnValue.x) * 30 * Time.deltaTime;
        float xoutput = (-turnValue.y) * 30 * Time.deltaTime;

        spd = new Vector3(Function.Approach(spd[0], -xoutput, 1f), 0, Function.Approach(spd[2], -zoutput, 1f));
        //transform.localRotation = Quaternion.Euler(rot.x + (xoutput * 50), rot.y, rot.z + (zoutput*50));
    }

    
    void StartupPlanet()
    {
        transform.GetChild(1).localRotation = Quaternion.Euler(Random.Range(0, 360), 0, Random.Range(0, 360));
        light = transform.GetChild(1).GetChild(0).gameObject;
        cursor = transform.GetChild(0).GetChild(0).gameObject;
    }

    void SetColors(int player)
    {
        Color32 outputCol;
        switch (player)
        {
            case 0:
                outputCol = new Color32(150, 150, 150, 255); break;
            default:
                outputCol = new Color32(255, 0, 0, 255); break;
            case (2):
                outputCol = new Color32(0, 255, 255, 255); break;
            case (3):
                outputCol = new Color32(255, 255, 0, 255); break;
            case (4):
                outputCol = new Color32(0, 255, 0, 255); break;
        }


        myCol = outputCol;
        cursor.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = outputCol;
        light.GetComponent<Renderer>().material.color = new Color32(outputCol[0], outputCol[1], outputCol[2], 70);
    }

    void PullGamepadPlanet()
    {
        var g = GameHandler.instance;
        if (g == null) { inputSetup = false; return; }
        var slotUse = -1;
        for (int j = 0; j < 4; j ++)
        {
            if (g.gamepadSetup[j] == slot - 1) { slotUse = j; break; }
        }
        if (slotUse == -1) { isCpu = true; return; }

        for (int i = 0; i < 4; i++)
        {
            var output = g.gamepad[slotUse];
            if (output == null) { isCpu = true; print($"PLAYER SLOT {slot} | NO CONNECTION | CPU SLOT"); break; }
            input = output; isCpu = false; inputSetup = true; move = input.actions.FindAction("Move");
            print($"PLAYER SLOT {slot} | GOT CONNECTION | SLOT {slotUse}");
        }
        inputSetup = true;
    }

    void EndRound()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        shake = 0.5f;

        exploder.Play();
    }

    void DoSetup()
    {
        var g = GameHandler.instance;
        if (g == null) { return; }

        SetColors(g.gamepadSetup[slot] + 1);
        setup = true;
    }
}
