using UnityEngine;

public class TextureScroll : MonoBehaviour
{

    [SerializeField] Vector2 scroll;

    Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();    
    }

    private void Update()
    {
        Scroll();
    }

    private void Scroll()
    {
        renderer.material.mainTextureOffset += scroll * Time.deltaTime;
    }


}
