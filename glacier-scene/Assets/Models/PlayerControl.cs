using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance;

    private Rigidbody rBody;
    private Animator ani;
    private AudioSource footstepAudioSource;

    [Header("移动设置")]
    [SerializeField] private float walkSpeed = 2.5f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] private bool reverseModelForward = false;

    [Header("摄像头设置")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float cameraVerticalSensitivity = 20f;
    [SerializeField] private float minXRotation = -45f;
    [SerializeField] private float maxXRotation = 45f;
    private float currentCameraXRotation = 0f;

    [Header("音频设置")]
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float footstepVolume = 0.5f;

    // 游戏结束标记
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        footstepAudioSource = gameObject.AddComponent<AudioSource>();
        footstepAudioSource.clip = footstepClip;
        footstepAudioSource.volume = footstepVolume;
        footstepAudioSource.loop = true;
        footstepAudioSource.playOnAwake = false;
    }

    void Update()
    {
        // 游戏结束 → 禁用所有输入
        if (isGameOver) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        HandleMovementAndAnimation(horizontal, vertical);
        HandleMouseRotation(mouseX, mouseY);
    }

    private void HandleMovementAndAnimation(float horizontal, float vertical)
    {
        float finalVertical = reverseModelForward ? -vertical : vertical;

        bool hasInput = !Mathf.Approximately(horizontal, 0) || !Mathf.Approximately(finalVertical, 0);
        bool isRunning = hasInput && Input.GetKey(KeyCode.LeftShift);

        if (!hasInput)
        {
            ani.SetBool("Walk", false);
            ani.SetBool("Run", false);
            rBody.velocity = new Vector3(0, rBody.velocity.y, 0);
            StopFootstepAudio();
            return;
        }

        if (!isRunning)
        {
            ani.SetBool("Walk", true);
            ani.SetBool("Run", false);
        }
        else
        {
            ani.SetBool("Walk", false);
            ani.SetBool("Run", true);
        }

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 moveDir = transform.forward * finalVertical + transform.right * horizontal;
        moveDir.y = 0;
        moveDir.Normalize();

        rBody.MovePosition(transform.position + moveDir * currentSpeed * Time.deltaTime);
        PlayFootstepAudio();
    }

    private void HandleMouseRotation(float mouseX, float mouseY)
    {
        transform.Rotate(Vector3.up, mouseX * rotateSpeed * Time.deltaTime);
        currentCameraXRotation -= mouseY * cameraVerticalSensitivity * Time.deltaTime;
        currentCameraXRotation = Mathf.Clamp(currentCameraXRotation, minXRotation, maxXRotation);
        cameraTransform.localEulerAngles = new Vector3(currentCameraXRotation, 0f, 0f);
    }

    private void PlayFootstepAudio()
    {
        // 游戏结束 → 绝对不播放音效
        if (isGameOver || footstepClip == null) return;
        if (!footstepAudioSource.isPlaying)
            footstepAudioSource.Play();
    }

    public void StopFootstepAudio()
    {
        if (footstepAudioSource != null)
        {
            footstepAudioSource.Stop();
            footstepAudioSource.loop = false; // 强制关闭循环
        }
    }

    // 游戏结束调用这个方法 → 彻底锁死脚步声
    public void OnGameOver()
    {
        isGameOver = true;
        StopFootstepAudio();
        // 直接销毁音效组件，让它彻底没声音
        Destroy(footstepAudioSource);
    }

    private void OnAnimatorMove()
    {
        if (ani.applyRootMotion) ani.applyRootMotion = false;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (isGameOver) return;

        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnApplicationQuit()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}