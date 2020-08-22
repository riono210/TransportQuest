using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoStart : MonoBehaviour {

    private bool once = false;
    [SerializeField] private GameControler controler;

    // Update is called once per frame
    void Update () {
        if (!once && Input.GetKeyDown (KeyCode.Return)) {
            controler.StartMultipul ();
            once = true;
        }
    }
}