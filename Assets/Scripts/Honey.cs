using System.Collections;
using UnityEngine;

public class Honey : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bear")
        {
            StartCoroutine(GetEaten(5));
        }
    }

    IEnumerator GetEaten(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
