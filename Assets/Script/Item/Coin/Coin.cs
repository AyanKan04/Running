using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool destroyCoin = false;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(50 * Time.deltaTime, 0, 0);
        if (destroyCoin)
        {
            transform.position += new Vector3(10 * Time.deltaTime, 20 * Time.deltaTime, 0);
            if (transform.position.y >= 4)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.TakeCoin(1);
            UIManager.TakeCoinShop(1);
            destroyCoin = true;
            Debug.Log("Coin: " + PlayerManager.coin);
            AudioManager.instance.PlaySFX("Pick Coin");
        }
    }
}
