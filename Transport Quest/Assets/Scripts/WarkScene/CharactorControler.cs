using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorControler : MonoBehaviour {

    private Animator animator;
    private float stageSpeed; // アニメーションの速度用

    private RaycastHit hit;
    private bool isGameover;

    // Start is called before the first frame update
    void Start () {
        animator = this.GetComponent<Animator> ();
        isGameover = false;
    }

    // Update is called once per frame
    void Update () {
        GroundCheck ();
    }

    // 地面の判定
    private void GroundCheck () {
        if (Physics.Raycast (this.transform.position, Vector3.down, out hit, 10f)) {
            if (hit.collider.gameObject.tag == "Miss") {
                //Debug.Log ("tag: " + hit.collider.gameObject.tag);
                //Debug.Log ("Game over");
                isGameover = true;
            }
        }
    }

    // アニメーション変更
    public void GameoverAnimation () {
        animator.SetBool ("Miss", true);
    }

    // 走る速度変更
    public void AnimationSpeedChange (float speed) {
        animator.speed = speed;
    }

    public bool GetIsGamoOver () {
        return isGameover;
    }
}