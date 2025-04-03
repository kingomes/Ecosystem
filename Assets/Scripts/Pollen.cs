using System.Collections;
using UnityEngine;

public class Pollen : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bee")
        {
            StartCoroutine(GetEaten(3));
        }
    }

    IEnumerator GetEaten(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
