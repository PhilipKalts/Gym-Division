namespace GymDivision.ScoreCalculators;

public static class PairCalculator
{
    const int scoreWhenPaired = 100;
    
    public static int GetScore(Member a, Member b, int weight)
    {
        if (weight == 0) return 0;
        bool isPair = a.Model.PairMember == b.Model.Name && b.Model.PairMember == a.Model.Name;
        return isPair ? scoreWhenPaired : 0;
    }
}