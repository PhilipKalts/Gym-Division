namespace GymDivision.ScoreCalculators;

public static class AgeCalculator
{
    const int maxScore = 100;
    const int scoreLossPerAgeDifference = 5;
    
    public static int GetScore(Member a, Member b, int weight)
    {
        if (weight == 0) return 0;

        int ageDifference = Math.Abs(a.Model.Age - b.Model.Age);
        int multiplier = maxScore - scoreLossPerAgeDifference * ageDifference;
        
        return multiplier * weight;
    }
}