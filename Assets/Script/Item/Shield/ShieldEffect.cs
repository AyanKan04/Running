using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : MonoBehaviour
{
    public GameObject activeShield;

    void Update()
    {
        activeShield.transform.Rotate(0, 360 * Time.deltaTime, 0);

        if (PlayerMovement.isShield)
        {

            if (PlayerMovement.offShield)
            {
                StartCoroutine(ActionShield());
            }
            else 
            {
                activeShield.SetActive(true);
            }
        }
        else
        {
            activeShield.SetActive(false);
        }
    }

    private IEnumerator ActionShield()
    {
        activeShield.SetActive(false);
        yield return new WaitForSeconds(1f);
        activeShield.SetActive(true);
    }
}
