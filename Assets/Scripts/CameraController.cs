using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController instance;
    public float rightLimit;
    public float leftLimit;
    public float upwardLimit; //should be 0 by default
    GameObject player;
    bool init;

    void Start() {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(leftLimit, 0, -10);
    }

    void Update() {
        if (!init)
        {
            transform.position = new Vector3(player.transform.position.x, 0, -10);
            init = true;
        }
        var playerPos = player.transform.position;
        var cameraPos = transform.position;

        if (playerPos.x > leftLimit && playerPos.x < rightLimit) {
            cameraPos.x = playerPos.x;
            transform.position = cameraPos;
        }

        if (playerPos.y > upwardLimit) {

            cameraPos.y = playerPos.y;
            transform.position = cameraPos;

        }
    }
}