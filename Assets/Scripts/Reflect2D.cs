using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect2D : MonoBehaviour
{
    public float power = 0;
    
    //public float velocity = 0; // 속도
    public Vector3 dir = Vector3.zero; // 방향
    public RaycastHit2D target = default; // 충돌 예정인 타겟
    public void SetDir(Vector2 _dir, float _velocity, float _power)
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
        //velocity = _velocity;
        power = _power;
    }
    void Update()
    {
        power -= Time.deltaTime;
       
        
        if (power > 0f)
        {
            ShootRay(transform.position);
            Collision();
            var move = dir * Time.deltaTime * power;
            transform.position += move;
        }
        
        
        
    }

    // 레이캐스트 발사
    // startPos : 레이캐스트의 시작 지점
    // dir : 레이캐스트 발사 방향
    // count : 레이캐스트가 벽에 반사되는 횟수
    void ShootRay(Vector2 startPos)
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
                if (transform.GetInstanceID() == hit.transform.GetInstanceID())
                {
                    continue;
                }

                target = hit;
                float distance = Vector2.Distance(startPos, hit.point);
                Debug.DrawRay(startPos, dir * distance, Color.red);
                break;
            }
        }
    }

    private void Collision()
    {
        if (target == null)
        {
            return;
        }
        var startPos = transform.position;
        var endPos = target.point;
        if (Vector2.Distance(startPos,endPos) <= 0.5f)
        {
            Debug.Log("충돌");
            Vector2 reflectVec = Vector2.Reflect(dir, target.normal);
            SetDir(reflectVec, power, power);
            var reflect = target.transform.GetComponent<Reflect2D>();
            if (reflect != null)
            {
                var colDir = (Vector2)target.transform.position - endPos;
                reflect.SetDir(colDir, power, power);
            }
        }
    }
}