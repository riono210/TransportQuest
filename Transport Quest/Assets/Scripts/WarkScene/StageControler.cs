﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControler : MonoBehaviour {

    private float stageSpeed; // ステージ自体の速度
    private Vector3 pos;
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start () {
        pos = Vector3.zero;
        rigidbody = GetComponent<Rigidbody> ();
        //StageMove ();
    }

    // Update is called once per frame
    void FixedUpdate () {
        StageMove ();
    }

    // ステージを動かす
    private void StageMove () {
        pos.x = stageSpeed;
        //rigidbody.AddForce (pos, ForceMode.VelocityChange);
        rigidbody.velocity = pos;
    }

    // スピードの設定
    public void SetSpeed (float moveSpeed) {
        this.stageSpeed = moveSpeed;
    }
}