using UnityEngine;

public class Function : MonoBehaviour
{
    public static float Sine(float magnitude, float speed, float timeOffset = 0f)
    {
        return magnitude * Mathf.Sin((Time.time + timeOffset) * speed);
    }

    public static float Approach(float start, float final, float amount)
    {
        var a = start;
        if (start < final)
        {
            start += amount * (Time.deltaTime * 1000);
            if (start > final) { return final; }
        }
        if (start > final)
        {
            start -= amount * (Time.deltaTime * 1000);
            if (start < final) { return final; }
        }
        return start;
    }
}
