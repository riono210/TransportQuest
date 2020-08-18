using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollierTrigger : MonoBehaviour {

    //　カメラ外になったら消す
    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Stage") {
            Debug.Log ("Delete");
            Destroy (other.transform.parent.gameObject);
        }
    }
}