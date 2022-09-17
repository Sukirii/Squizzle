[System.Serializable]
public class GameData
{
    public bool[] finished, collectable;

    public GameData(GameManager _gameManager)
    {
        finished = new bool[GameManager.levels.Length];
        collectable = new bool[finished.Length];

        for (int i = 0; i < finished.Length; i++)
        {
            finished[i] = GameManager.levels[i].finished;
            collectable[i] = GameManager.levels[i].collectable;
        }
    }
}