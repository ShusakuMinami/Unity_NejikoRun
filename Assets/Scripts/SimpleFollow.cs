using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    Vector3 diff;
    
    // 追従ターゲットパラメータ
    public GameObject target;
    public float followSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        // 追従距離の計算
        diff = target.transform.position - transform.position;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            target.transform.position - diff,
            Time.deltaTime * followSpeed
        );
    }
}
