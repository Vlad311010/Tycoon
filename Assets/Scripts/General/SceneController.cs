using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static void BakeNavMesh(bool immediate = false)
    {
        if (immediate)
        {
            return;
        }
    }

    public static void Pause()
    {
        Time.timeScale = 0f;
        // GameObject player = GameObject.FindGameObjectWithTag("Player");
    }

    public static void Resume()
    {
        Time.timeScale = 1f;
        // GameObject player = GameObject.FindGameObjectWithTag("Player");
    }

}
