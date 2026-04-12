using UnityEngine;

public class EndDoor : MonoBehaviour
{
    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (used || !other.CompareTag("Player")) return;

        if (GameManager.Instance.IsBeanCollectedEnough())
        {
            GameManager.Instance.GameSuccess();
            used = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        if (col == null) return;

        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(col.center, col.size);
    }
}