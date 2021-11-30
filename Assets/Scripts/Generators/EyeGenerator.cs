using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeGenerator : MonoBehaviour {
    public GameObject eyeModel;
    public Transform midiTransform;

    public int numberOfEyes = 400;
    public float timeBetweenBlinkAttempts = 0.2f;
    public float chanceOfBlink = 0.025f;
    public float driftSpeedBase = 0.2f;
    public float driftStrengthBase = 1.5f;
    public float growthSpeedBase = 100f;
    public float growthStrengthBase = 1.5f;

    private struct Eye {
        public GameObject gameObject;
        public Animation animation;
        public Vector3 startPosition;
    }

    private float blinkAttemptTimer = 0f;
    private List<Eye> eyes = new List<Eye>();

    void Start() {
        for (int i = 0; i < numberOfEyes; i++) {
            Eye eye;
            eye.gameObject = Instantiate(
                eyeModel,
                new Vector3(0, 0, 0),
                Quaternion.identity
            );
            eye.animation = eye.gameObject.GetComponent<Animation>();

            if (Random.Range(0f, 1f) > 0.75) {
                eye.gameObject.transform.position = new Vector3(
                    Random.Range(-200f, 200f),
                    Random.Range(-200f, 200f),
                    Random.Range(-5f, 150f)
                );
            } else {
                eye.gameObject.transform.position = new Vector3(
                    Random.Range(-75f, 75f),
                    Random.Range(-75f, 75f),
                    Random.Range(-5f, 75f)
                );                
            }
            eye.startPosition = eye.gameObject.transform.position;

            eyes.Add(eye);
        }
    }

    void Update() {
        blinkAttemptTimer += Time.deltaTime;

        for (int i = 0; i < eyes.Count; i++) {
            eyes[i].gameObject.transform.LookAt(Camera.main.transform.position, -Vector3.up);
            eyes[i].gameObject.transform.position = eyes[i].startPosition + new Vector3(
                Mathf.Sin(
                    (Time.time * driftSpeedBase) +
                    eyes[i].startPosition.x
                ) * driftStrengthBase,
                Mathf.Sin(
                    (Time.time * driftSpeedBase) +
                    eyes[i].startPosition.y
                ) * driftStrengthBase,
                Mathf.Sin(
                    (Time.time * driftSpeedBase) +
                    eyes[i].startPosition.z    
                ) * driftStrengthBase
            );
            eyes[i].gameObject.transform.localScale = Vector3.MoveTowards(
                eyes[i].gameObject.transform.localScale,
                new Vector3(1f, 1f, 1f) + new Vector3(
                    midiTransform.position.x * growthStrengthBase,
                    midiTransform.position.y * growthStrengthBase,
                    midiTransform.position.z * growthStrengthBase
                ),
                (growthSpeedBase * Time.deltaTime)
            );
            if (blinkAttemptTimer > timeBetweenBlinkAttempts) {
                if (Random.Range(0f, 1f) > (1f - chanceOfBlink)) {
                    eyes[i].animation.Play("Scene");
                }
            }
        }

        if (blinkAttemptTimer > timeBetweenBlinkAttempts) {
            blinkAttemptTimer = 0;
        }
    }
}
