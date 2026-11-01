using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    private Vector4 myCol;
    private float alpha = 0;
    private float scale = 2f;

    public Sprite[] sprSet;
    public int value;

    private Vector3 basePos;

    void Start()
    {
        GetComponent<Image>().sprite = sprSet[value];
        myCol = new Vector4(255/255, 229/255, 92/255, 1);

        basePos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var spd = (scale >= .5f && scale <= 1f) ? .5f : 3f;
        scale -= spd * Time.deltaTime;
        alpha += 2 * Time.deltaTime;

        transform.localPosition = new Vector3(basePos[0] + Mathf.Sign(Random.Range(-1, 1)), basePos[1] + Mathf.Sign(Random.Range(-1, 1)), 0);
        GetComponent<RectTransform>().localScale = new Vector2(scale, scale);
        GetComponent<Image>().color = new Vector4(1, 0.972f, 0.36f, alpha);
        if (scale <= 0) { Destroy(gameObject); }
    }
}
