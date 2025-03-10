using UnityEngine;

public class Honey : MonoBehaviour
{
    public bool taken = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bear" && (!taken))
        {
            taken = true;

            Destroy(this.gameObject);
        }
    }
}
