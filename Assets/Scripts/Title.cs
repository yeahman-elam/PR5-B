using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Title : MonoBehaviour
{
    public GameObject spin;
    private float spd = 0.03f;
    private float scaleSpd = 0.03f;
    private bool started;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            var scale = spin.transform.localScale.x;

            spd += 0.05f; scaleSpd += 0.06f;
            spin.transform.localScale = new Vector3(scale + scaleSpd, scale + scaleSpd, scale + scaleSpd);
            spin.transform.Rotate(0, spd, 0);

            var col = spin.GetComponent<Renderer>().material.color; var bow = col[0] - (0.4f * Time.deltaTime);
            spin.GetComponent<Renderer>().material.color = new Vector4(bow, bow, bow, 255);
            //scale 675
            if (spin.transform.localScale[0] >= 675) { SceneManager.LoadScene("RoundSelect"); }
            return;
        }

        spin.transform.Rotate(0, spd, 0);
        for (int i = 0; i < spin.transform.childCount; i++)
        {
            Vector3 spinner = spin.transform.GetChild(i).localPosition;
            spin.transform.GetChild(i).localPosition = new Vector3(spinner.x, 0 + Function.Sine(0.02f, 2f, i/1.15f), spinner.z);
        }

        var myKb = Keyboard.current;
        if (myKb.spaceKey.wasPressedThisFrame)
        {
            scaleSpd = -1.5f;
            started = true;
        }
    }
}
