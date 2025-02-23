public static class TransientScoring
{
    public static int TimeAlive { get; private set; } = 0;
    public static int EnemyKillScore { get; private set; } = 0;
    public static int UpgradesCollected { get; private set; } = 0;
    public static int BonusScore { get; private set; } = 0; // if there is any

    public static int TotalScore => TimeAlive * TimeAliveFactor + EnemyKillScore * EnemyKillScoreFactor + UpgradesCollected * UpgradeScoreFactor + BonusScore * BonusScoreFactor;
    public const int TimeAliveFactor = 1;
    public const int EnemyKillScoreFactor = 1;
    public const int UpgradeScoreFactor = 1000;
    public const int BonusScoreFactor = 1;
    public static void SetTimeAlive(int time) => TimeAlive = time;
    public static void AddEnemyKillScore(int score) => EnemyKillScore += score;
    public static void AddUpgradesCollected(int score) => UpgradesCollected += score;
    public static void AddBonusScore(int score) => BonusScore += score;

    public static bool HasScoreToSave = true;

    public async static System.Threading.Tasks.Task Save()
    {
        if (!HasScoreToSave) return;
        await LeaderboardManager.SetCurrentPlayerScore(TotalScore);
        HasScoreToSave = false;
    }

    public static void Restart()
    {
        TimeAlive = 0;
        EnemyKillScore = 0;
        UpgradesCollected = 0;
        BonusScore = 0;
        HasScoreToSave = true;
    }
}