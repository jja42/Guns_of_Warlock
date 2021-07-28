using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform position_1;
    public Transform position_2;
    bool target_position; //false is 1, true is 2
    public float speed;
    private float moveTime;
    private float distance; //Distance between the positions.
    float lerpval;
    // Start is called before the first frame update
    void Start()
    {
        target_position = false;
        distance = Vector3.Distance(position_1.position, position_2.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!target_position)
        {
            lerpval = (Time.time - moveTime) * speed / distance;  
            transform.position = Vector3.Lerp(position_1.position, position_2.position, lerpval);
        }
        if (target_position)
        {
            lerpval = (Time.time - moveTime) * speed / distance;
            transform.position = Vector3.Lerp(position_2.position, position_1.position, lerpval);
        }
        if (Vector3.Distance(transform.position, position_2.position) <= .01f)
        {
            moveTime = Time.time;
            target_position = true;
        }
        if (Vector3.Distance(transform.position, position_1.position) <= .01f)
        {
            moveTime = Time.time;
            target_position = false;
        }
    }
}
