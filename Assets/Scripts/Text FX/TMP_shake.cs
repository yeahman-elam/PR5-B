using UnityEngine;
using TMPro;

public class TMP_shake : MonoBehaviour
{
    public float shakeMagnitude;

    private TMP_Text textMesh;
    private TMP_TextInfo textInfo;
    private Vector3[][] originalVertices;
    private bool setup = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }
    void Start()
    {
        textMesh = GetComponent<TMP_Text>();

        Init();
        setup = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (setup) { Init(); }
        textMesh.ForceMeshUpdate();
        textInfo = textMesh.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int meshIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            Vector3[] vertices = textInfo.meshInfo[meshIndex].vertices;
            Color32[] colors = textInfo.meshInfo[meshIndex].colors32;

            Vector3 offsetShake = new Vector3((Mathf.Sign(Random.Range(-1, 1)) * shakeMagnitude), (Mathf.Sign(Random.Range(-1, 1)) * shakeMagnitude), 0);//new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
            for (int j = 0; j < 4; j++)
            {
                vertices[vertexIndex + j] = originalVertices[meshIndex][vertexIndex + j] + (offsetShake);
            }
        }
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }

    void Init()
    {
        textMesh.ForceMeshUpdate();
        textInfo = textMesh.textInfo;
        originalVertices = new Vector3[textInfo.meshInfo.Length][];
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            originalVertices[i] = textInfo.meshInfo[i].vertices.Clone() as Vector3[];
        }
        setup = false;
    }
}
