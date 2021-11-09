using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private GameObject whiteBall;
    [SerializeField] private LineRenderer lineRenderer;
    private Vector3 startPos;
    private int reflectCount = 3;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            startPos = Input.mousePosition;
            StartCoroutine(CoWaitMouseUp());
        }
    }

    IEnumerator CoWaitMouseUp()
    {
        while (true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                break;
            }
            
            var endPos = Input.mousePosition;
            var dir = startPos - endPos;
            var originPos = whiteBall.transform.position;
            reflectCount = 0;
            RayCastReflect(originPos, dir);
            /*
            var rayHits = Physics2D.RaycastAll(originPos,dir);
            foreach (var rayHit in rayHits)
            {
                if (rayHit.collider == null)
                {
                    continue;
                }

                // 충돌지점
                var hitPos = rayHit.point;

                var incidence = hitPos - (Vector2)originPos;

                var hitNormalized = rayHit.point.normalized;
                var reflectVector = Vector3.Reflect(incidence, rayHit.normal);
                Debug.DrawRay(rayHit.point,reflectVector,Color.yellow,10);
            }
            Debug.DrawRay(originPos,dir);
            */
            yield return null;
        }
    }

    void RayCastReflect(Vector3 originPos, Vector3 dir)
    {
        if (reflectCount >= 3)
        {
            return;
        }

        reflectCount++;
        var results = Physics2D.RaycastAll(originPos, dir);
        foreach (var rayHit in results)
        {
            if (rayHit.collider == null)
            {
                continue;
            }

            // 충돌지점
            var hitPos = rayHit.point;
            var incidence = hitPos - (Vector2)originPos;
            var hitNormalized = rayHit.point.normalized;
            var reflectVector = Vector3.Reflect(incidence, rayHit.normal);
            RayCastReflect(rayHit.point, reflectVector);
            Debug.DrawRay(rayHit.point,reflectVector);

        }
        Debug.DrawRay(originPos,dir);
    }
}
