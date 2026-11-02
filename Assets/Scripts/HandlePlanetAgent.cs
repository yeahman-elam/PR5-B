using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandlePlanetAgent : Agent
{
    //idk if this works at all lol

    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Renderer floor;
    [SerializeField] private Color winColour = Color.green;
    [SerializeField] private Color loseColour = Color.red;
    [SerializeField] private Color idleColour = Color.grey;
    private HandlePlanet gameplay;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(
        Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 0f));
        target.localPosition = new Vector3(Random.Range(-4f, 4f), 0.5f, Random.Range(1f, 4f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localRotation); // 3
        sensor.AddObservation(target.localRotation); // 3
    }

    public override void Initialize() // runs once at startup
    {
        floor.material.color = idleColour;
        gameplay = GetComponent<HandlePlanet>();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        transform.GetChild(0).Rotate(moveX * moveSpeed * Time.deltaTime, 0, moveZ * moveSpeed * Time.deltaTime);

        // Small penalty every step so it learns to hurry
        if (MaxStep > 0) AddReward(-1f / MaxStep);
    }

    void Update()
    {
        SetReward(Mathf.Clamp(gameplay.charge, 0, 1));
        if (gameplay.charge == 1) { floor.material.color = winColour; EndEpisode(); }
    }
}
