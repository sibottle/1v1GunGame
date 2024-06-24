using UnityEngine;
using UnityEngine.UI;

public class DisappearOffscreen : MonoBehaviour
{
    RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RendererExtensions.IsVisibleFrom(rectTransform, Camera.main))
            Destroy(gameObject);
    }
}
