using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandlePlanetAgent : Agent
{
    //idk if this works at all lol

    [SerializeField] public Transform target;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Renderer floor;
    [SerializeField] private Color winColour = Color.green;
    [SerializeField] private Color loseColour = Color.red;
    [SerializeField] private Color idleColour = Color.grey;
    private HandlePlanet gameplay;
    public float agentAddBonus = 0;
    private bool hasCharged = false;
    private int hits;

    public override void OnEpisodeBegin()
    {
        //target.localRotation = Quaternion.Euler(Random.Range(0, 360), 0, Random.Range(0, 360));
        //transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);

        if (gameplay.DoAgent) { target.localRotation = Quaternion.Euler(Random.Range(0, 360), 0, Random.Range(0, 360)); transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0); }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.GetChild(0).localRotation); // 3
        sensor.AddObservation(target.localRotation); // 3
    }

    public override void Initialize() // runs once at startup
    {
        floor.material.color = idleColour;
        gameplay = GetComponent<HandlePlanet>();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (!gameplay.isCpu && !gameplay.DoAgent) { print($"isNotCpu {gameplay.slot}"); return; }

        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        transform.GetChild(0).Rotate(moveX * moveSpeed * Time.deltaTime, 0, moveZ * moveSpeed * Time.deltaTime);

        // Small penalty every step so it learns to hurry
        if (MaxStep > 0) AddReward(-1f / MaxStep);
    }

    void Update()
    {
        if (gameplay.charge >= 1) { floor.material.color = winColour; AddReward(.75f); End(); EndEpisode(); floor.material.color = winColour; gameplay.charge = 0; }
        if (gameplay.charge > 0 && !hasCharged) { AddReward(0.5f/(hits+1)); hasCharged = true; hits += 1; }
        if (gameplay.charge <= 0 && hasCharged) { AddReward(-0.15f*(hits+1));  hasCharged = false; }
        if (agentAddBonus != 0)
        {
            AddReward(agentAddBonus);
            agentAddBonus = 0;
        }
    }

    void End()
    {
        if (!gameplay.DoAgent) { return; }

        hasCharged = false;
        gameplay.shiner.Stop();
        gameplay.exploder.Play();
        target.localRotation = Quaternion.Euler(Random.Range(0, 360), 0, Random.Range(0, 360));
        gameplay.myMatRef.color = gameplay.myCol;
        gameplay.charge = 0;
        hits = 0;

        if (Round.instance == null) { return; }
        Round.UpdatePoints(gameplay.slot - 1, 1);
        gameplay.shake = 0.25f;
    }
}
