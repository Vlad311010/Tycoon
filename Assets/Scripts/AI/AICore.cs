using Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class AICore : MonoBehaviour
{
    public CustomerSO CustomerData { get => customerData; }
    public NavMeshAgent Agent { get => agent; }

    NavMeshAgent agent;

    [SerializeField] LayerMask floorLayerMask;    
    [SerializeField] State currentState;

    public bool isActive = true;
    public bool rotateTowardWaypoint = false;
    public Transform lookAt;
    
    private CustomerSO customerData;
    private Vector3 defaultPos;
    private State startState;


    private void Awake()
    {
        startState = currentState;
        defaultPos = transform.position;
        lookAt = transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    /*private void Start()
    {
        Initialize();
    }*/

    private void Update()
    {
        if (!isActive) return;

        if (rotateTowardWaypoint)
        {
            Vector3 lookAtPosition = transform.forward;
            if (agent.path.corners.Length > 1)
                lookAtPosition = agent.path.corners[1];
            else if (lookAt != null)
                lookAtPosition = lookAt.position;

            AIGeneral.LookAt(transform, lookAtPosition, agent.angularSpeed);
        }

        SwitchState(currentState.UpdateState(this));
        currentState.ExecuteState(this);
    }

    public void Initialize(CustomerSO customerDataVariant)
    {
        customerData = CustomerSO.CreateInstance(customerDataVariant);
        currentState = startState;
        currentState.OnStateEnter(this);
        agent.enabled = true;
        isActive = true;
    }

    public void Despawn()
    {
        transform.position = defaultPos;
        agent.enabled = false;
        isActive = false;
    }

    private void SwitchState(State newState)
    {
        if (currentState != newState)
        {
            // Debug.Log(currentState .ToString() + " -> " + newState.ToString());
            currentState.OnStateExit(this);
            currentState = newState;
            newState.OnStateEnter(this);
        }
    }

    public void ForceStateChange(State newState)
    {
        currentState.OnStateExit(this);
        currentState = newState;
        newState.OnStateEnter(this);
    }

    public void Interact(float time, IInteractable interactable, Action<AICore> action)
    {
        StartCoroutine(InteractionCoroutine(time, interactable, action));
    }

    private IEnumerator InteractionCoroutine(float time, IInteractable interactable, Action<AICore> action)
    {
        yield return new WaitForSeconds(time);
        interactable.Interact(this);
        action(this);
    }

    public void GetGoods(IInteractable interactable)
    {
        interactable.Interact(this);
    }

    public IEnumerable<GoodsContainer> AvailableGoodsContainers(int availableMoney)
    {
        return ShopManager.current.Containers
            .Where(
            c => c.ItemMinimalPrice() <= availableMoney 
            && AIGeneral.PointIsReachable(agent, c.transform.position, out UnityEngine.AI.NavMeshPath path));
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
