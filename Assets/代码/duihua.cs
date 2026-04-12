using UnityEngine;
using UnityEngine.UI;

public class duihua : MonoBehaviour
{
    [Header("基础配置")]
    public float triggerDistance = 3f; // 玩家靠近触发距离（建议3-5）
    public Text npcHeadText; // 你已写好的NPC头顶文字组件（直接赋值）
    public AudioClip dialogueAudio; // 对话音频（无则留空）

    private AudioSource audioSource; // 音频播放组件
    private Transform playerTransform; // 玩家位置缓存
    private bool isPlayerNear = false; // 玩家是否在范围内
    private bool hasPlayedAudio = false; // 避免音频重复播放


    void Start()
    {
        if (npcHeadText != null)
        {
            npcHeadText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("请赋值NPC头顶文字组件（npcHeadText）！");
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.loop = false; // 音频不循环


        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("未找到Tag为'Player'的玩家对象！");
        }
    }


    void Update()
    {
        // 玩家或文字组件未配置时，不执行逻辑
        if (playerTransform == null || npcHeadText == null) return;

        // 检测玩家与NPC的距离
        CheckPlayerDistance();

        // 玩家靠近：显示文字+播放一次音频
        if (isPlayerNear)
        {
            npcHeadText.gameObject.SetActive(true);
            if (dialogueAudio != null && !hasPlayedAudio)
            {
                PlayDialogueAudio();
                hasPlayedAudio = true; // 标记音频已播放，避免重复触发
            }
        }
        // 玩家远离：隐藏文字+重置音频状态
        else
        {
            npcHeadText.gameObject.SetActive(false);
            hasPlayedAudio = false; // 重置，玩家再次靠近可重新播放
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }


    /// 检测玩家是否在触发距离内
    void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        isPlayerNear = distance <= triggerDistance;
    }


    /// 播放对话音频
    void PlayDialogueAudio()
    {
        audioSource.clip = dialogueAudio;
        audioSource.Play();
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, triggerDistance);
    }
}
