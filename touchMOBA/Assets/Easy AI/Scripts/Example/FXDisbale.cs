using UnityEngine;
using System.Collections;

public class FXDisbale : MonoBehaviour {

    void Start() {

        Invoke("fxdisbale", 1f);
    }
    void OnEnable() {
        Invoke("fxdisbale", 1f);
    }
    void fxdisbale() {

        this.gameObject.SetActive(false);  
    }
}
