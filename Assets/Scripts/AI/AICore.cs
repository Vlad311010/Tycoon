using UnityEngine;
using UnityEngine.AI;

public class AICore : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] LayerMask floorLayerMask;    
    [SerializeField] Transform follow;    
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.C))
        {
            agent.destination = follow.position;
            Debug.Log(agent.path.status);
            
        }
    }
}
