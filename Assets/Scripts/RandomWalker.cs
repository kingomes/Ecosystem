using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class RandomWalker : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private float xCoordinate = 0f;
    private float incrementAmount = 0.001f;
    private Vector3 currentPosition;
    void Start() {
        xCoordinate = 70f;
        Debug.Log("Start!");
    }

    // Update is called once per frame
    void Update() {
        currentPosition = this.transform.position;

        float perlinValue = Mathf.PerlinNoise(xCoordinate, 0);
        Debug.Log(perlinValue);
        xCoordinate += incrementAmount;
        float newX = Unity.Mathematics.math.remap(0f, 1f, 0f, 10f, perlinValue);
        this.transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        if (this.transform.position.x < currentPosition.x) {
            this.transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else if (this.transform.position.x > currentPosition.x) {
            this.transform.eulerAngles = new Vector3(0, 90, 0);
        }
    }
}
