using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WindowLoadSceneAction : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] int sceneIdx;
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(sceneIdx);
    }
}
