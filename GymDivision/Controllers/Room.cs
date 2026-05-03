using GymDivision.Models;
using GymDivision.ScoreCalculators;

namespace GymDivision;

public class Room
{
    public List<Member> AllMembers { get; set; }
    public HashSet<Injury> Injuries { get; set; }
    public int Score { get; private set; }
    public int AverageLevel { get; private set; }

    int levelWeight;
        
    public Room(int levelWeight)
    {
        this.levelWeight = levelWeight;
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
                member.ScoreInRoom += LevelCalculator.GetLevelScore(member, AllMembers[i], levelWeight);
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
            potentialScore -= LevelCalculator.GetLevelScore(memberToSwap, member, levelWeight) * 2;
            potentialScore += LevelCalculator.GetLevelScore(memberToSwapWith, member, levelWeight) * 2;
        }
        
        return potentialScore;
    }
}