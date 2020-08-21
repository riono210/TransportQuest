using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorControler : MonoBehaviour {

    private Animator animator;
    private float stageSpeed; // アニメーションの速度用

    private RaycastHit hit;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        GroundCheck ();
    }

    private void GroundCheck () {
        if (Physics.Raycast (this.transform.position, Vector3.down, out hit, 10f)) {
            if(hit.collider.gameObject.tag != "Untagged")
            Debug.Log ("tag: " + hit.collider.gameObject.tag);
        }
    }
}