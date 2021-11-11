using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : MonoBehaviour
{
    [Range(85f, -85f)] public float zRot; // z회전 각도 
    public int maxCount = 3; // 벽에 반사되는 횟수

    void Update()
    {
        ZRotation();
        ShootRay(transform.position, transform.up, 1);
    }

    // 레이캐스트 발사
    // startPos : 레이캐스트의 시작 지점
    // dir : 레이캐스트 발사 방향
    // count : 레이캐스트가 벽에 반사되는 횟수
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
                    Debug.Log($"count {count} : {hit.collider.name} / {dir}" );
                    Vector2 reflectVec = Vector2.Reflect(dir, hit.normal);
                    ShootRay(hit.point, reflectVec, count, hit);
                    break;
                }
            }
        }
    }

    // 오브젝트의 회전 
    void ZRotation()
    {
        transform.eulerAngles = new Vector3(0, 0, zRot);
    }
}
