using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public float shakeTime;
    public float shakeAmount;
    
    private float sTime;
    private float sAmount;

    void Update() {
        if (sTime >= 0f) {
            Vector2 shakePos = Random.insideUnitCircle * sAmount;
            transform.position = new Vector3(transform.position.x + shakePos.x, transform.position.y + shakePos.y, transform.position.z);

            sTime -= Time.deltaTime;
        }
        else
            transform.position = new Vector3(11f, 6f, -1f);
    }

    public void ShakeCamera() {
        sTime = shakeTime;
        sAmount = shakeAmount;
    }
}
