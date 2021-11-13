using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private Ball whiteBall;
    private LineRenderer lineRenderer;
    private Vector3 inputStartPos;
    private int maxCount = 3;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {            
            inputStartPos = Input.mousePosition;
            StartCoroutine(CoWaitMouseUp());
        }
    }

    IEnumerator CoWaitMouseUp()
    {
        while (true)
        {
            var inputEndPos = Input.mousePosition;
            var dir = inputStartPos - inputEndPos;
            if (Input.GetMouseButtonUp(0))
            {
                var power = dir.magnitude * 0.01f;
                whiteBall.SetDir(dir, power);
                break;
            }           
            
            var startPos = whiteBall.transform.position;
            ShootRay(startPos, dir / dir.magnitude,1);
            yield return null;
        }
    }

   
    void ShootRay(Vector2 startPos, Vector2 dir,  int count, RaycastHit2D startHit = default)
    {
        RaycastHit2D[] hits = new RaycastHit2D[2];
        if (0 < Physics2D.RaycastNonAlloc(startPos, dir, hits))
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit.transform == null)
                {
                    continue;
                }

                if (whiteBall.transform.GetInstanceID() == hit.transform.GetInstanceID())
                {
                    continue;
                }
                
                if (startHit.transform != null)
                {
                    if (startHit.transform.GetInstanceID() == hit.transform.GetInstanceID())
                    {
                        continue;
                    }
                }
                
                float distance = Vector2.Distance(startPos, hit.point);
                Debug.DrawRay(startPos, dir * distance, Color.red);

                if (count <= maxCount)
                {
                    count++;
                    Vector2 reflectVec = Vector2.Reflect(dir, hit.normal);
                    ShootRay(hit.point, reflectVec, count, hit);
                    break;
                }
            }
        }
    }
}
