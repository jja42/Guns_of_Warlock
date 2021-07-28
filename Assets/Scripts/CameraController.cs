using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float rightLimit;
    public float leftLimit;
    public float upwardLimit; //should be 0 by default
    GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(leftLimit, 0, -10);
    }

    void Update() {
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