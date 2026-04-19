using UnityEngine;

public class Bean5 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(5);
            Destroy(gameObject);
        }
    }
}