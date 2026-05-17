using GymDivision.ScoreCalculators;

namespace GymDivision;

public class Room
{
    #region Fields

    public List<Member> AllMembers { get; set; }
    public int Score { get; private set; }
    
    DistributionWeights distributionWeights;

    #endregion
        
    
    public Room(DistributionWeights distributionWeights)
    {
        this.distributionWeights = distributionWeights;
        AllMembers = new List<Member>();
    }
        
    
    /// <summary>
    /// Calculate the score of the room and each member inside it
    /// </summary>
    public void CalculateScores()
    {
        if (AllMembers.Count == 0) return;

        Score = 0;
        
        foreach (Member member in AllMembers)
        {
            member.ScoreInRoom = 0;
            for (int i = 0; i < AllMembers.Count; i++)
            {
                if (member == AllMembers[i]) continue;
                member.ScoreInRoom = GetScoreFromCalculations(member, AllMembers[i]);
            }
        }

        Score = AllMembers.Sum(member => member.ScoreInRoom);
    }
    

    /// <summary>
    /// Calculate the potential score change in the room from a swap
    /// </summary>
    /// <param name="memberToSwap"></param>
    /// <param name="memberToSwapWith"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int GetPossibleScoreForSwap(Member memberToSwap, Member memberToSwapWith)
    {
        if (!AllMembers.Contains(memberToSwap)) 
            throw new Exception($"You are trying to swap a member that is not in the room");
        if (AllMembers.Contains(memberToSwapWith))
            throw new Exception($"You are trying to put a member that is already in the room");
        
        int potentialScore = Score;
        
        foreach (var member in AllMembers)
        {
            if (member == memberToSwap) continue;

            // The interaction between members goes both ways in the total Score
            // A -> B & B -> A
            // That is why we multiply by 2.
            potentialScore -= GetScoreFromCalculations(memberToSwap, member) * 2;
            potentialScore += GetScoreFromCalculations(memberToSwapWith, member) * 2;
        }
        
        return potentialScore;
    }

    
    /// <summary>
    /// Get all the scores between 2 members from the respective calculators
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="shouldScoreDouble"></param>
    /// <returns></returns>
    int GetScoreFromCalculations(Member a, Member b)
    {
        int score = 0;
        
        score += AgeCalculator.GetScore(a, b, distributionWeights.Age);
        score += PairCalculator.GetScore(a, b, distributionWeights.Pair);
        score += LevelCalculator.GetScore(a, b, distributionWeights.Level);
        score += InjuryCalculator.GetScore(a, b, distributionWeights.Injury);
        
        return score;
    }


    /// <summary>
    /// Remove a member from the Room and add someone else
    /// </summary>
    /// <param name="memberToSwap"></param>
    /// <param name="memberToSwapWith"></param>
    public void SwapMember(Member memberToSwap, Member memberToSwapWith)
    {
        if (!AllMembers.Contains(memberToSwap))
            throw new Exception("You are trying to remove a member who is not in the room");
        if (AllMembers.Contains(memberToSwapWith))
            throw new Exception("You are trying to add a member who is already in the room");
        
        AllMembers.Remove(memberToSwap);
        AllMembers.Add(memberToSwapWith);
        CalculateScores();
    }
}