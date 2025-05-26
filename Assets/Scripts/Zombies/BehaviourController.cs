using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using utilities.Controllers;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AnimatorController))]
[RequireComponent(typeof(HealthController))]
public class BehaviourController : MonoBehaviour
{
    [SerializeField] Animator animator;
    NavMeshAgent agent;
    HealthController healthController;
    BehaviourController behaviourController;

    [Header("States")]
    [SerializeField] STATE defaultState;
    [SerializeField]List<StateDefinition> stateDefinitions = new List<StateDefinition>();

    [Header("Stats")]
    [SerializeField] float visionDistance = 15.0f;
    [SerializeField] float visionAngle = 60.0f;
    [SerializeField] float shootDistance = 4.0f;

    [Header("DON'T TOUCH")]
    public List<State> states = new List<State>();
    public State currentState;

    Transform player;

    public float VisionDistance { get => visionDistance; set => visionDistance = value; }
    public float VisionAngle { get => visionAngle; set => visionAngle = value; }
    public float ShootDistance { get => shootDistance; set => shootDistance = value; }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        behaviourController = GetComponent<BehaviourController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthController = GetComponent<HealthController>();
        healthController.onDeath += OnDeath;
    }

    private void OnDeath()
    {
        behaviourController.ChangeState(STATE.DEATH);
    }


    private void Update()
    {
        if (currentState != null)
        {
            ChangeState(currentState.Process());
        }
        else
        { 
            ChangeState(defaultState);
        }
    }

    public void ChangeState(STATE stateName)
    {
        State previousState = currentState;
        currentState = GetOrAddState(stateName);

        if(currentState != previousState)
            previousState.stage = EVENT.ENTER;
    }

    private State GetOrAddState(STATE stateName)
    {
        GetState(stateName, out State retrievedState);

        return retrievedState;
    }

    public void RemoveState(STATE stateName)
    {
        foreach (var state in states)
        {
            if (state.stateName == stateName)
            {
                states.Remove(state);
                return;
            }
        }
    }

    public bool HasState(STATE stateName)
    {
        foreach (var state in states)
        {
            if (state.stateName == stateName)
            {
                return true;
            }
        }
        return false;
    }

    private void GetState(STATE stateName, out State retrievedState)
    {
        foreach (var state in states)
        {
            if (state.stateName == stateName)
            {
                retrievedState = state;
                return;
            }
        }

        retrievedState = GetStateFromDefinition(stateName);
        states.Add(retrievedState);
    }

    private State GetStateFromDefinition(STATE stateName)
    {
        foreach (var stateDefinition in stateDefinitions)
        {
            if (stateDefinition.stateName == stateName)
            {
                return stateDefinition.CreateState(gameObject, GetComponent<NavMeshAgent>(), animator , this);
            }
        }

        Debug.LogError($"StateDefinition for {stateName} not found.");
        return null;
    }
}

