using UnityEngine;

public class BearCam : MonoBehaviour
{
    public Transform bear;

    // Update is called once per frame
    void Update()
    {
        if (bear != null) {
            this.transform.position = bear.position + new Vector3(0, 10, 15);
        }
    }
}
