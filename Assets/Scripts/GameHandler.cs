using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance = null;
    public int[] gamepadSetup = new int[4];
    public PlayerInput[] gamepad = new PlayerInput[4];
    public int curGamepad = 0;

    public enum Selection
    {
        AddPlayer,
        AddCPU,
        StartGame,
        Ready,
        Start
    }

    void Start()
    {
        if (GameHandler.instance == null) { GameHandler.instance = this; } else { Destroy(gameObject); return; }
        DontDestroyOnLoad(this.gameObject);
            
        if (SceneManager.GetActiveScene().name == "Init") { SceneManager.LoadScene("Title"); }

        gamepadSetup[0] = -1; gamepadSetup[1] = -1; gamepadSetup[2] = -1; gamepadSetup[3] = -1;
    }
}
