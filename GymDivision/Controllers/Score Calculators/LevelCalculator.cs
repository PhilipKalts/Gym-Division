namespace GymDivision.ScoreCalculators;

public static class LevelCalculator
{
    const int maxScore = 100;
    const int scoreLossPerPoint = 20;
    
    public static int GetLevelScore(Member a, Member b, int weight)
    {
        if (weight == 0) return 0;
            
        int levelDifference = Math.Abs(a.Model.Level - b.Model.Level);
        int multiplier = maxScore - scoreLossPerPoint * levelDifference;
        
        return multiplier * weight;
    }
}