using UnityEngine;
using UnityEngine.UI;

public class EndNPC : MonoBehaviour
{
    [Header("检测范围")]
    public float talkRange = 3f;

    [Header("对话文本")]
    public Text npcText;
    public string notEnoughText = "You haven't collected all the soybeans yet! Keep looking！";
    public string enoughText = "Congratulations! You've collected all the soybeans! You can go open the door now.";

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (npcText != null) npcText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        bool isNear = dist <= talkRange;

        if (isNear)
        {
            ShowDialog();
        }
        else
        {
 
            HideDialog();
        }
    }

    void ShowDialog()
    {
        if (npcText == null) return;

        bool ok = GameManager.Instance.IsBeanCollectedEnough();
        npcText.text = ok ? enoughText : notEnoughText;
        npcText.gameObject.SetActive(true);
    }

    void HideDialog()
    {
        if (npcText != null)
            npcText.gameObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, talkRange);
    }
}