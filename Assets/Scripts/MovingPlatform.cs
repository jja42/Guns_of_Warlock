using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    Vector2 position_1;
    Vector2 position_2;
    bool target_position; //false is 1, true is 2
    public float speed;
    private float moveTime;
    private float distance; //Distance between the positions.
    float lerpval;
    // Start is called before the first frame update
    void Start()
    {
        target_position = false;
        position_1 = left.transform.position;
        position_2 = right.transform.position;
        distance = Vector3.Distance(position_1, position_2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!target_position)
        {
            lerpval = (Time.time - moveTime) * speed / distance;
            transform.position = Vector3.Lerp(position_1, position_2, lerpval);
        }
        if (target_position)
        {
            lerpval = (Time.time - moveTime) * speed / distance;
            transform.position = Vector3.Lerp(position_2, position_1, lerpval);
        }
        if (Vector3.Distance(transform.position, position_2) <= .01f)
        {
            moveTime = Time.time;
            target_position = true;
        }
        if (Vector3.Distance(transform.position, position_1) <= .01f)
        {
            moveTime = Time.time;
            target_position = false;
        }
    }
}