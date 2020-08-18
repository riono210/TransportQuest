using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour {

    private Transform parent;
    private Rigidbody rigidbody;
    private Vector3 newPos;
    private float speed = 0;

    // Start is called before the first frame update
    void Start () {
        //Debug.Log ("bullet" + transform.parent.name);
        rigidbody = this.GetComponent<Rigidbody> ();
        newPos = Vector3.zero;
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update () {
        // setSpeedするため
        ShotMove ();
    }

    private void ShotMove () {
        Debug.Log ("shot!");
        newPos.x = speed;
        newPos.z = 10f;
        //rigidbody.AddForce (pos, ForceMode.VelocityChange);
        rigidbody.velocity = newPos;
    }

    // スピードの設定
    public void SetSpeed (float moveSpeed) {
        this.speed = moveSpeed;
    }

    public void OnCollisionEnter (Collision other) {
        string layer = LayerMask.LayerToName (other.gameObject.layer);
        if (layer == "Bullet") {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation |
                RigidbodyConstraints.FreezePositionZ |
                RigidbodyConstraints.FreezePositionY;
        }
    }

}