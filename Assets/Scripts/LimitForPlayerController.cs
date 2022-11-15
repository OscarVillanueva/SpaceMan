using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitForPlayerController : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

}
