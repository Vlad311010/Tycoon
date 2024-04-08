using UnityEngine;
using UnityEngine.AI;

public class AIAnimations : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    // [SerializeField] movem

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        animator.SetBool("Walk", agent.velocity.magnitude > 1);
    }
}
