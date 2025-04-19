using System.Collections;
using UnityEngine;

public class Pollen : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bee" && other.gameObject.GetComponent<Bee>().GetHunger() < 20)
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
