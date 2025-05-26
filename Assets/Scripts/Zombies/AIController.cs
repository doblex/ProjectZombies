using System;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using utilities.Controllers;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AnimatorController))]
[RequireComponent(typeof(HealthController))]
public class AIController : MonoBehaviour
{
    [SerializeField] StateDefinition defaultState;
    [SerializeField] StateDefinition deathState;

    [SerializeField] Animator animator;
    NavMeshAgent agent;
    HealthController healthController;

    Transform player;

    State currentState;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthController = GetComponent<HealthController>();
        healthController.onDeath += OnDeath;

        currentState = defaultState.CreateState(gameObject,agent,animator);
    }

    private void OnDeath()
    {
        currentState = deathState.CreateState(gameObject, agent, animator);
    }

    private void Update()
    {
        currentState = currentState.Process();
    }
}
