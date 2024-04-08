using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public static class AIGeneral
{
    public static bool AgentIsAtDestinationPoint(NavMeshAgent agent, float threshold = 0.25f)
    {
        return Vector3.Distance(agent.pathEndPosition, agent.transform.position) < threshold;
    }

    public static void CreatePath(NavMeshAgent agent, Vector3 target)
    {
        if (!agent.isOnNavMesh) return;

        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target, path);
        agent.path = path;
    }

    public static NavMeshPath GetPath(NavMeshAgent agent, Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target, path);
        return path;
    }

    public static void LookAt(Transform self, Transform target)
    {
        LookAt(self, target.position);
    }

    public static void LookAt(Transform self, Vector3 target)
    {
        Vector3 playerDir = (target - self.position).normalized;
        self.rotation = Quaternion.LookRotation(Vector3.up, playerDir);
    }

    public static void LookAt(Transform self, Transform target, float step, bool inverse=true)
    {
        LookAt(self, target.position, step, inverse);
    }

    public static void LookAt(Transform self, Vector3 target, float step, bool inverse = true)
    {
        Vector3 playerDir = (target - self.position).normalized;
        Quaternion desiredRotation = Quaternion.LookRotation(Vector3.up, playerDir);
        if (inverse)
            desiredRotation = Quaternion.Euler(desiredRotation.eulerAngles.x, desiredRotation.eulerAngles.y, desiredRotation.eulerAngles.z - 180);
        self.rotation = Quaternion.RotateTowards(self.rotation, desiredRotation, step * Time.deltaTime);
    }

    public static bool IsInsideVisionCon(Vector2 targetPoint, Vector2 position, Vector2 direction, float range, float radius)
    {
        return IsPointInsideCone(targetPoint, position, direction, range/2, radius);
    }

    public static bool IsPointInsideCone(Vector3 point, Vector3 coneOrigin, Vector3 coneDirection, float maxAngle, float maxDistance)
    {
        var distanceToConeOrigin = (point - coneOrigin).magnitude;
        if (distanceToConeOrigin < maxDistance)
        {
            var pointDirection = point - coneOrigin;
            var angle = Vector3.Angle(coneDirection, pointDirection);
            if (angle < maxAngle)
                return true;
        }
        return false;
    }


    public static Vector2 InsideCirlce(float innerRadius, float outerRadius)
    {
        float angle = Random.Range(0, 359);
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
        return direction * Random.Range(innerRadius, outerRadius);
    }

    public static Vector2 InsideCirlce(float innerRadius, float outerRadius, float alpha, float beta)
    {
        float angle = Random.Range(alpha, beta);
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
        return direction * Random.Range(innerRadius, outerRadius);
    }

    public static Vector2 InsideCirlce(float innerRadius, float outerRadius, Vector2 direction, float angle)
    {
        angle = Random.Range(0, angle);
        direction = Quaternion.Euler(0, 0, angle) * direction;
        return direction * Random.Range(innerRadius, outerRadius);
    }

    public static Vector2 GetPositionByRaycast(Vector2 position, float distance, float angle, float hitOffset, LayerMask collisionLayerMask)
    {
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, collisionLayerMask);
        if (hit)
        {
            return position + direction * hit.distance - direction * hitOffset;
        }
        else
            return position + direction * distance;
    }

    public static bool TargetIsVisible(Vector2 position, Transform target, float distance, LayerMask collisionLayerMask)
    {
        Vector2 direction = ((Vector2)target.position - position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, collisionLayerMask);
        return hit && hit.transform == target;
    }

    public static bool TargetIsVisible(Vector2 position, Transform target, float distance, float width, LayerMask collisionLayerMask)
    {
        Vector2 direction = ((Vector2)target.position - position).normalized;
        Vector2 normalToDirection = Vector2.Perpendicular(direction);
        Vector2 directionRight = ((Vector2)target.position - (position + normalToDirection * width)).normalized;
        Vector2 directionLeft = ((Vector2)target.position - (position - normalToDirection * width)).normalized;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, collisionLayerMask);
        RaycastHit2D hitRight = Physics2D.Raycast(position + normalToDirection * width, directionRight, distance, collisionLayerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(position - normalToDirection * width, directionLeft, distance, collisionLayerMask);

        // Debug.DrawRay(position, direction * hit.distance, Color.white);
        // Debug.DrawRay(position + normalToDirection * width, directionRight * hitRight.distance, Color.cyan);
        // Debug.DrawRay(position - normalToDirection * width, directionLeft * hitLeft.distance, Color.yellow);

        return (hit && hit.transform == target) && (hitRight && hitRight.transform == target) && (hitLeft && hitLeft.transform == target);
    }

    public static bool PointIsReachable(NavMeshAgent agent, Vector3 point, out NavMeshPath path)
    {
        path = new NavMeshPath();
        return agent.CalculatePath(point, path) && path.status == NavMeshPathStatus.PathComplete;
    }
}

