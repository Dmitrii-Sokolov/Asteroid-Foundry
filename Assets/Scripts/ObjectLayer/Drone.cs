using UnityEngine;

public class Drone : MonoBehaviour
{
    private DroneStateMachine DroneStateMachine;

    private void Awake()
    {
        DroneStateMachine = new();
    }

    private void Start()
    {
        DroneStateMachine.ChangeState<Idle>();
    }

    private void Update()
    {
        DroneStateMachine.Check();
    }
}

