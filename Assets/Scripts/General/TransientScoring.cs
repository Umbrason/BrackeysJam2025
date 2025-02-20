public static class TransientScoring
{
    public static int TimeAlive { get; private set; } = 0;
    public static int EnemyKillScore { get; private set; } = 0;
    public static int UpgradeScore { get; private set; } = 0;
    public static int BonusScore { get; private set; } = 0; // if there is any
    
    public static int TotalScore => TimeAlive + EnemyKillScore + UpgradeScore + BonusScore;
    
    public static void SetTimeAlive(int time) => TimeAlive = time;
    public static void AddEnemyKillScore(int score) => EnemyKillScore += score;
    public static void AddUpgradesCollected(int score) => UpgradeScore += score;
    public static void AddBonusScore(int score) => BonusScore += score;

    public static void Reset()
    {
        TimeAlive = 0;
        EnemyKillScore = 0;
        UpgradeScore = 0;
        BonusScore = 0;
    }
}