using Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICore : MonoBehaviour
{
    public CustomerSO CustomerData { get => customerData; }
    public NavMeshAgent Agent { get => agent; }

    NavMeshAgent agent;

    [SerializeField] LayerMask floorLayerMask;    
    [SerializeField] Transform follow;    

    [SerializeField] CustomerSO customerData;


    [SerializeField] State currentState;

    public bool rotateTowardWaypoint = false;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        customerData = Instantiate(customerData);
    }

    private void Update()
    {
        if (rotateTowardWaypoint)
        {
            Vector3 lookAt = agent.path.corners.Length > 1 ? agent.path.corners[1] : transform.forward;
            AIGeneral.LookAt(transform, lookAt, agent.angularSpeed);
        }

        SwitchState(currentState.UpdateState(this));
        currentState.ExecuteState(this);
    }

    private void SwitchState(State newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            newState.OnStateEnter(this);
        }
    }

    public void GetGoods(IInteractable interactable)
    {
        interactable.Interact(this);
    }


    private void OnDrawGizmosSelected()
    {
        if (agent == null) return;

        for (int i = 0; i < agent.path.corners.Length; i++)
        {
            if (i == 0)
                Debug.DrawLine(transform.position, agent.path.corners[i], Color.red);
            else
                Debug.DrawLine(agent.path.corners[i - 1], agent.path.corners[i], Color.red);
        }
    }

}
