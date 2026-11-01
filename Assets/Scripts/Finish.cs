using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Finish : MonoBehaviour
{
    public GameObject finishTxt;
    public GameObject txt;
    public GameObject fade;

    private bool fading = false;
    private bool displaytxt = false;

    private Vector3[] originalPos = new Vector3[6];
    private float[] shake = new float[6];

    private GameObject[] objList = new GameObject[6];
    private float alpha = 0;

    private float cur;
    private bool roundExists = false;

    void Start()
    {
        Invoke("DisplayTxt", 2f);
        Invoke("BeginFade", 4f);
        for (var i = 0; i < objList.Length; i++)
        {
            objList[i] = finishTxt.transform.GetChild(i).gameObject;
            originalPos[i] = objList[i].transform.position;
        }

        if (Round.instance != null) { roundExists = true; }
    }

    // Update is called once per frame
    void Update()
    {
        cur += 12 * Time.deltaTime;

        for (int i = 0; i < Mathf.Clamp(Mathf.FloorToInt(cur), 0, 6); i++)
        {
            var prevScale = objList[i].transform.localScale; print(prevScale.x);
            if (prevScale.x != 0.65) { objList[i].transform.localScale -= new Vector3(10 * Time.deltaTime, 10 * Time.deltaTime, 1); }
            if (objList[i].transform.localScale.x <= 0.65f && prevScale.x != 0.65f) { shake[i] = .5f; objList[i].transform.localScale = new Vector3(0.65f, 0.65f, 1); }
        }

        if (displaytxt) { txt.SetActive(true); }
        if (fading) { alpha += 0.5f * Time.deltaTime; fade.GetComponent<Image>().color = new Vector4(0, 0, 0, alpha); }

        if (alpha >= 1.5f)
        {
            SceneManager.LoadScene("RoundSelect");
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < 6; i++)
        {
            objList[i].transform.position = originalPos[i]+(new Vector3(Mathf.Sign(Random.Range(-1, 1)) * shake[i], Mathf.Sign(Random.Range(-1, 1)) * shake[i], 1));
            shake[i] *= -0.8f;
        }
    }

    void DisplayTxt() 
    {
        displaytxt = true; txt.SetActive(true);

        if (roundExists) 
        {
            var s = Round.instance.winnerSlot;
            if (s == -1) { txt.GetComponent<TextMeshProUGUI>().text = "TIE"; return; }
            txt.GetComponent<TextMeshProUGUI>().text = $"PLAYER {s+1} WINS"; 
        }
        //
    }
    void BeginFade() { fading = true; fade.SetActive(true); }
}
