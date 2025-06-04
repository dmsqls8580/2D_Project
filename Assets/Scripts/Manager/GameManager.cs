using UnityEngine;

// ���� ��ü�� �����ϴ� ���� �Ŵ��� Ŭ����
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController player { get; private set; } // �÷��̾� ��Ʈ�ѷ� (�б� ���� ������Ƽ)
    private ResourceController _playerResourceController;

    [SerializeField] private int currentWaveIndex = 0; // ���� ���̺� ��ȣ

    private EnemyManager enemyManager; // �� ���� �� �����ϴ� �Ŵ���

    private UIManager uiManager;
    public static bool isFirstLoading = true;

    private void Awake()
    {
        // �̱��� �Ҵ�
        instance = this;

        // �÷��̾� ã�� �ʱ�ȭ
        player = FindObjectOfType<PlayerController>();
        player.Init(this);

        uiManager = FindObjectOfType<UIManager>();

        _playerResourceController = player.GetComponent<ResourceController>();
        _playerResourceController.RemoveHealthChangeEvent(uiManager.ChangePlayerHP);
        _playerResourceController.AddHealthChangeEvent(uiManager.ChangePlayerHP);

        // �� �Ŵ��� �ʱ�ȭ
        enemyManager = GetComponentInChildren<EnemyManager>();
        enemyManager.Init(this);
    }
    private void Start()
    {
        if (!isFirstLoading)
        {
            StartGame();
        }
        else
        {
            isFirstLoading = false;
        }
    }

    public void StartGame()
    {
        uiManager.SetPlayGame();
        StartNextWave(); // ù ���̺� ����
    }

    void StartNextWave()
    {
        currentWaveIndex += 1; // ���̺� �ε��� ����

        uiManager.ChangeWave(currentWaveIndex);
        // 5���̺긶�� ���̵� ���� (��: 1~4 �� ���� 1, 5~9 �� ���� 2 ...)
        enemyManager.StartWave(1 + currentWaveIndex / 5);
    }

    // ���̺� ���� �� ���� ���̺� ����
    public void EndOfWave()
    {
        StartNextWave();
    }

    // �÷��̾ �׾��� �� ���� ���� ó��
    public void GameOver()
    {
        enemyManager.StopWave(); // �� ���� ����
    }

    //// ���߿� �׽�Ʈ: Space Ű�� ���� ����
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        StartGame();
    //    }
    //}
}
