using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 GetMouseWorldPosition(Camera camera)
    {
        return GetMouseWorldPosition(camera, Vector3.one);
    }

    public static Vector3 GetMouseWorldPositionDropY(Camera camera)
    {
        return GetMouseWorldPosition(camera, new Vector3(1, 0, 1));
    }

    private static Vector3 GetMouseWorldPosition(Camera camera, Vector3 mask)
    {
        Vector3 worldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        return Vector3.Scale(worldPos, mask);
    }

    public static bool GetMouseWorldPositionRaycast(out Vector3 position)
    {
        return GetMouseWorldPositionRaycast(out position, ~0);
    }

    public static bool GetMouseWorldPositionRaycast(out Vector3 position, LayerMask layerMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;

        if (Physics.Raycast(ray, out hitData, 1000, layerMask))
        {
            position = hitData.point;
            return true;
        }

        position = Vector3.zero;
        return false;
    }



}
