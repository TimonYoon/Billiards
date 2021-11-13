using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect2D : MonoBehaviour
{
    [Range(85f, -85f)] public float zRot; // z회전 각도 
    public int maxCount = 3; // 벽에 반사되는 횟수

    void Update()
    {
        ZRotation();
        ShootRay(transform.position, transform.up, 1);
    }
    
    /// <summary>
    /// 레이캐스트 발사해서 충돌되는 타겟 체크
    /// </summary>
    /// <param name="startPos">레이케스트 시작 지점</param>
    /// <param name="dir">레이캐스트 발사 뱡향</param>
    /// <param name="count">레이캐스트가 벽에 반사된 횟수</param>
    /// <param name="startHit">2D Raycast 경우 발사하는 본인이 hit되는 경우가 있어서 </param>
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
