using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect3D : MonoBehaviour
{
    [Range(85f, -85f)]
    public float zRot;              // z회전 각도 
    public int maxCount = 3;        // 벽에 반사되는 횟수

    void Update()
    {        
        ZRotation();
        ShootRay(transform.position, transform.up, 1);
    }

    // 레이캐스트 발사
    // startPos : 레이캐스트의 시작 지점
    // dir : 레이캐스트 발사 방향
    // count : 레이캐스트가 벽에 반사되는 횟수
    void ShootRay(Vector3 startPos, Vector3 dir, int count)
    {
        RaycastHit hit;

        if (Physics.Raycast(startPos, dir, out hit))
        {
            float distance = Vector3.Magnitude(startPos - hit.point);
            Debug.DrawRay(startPos, dir * distance, Color.red);

            if (count <= maxCount)
            {
                count++;

                Vector3 reflectVec = Vector3.Reflect(dir, hit.normal);
                ShootRay(hit.point, reflectVec, count);
            }
        }
    }

    // 오브젝트의 회전 
    void ZRotation()
    {
        transform.eulerAngles = new Vector3(0, 0, zRot);
    }
}
