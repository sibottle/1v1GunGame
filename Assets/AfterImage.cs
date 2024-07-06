using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AfterImage : MonoBehaviour //잔상 효과
{
    [SerializeField] private float rate = 2f; //잔상 나타나는 속도
    [SerializeField] private float lifeTime = 0.2f; //잔상 남아있는 시간

    private SpriteRenderer baseRenderer; //잔상 효과 적용할 스프라이트
    private bool isActive = false;
    private float interval;
    private Vector3 previousPos;

    private void Start()
    {
        baseRenderer = GetComponent<SpriteRenderer>();
        interval = 1f / rate; //잔상을 나타네는 간격 설정
    }

    private void Update()
    {  
        //간격 밖으로 거리가 나가면 잔상 효과 스폰
        if (isActive && Vector3.Distance(previousPos, transform.position) > interval)
        {
            SpawnTrailPart();
            previousPos = transform.position;
        }
    }
    
    public void Activate(bool shouldActivate)
    {
        isActive = shouldActivate;
        if (isActive)
            previousPos = transform.position;
    }

    //잔상 소환
    private void SpawnTrailPart()
    {
        GameObject trailPart = new GameObject(gameObject.name + " trail part");

        // Sprite renderer
        SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer>();
        CopySpriteRenderer(trailPartRenderer, baseRenderer);

        // Transform
        trailPart.transform.position = transform.position;
        trailPart.transform.rotation = transform.rotation;
        trailPart.transform.localScale = transform.lossyScale;

        // Sprite rotation
        //trailPart.AddComponent<CameraSpriteRotater>();

        // Fade & Destroy
        StartCoroutine(FadeTrailPart(trailPartRenderer));
    }

    //소환된 잔상 lifeTime이랑 같이 계산해 사라지게 함
    private IEnumerator FadeTrailPart(SpriteRenderer trailPartRenderer)
    {
        float fadeSpeed = 1 / lifeTime;

        while(trailPartRenderer.color.a > 0)
        {
            Color color = trailPartRenderer.color;
            color.a -= fadeSpeed * Time.deltaTime;
            trailPartRenderer.color = color;

            yield return new WaitForEndOfFrame();
        }

        Destroy(trailPartRenderer.gameObject);
    }

    private static void CopySpriteRenderer(SpriteRenderer copy, SpriteRenderer original)
    {
        // Can modify to only copy what you need!
        copy.sprite = original.sprite;
        copy.flipX = original.flipX;
        copy.flipY = original.flipY;
        copy.sortingLayerID = original.sortingLayerID;
        copy.sortingLayerName = original.sortingLayerName;
        copy.sortingOrder = original.sortingOrder;
    }
}
