using UnityEngine;
using TMPro; // Bắt buộc phải có để dùng TextMeshPro

public class CountdownTimer : MonoBehaviour
{
    [Header("Cấu hình thời gian")]
    [Tooltip("Số giây đếm ngược ban đầu")]
    public float timeRemaining = 60f;
    public bool timerIsRunning = false;

    [Header("Thành phần UI")]
    [Tooltip("Kéo Object TextMeshPro vào đây")]
    public TextMeshProUGUI timeText;

    void Start()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Hết giờ!");
                timeRemaining = 0;
                timerIsRunning = false;
                DisplayTime(timeRemaining);

                OnTimerEnd();
            }
        }
    }

    // Hàm đã sửa: Chỉ hiển thị số giây lẻ
    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0) timeToDisplay = 0;

        // Làm tròn lên số nguyên gần nhất (ví dụ: 60.3 giây thành 60 giây)
        int seconds = Mathf.CeilToInt(timeToDisplay);

        // Hiển thị trực tiếp số giây lên màn hình dưới dạng chữ (string)
        timeText.text = seconds.ToString();
    }

    void OnTimerEnd()
    {
        timeText.text = "0"; // Hoặc chữ "K.O!", "HẾT GIỜ!" tùy bạn muốn
    }
}