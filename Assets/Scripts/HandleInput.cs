using UnityEngine;
using UnityEngine.InputSystem;

public class HandleInput : MonoBehaviour
{
    private bool setup = false;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        DoSetup();
    }

    void Update()
    {
        if (!setup) { DoSetup(); }
    }

    void DoSetup()
    {
        var g = GameHandler.instance;
        if (g == null) { return; }

        var slot = g.curGamepad;
        for (int i = 0; i < 4; i++)
        {
            if (g.gamepad[i] == null) { slot = i; break; }
        }

        g.gamepad[slot] = GetComponent<PlayerInput>();
        g.curGamepad++;

        setup = true;
    }
}
