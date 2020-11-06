using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // 跟随目标
    public Transform target;
    // 边界
    public float MinX;
    public float MaxX;

    // 变成石头对象
    public GameObject nextPrefab;

    // Start is called before the first frame update
    void Start()
    {
        var obj = Instantiate(nextPrefab);
        target = obj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // 获取当前摄像机
        Vector3 v = transform.position;
        v.x = target.position.x;
        if (v.x > MaxX)
        {
            v.x = MaxX;
        } 
        else if(v.x < MinX)
        {
            v.x = MinX;
        }
        transform.position = v;
    }
}
