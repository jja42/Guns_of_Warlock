using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float rightLimit;
    GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        var playerPos = player.transform.position;
        var cameraPos = transform.position;

        if (playerPos.x > 0 && playerPos.x < rightLimit) {
            cameraPos.x = playerPos.x;
            transform.position = cameraPos;
        }

    }
}