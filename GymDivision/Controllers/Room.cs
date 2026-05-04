using GymDivision.Models;
using GymDivision.ScoreCalculators;

namespace GymDivision;

public class Room
{
    public List<Member> AllMembers { get; set; }
    public HashSet<Injury> Injuries { get; set; }
    public int Score { get; private set; }
    public int AverageLevel { get; private set; }

    DistributionWeights distributionWeights;
        
    public Room(DistributionWeights distributionWeights)
    {
        this.distributionWeights = distributionWeights;
        AllMembers = new List<Member>();
        Injuries = new HashSet<Injury>();
    }
        
        
    public void CalculateScores()
    {
        if (AllMembers.Count == 0) return;

        Score = 0;
        int sumLevels = 0;
        
        foreach (Member member in AllMembers)
        {
            member.ScoreInRoom = 0;
            sumLevels += member.Model.Level;
            for (int i = 0; i < AllMembers.Count; i++)
            {
                if (member == AllMembers[i]) continue;
                member.ScoreInRoom += AgeCalculator.GetScore(member, AllMembers[i], distributionWeights.Age);
                member.ScoreInRoom += PairCalculator.GetScore(member, AllMembers[i], distributionWeights.Pair);
                member.ScoreInRoom += LevelCalculator.GetScore(member, AllMembers[i], distributionWeights.Level);
                member.ScoreInRoom += InjuryCalculator.GetScore(member, AllMembers[i], distributionWeights.Injury);
            }
        }

        AverageLevel = sumLevels / AllMembers.Count;
        Score = AllMembers.Sum(member => member.ScoreInRoom);
    }
        
    
    public void SetInjuries()
    {
        Injuries.Clear();
        for (int i = 0; i < AllMembers.Count; i++)
            Injuries.Add(AllMembers[i].Model.Injury);
    }


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
            potentialScore -= AgeCalculator.GetScore(memberToSwap, member, distributionWeights.Age) * 2;
            potentialScore -= PairCalculator.GetScore(memberToSwap, member, distributionWeights.Pair) * 2;
            potentialScore -= LevelCalculator.GetScore(memberToSwap, member, distributionWeights.Level) * 2;
            potentialScore -= InjuryCalculator.GetScore(memberToSwap, member, distributionWeights.Injury) * 2;
            
            potentialScore += AgeCalculator.GetScore(memberToSwapWith, member, distributionWeights.Age) * 2;
            potentialScore += PairCalculator.GetScore(memberToSwapWith, member, distributionWeights.Pair) * 2;
            potentialScore += LevelCalculator.GetScore(memberToSwapWith, member, distributionWeights.Level) * 2;
            potentialScore += InjuryCalculator.GetScore(memberToSwapWith, member, distributionWeights.Injury) * 2;
        }
        
        return potentialScore;
    }

    // int GetScoreFromCalculations()
    // {
    //     
    // }
}