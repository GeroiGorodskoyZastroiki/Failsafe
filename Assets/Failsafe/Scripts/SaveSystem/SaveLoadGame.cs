using UnityEngine;

public class SaveLoadGame : MonoBehaviour
{
    private SaveLoadManager saveLoadManager;
    private GameData currentGameData;

    [SerializeField] private Player player;//�� ����� ���� ����, ��� ��������

    private void Awake()
    {
        saveLoadManager = GetComponent<SaveLoadManager>();
        currentGameData = new GameData(); // ������������� ����� ������
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            SavePlayerState();

        if (Input.GetKeyDown(KeyCode.F9))
            LoadGame();
    }
    private void SaveGame()
    {
        //��������� ������� ��������� ����

        saveLoadManager.SaveGame(currentGameData);
    }

    public void SavePlayerState()
    {
        // ���������� ��������� ������
        currentGameData.runData.playerState.health = player.health;
        currentGameData.runData.playerState.position = player.gameObject.transform.position;
        currentGameData.runData.playerState.rotation = player.gameObject.transform.rotation;

        Debug.Log($"Saving Health: {currentGameData.runData.playerState.health}");
        Debug.Log($"Saving Position: {player.gameObject.transform.position}, {player.gameObject.transform.rotation}");

        SaveGame();
    }
    public void LoadGame()
    {
        currentGameData = saveLoadManager.LoadGame();
        if (currentGameData != null)
        {
            UpdateGameState(currentGameData);
        }
    }
    private void UpdateGameState(GameData gameData)
    {
        // ���������� ������ � ������� ������

        // ���������� ��������� ������
        player.health = gameData.runData.playerState.health;
        player.gameObject.transform.position = gameData.runData.playerState.position;
        player.gameObject.transform.rotation = gameData.runData.playerState.rotation;

        Debug.Log($"Loaded Health: {gameData.runData.playerState.health}");
        Debug.Log($"Loaded Position: {gameData.runData.playerState.position}");
        Debug.Log($"Current Position: {player.gameObject.transform.position}");




        // ���������� �������� �����

        // ���������� ���������� � �����������

        // ���������� �������

        // �������������� ����������, ���� ����������
        // ��������, ���������� UI, ��������� ���� � �.�.
    }
}
