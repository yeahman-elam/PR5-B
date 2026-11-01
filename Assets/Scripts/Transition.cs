using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    private float spd;
    public string sceneId;
    public bool active = false;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) { return; }

        spd += 2f * Time.deltaTime;
        transform.position += new Vector3(spd, 0, 0);
        if (transform.position.x >= 1080) { SceneManager.LoadScene(sceneId); }
    }
}
