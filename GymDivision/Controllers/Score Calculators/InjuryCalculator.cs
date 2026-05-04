namespace GymDivision.ScoreCalculators;

public static class InjuryCalculator
{
    const int maxScore = 100;
    
    public static int GetScore(Member a, Member b, int weight)
    {
        if (weight == 0) return 0;

        return a.Model.Injury == b.Model.Injury ? maxScore : 0;
    }
}