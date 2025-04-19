using System.Collections;
using UnityEngine;

public class Hive : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bear" && other.gameObject.GetComponent<Bear>().GetHunger() < 20)
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
