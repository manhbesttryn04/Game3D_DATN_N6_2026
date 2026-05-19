using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Giao diện UI Con")]
    [Tooltip("Kéo Object nhóm con (chứa toàn bộ nút, nền...) vào đây")]
    public GameObject pauseMenuGroup;

    // Biến lưu trữ Panel Options tự động tìm kiếm
    private GameObject optionsPanel;
    private bool isPaused = false;

    void Start()
    {
        // 1. Tự động tìm kiếm Panel có tên chính xác là "PanelOptions" trong Scene
        optionsPanel = GameObject.Find("PanelOptions");

        // 2. Mới vào game, ẩn cả nhóm Pause và nhóm Options đi
        if (pauseMenuGroup != null)
        {
            pauseMenuGroup.SetActive(false);
        }

        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy Object nào tên là 'PanelOptions' trong Scene này!");
        }

        Time.timeScale = 1f;
        isPaused = false;
    }

    void Update()
    {
        // Bắt sự kiện bấm nút ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Nếu đang mở bảng Options mà bấm ESC thì quay về Pause
            if (optionsPanel != null && optionsPanel.activeSelf)
            {
                BackToPauseFromOptions();
            }
            // Nếu đang Pause bình thường thì tiếp tục game
            else if (isPaused)
            {
                ContinueGame();
            }
            // Nếu đang chơi thì Pause game
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenuGroup == null) return;
        pauseMenuGroup.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ContinueGame()
    {
        if (pauseMenuGroup == null) return;

        // Ẩn cả 2 đề phòng trường hợp đang ở Options mà bấm Continue
        pauseMenuGroup.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    // --- TÍNH NĂNG MỚI BỔ SUNG ---

    // Hàm gán vào nút "Options" trên bảng Pause
    public void OpenOptions()
    {
        // Nếu không tìm thấy ở Start, thử tìm lại một lần nữa
        if (optionsPanel == null) optionsPanel = GameObject.Find("PanelOptions");

        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);    // Hiện bảng Options lên
            if (pauseMenuGroup != null)
            {
                pauseMenuGroup.SetActive(false); // Ẩn nhóm Pause đi
            }
        }
    }

    // Hàm gán vào nút "Exit" trên bảng Options để quay lại Pause
    public void BackToPauseFromOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);   // Ẩn bảng Options đi
        }
        if (pauseMenuGroup != null)
        {
            pauseMenuGroup.SetActive(true);  // Hiện lại nhóm Pause
        }
    }
}