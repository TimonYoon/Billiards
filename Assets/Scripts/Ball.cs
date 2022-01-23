using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float power = 0;

    //public float velocity = 0; // 속도
    private Vector3 dir = Vector3.zero; // 방향
    private RaycastHit2D target = default; // 충돌 예정인 타겟

    public void SetDir(Vector2 _dir, float _power)
    {
        // 방향과 속도를 기입
        // 해당 방향으로 이동
        // 타겟과 근접했을 때 반사각 구함
        // 반사되는 방향으로 이동
        // 속도가 0이 될때까지 반복

        // 예외 이동중 다른 공과 충돌시
        // 이동 방향으로 레이를 계속 체크
        // 타겟이 변경시 거리 체크
        // 타겟과 근접했다면 ?? 충돌 대상에게 반사각을 알려줌
        dir = _dir / _dir.magnitude;
        Debug.Log(dir);
        //velocity = _velocity;
        power = _power;
        //ShootRay(transform.position);
    }

    // void Update()
    // {
    //     Move();
    // }

    void LoginTest()
    {
    }
    private Vector3 preBallPosition; // 이전 Frame의 공의 위치를 저장
    private void FixedUpdate()
    {
        if ((preBallPosition != null))
        {       
            RaycastHit2D[] hits = new RaycastHit2D[3];
            var dis = Vector2.Distance(preBallPosition, transform.position);
            var fixedDir = preBallPosition - transform.position;
            if (0 < Physics2D.RaycastNonAlloc(transform.position,fixedDir , hits, dis))
            {
                foreach (var hit in hits)
                {
                    if (hit.transform == null)
                    {
                        continue;
                    }

                    if (transform.GetInstanceID() == hit.transform.GetInstanceID())
                    {
                        continue;
                    }

                    if (target.transform == null)
                    {
                        continue;
                    }

                    if (target.transform.GetInstanceID() != hit.transform.GetInstanceID())
                    {
                        continue;
                    }

                    Debug.Log("통과해버림");
                        
                    if (power > 0f)
                    {
                        Collision();
                        transform.position = (Vector3)target.point;
                    }
                    break;
                }
            }
        }
        // BallPosition을 저장한다.
        preBallPosition = transform.position;
        if (power > 0f)
        {
            ShootRay(transform.position);
            Move();
        }
    }
    
    void Move()
    {
        power -= Time.deltaTime;

        if (power > 0f)
        {
            var move = dir * Time.deltaTime * power;
            transform.position += move;
        }
    }


    void ShootRay(Vector2 startPos)
    {
        RaycastHit2D[] hits = new RaycastHit2D[3];
        if (0 == Physics2D.RaycastNonAlloc(startPos, dir, hits))
        {
            return;
        }

        foreach (var hit in hits)
        {
            if (hit.transform == null)
            {
                continue;
            }

            if (transform.GetInstanceID() == hit.transform.GetInstanceID())
            {
                continue;
            }

            if (target.transform != null)
            {
                if (target.transform.GetInstanceID() == hit.transform.GetInstanceID())
                {
                    continue;
                }
            }                

            target = hit;
            Debug.Log($"shootRay {target.collider.transform.name} // {dir}");
            var distance = Vector2.Distance(startPos, hit.point);
            Debug.DrawRay(startPos, dir * distance, Color.red);
            break;
        }
    }

    private void Collision()
    {
        
        var reflectVec = Vector2.Reflect(dir, target.normal);
        Debug.Log($"충돌 {target.collider.transform.name} // {dir} // {target.normal} // {reflectVec}");
        SetDir(reflectVec, power);
        
        var reflect = target.transform.GetComponent<Ball>();
        if (reflect == null)
        {
            return;
        }
        var endPos = target.point;
        var targetDir = (Vector2) target.transform.position - endPos;
        reflect.SetDir(targetDir, power);
    }
}