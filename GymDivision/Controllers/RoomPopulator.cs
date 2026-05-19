using GymDivision.Controllers;
using GymDivision.Models;
using GymDivision.ScoreCalculators;

namespace GymDivision.Domain;

public class RoomPopulator(
    List<MemberModel> allMemberModels, 
    RoomSeparationData[] allRoomSeparationData, 
    WeightData[] allWeightData)
{
    Room[] allRooms;
    
    
    /// <summary>
    /// The starting point for settings the members inside their appropriate rooms.
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void SetRoomSeparations()
    {
        Console.WriteLine("______________________________");
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        int totalMembersInRoomSeparationDatas = allRoomSeparationData.Sum(x => x.MembersToAdd);
        if (allMemberModels.Count != totalMembersInRoomSeparationDatas)
            throw new Exception("Number of members in RoomSeparationData and allMembers are not equal");

        bool isOnlyOneWeightActive = IsOnlyOneWeightActive(out WeightType type);
        if (isOnlyOneWeightActive) SortMembersWithOneWeight(type);
        SetMembersToRooms();
        if (!isOnlyOneWeightActive) ImproveRooms();
        
        SetRoomsToSeparationDatas();
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
    }
    
    
    /// <summary>
    /// If we have only 1 active weight,
    /// the sorting can be done much faster since we do not have to calculate scores.
    /// </summary>
    bool IsOnlyOneWeightActive(out WeightType type)
    {
        type = WeightType.Age;
        bool hasFoundActive = false;
        for (int i = 0; i < allWeightData.Length; i++)
        {
            if (allWeightData[i].Value == 0) continue;
            // If there is more than 1 active weight return
            if (hasFoundActive) return false;
            
            hasFoundActive = true;
            type = allWeightData[i].Type;
        }

        return true;
    }

    
    /// <summary>
    /// If there is only 1 Weight type, we can sort the members based on that type.
    /// That may make the swapping later faster since the members will already be in good rooms 
    /// </summary>
    /// <param name="type"></param>
    void SortMembersWithOneWeight(WeightType type)
    {
        switch (type)
        {
            case WeightType.Age:
                allMemberModels = allMemberModels.OrderBy(x => x.Age).ToList();
                break;
            case WeightType.Level:
                allMemberModels = allMemberModels.OrderBy(x => x.Level).ToList();
                break;
        }
    }
    
    
    /// <summary>
    /// First, we add all the members inside the rooms
    /// to have a starting point when we need to calculate scores and swap
    /// </summary>
    void SetMembersToRooms()
    {
        int memberIndex = 0;
        allMemberModels = allMemberModels.Shuffle().ToList();
        allRooms = new Room[allRoomSeparationData.Length];

        for (int i = 0; i < allRooms.Length; i++)
        {
            allRooms[i] = new Room(allWeightData);
            for (int j = 0; j < allRoomSeparationData[i].MembersToAdd; j++)
            {
                Member member = new(allMemberModels[memberIndex]);
                allRooms[i].AllMembers.Add(member);
                memberIndex++;
            }
            allRooms[i].CalculateScores();
        }
    }


    /// <summary>
    /// After having a starting point, now we check the rooms to find any swaps between members
    /// </summary>
    void ImproveRooms()
    {
        const int maxAttempts = 100;
        for (int i = 0; i < maxAttempts; i++)
        {
            CheckForSwaps(out bool hasFoundImprovement);
            if (!hasFoundImprovement)
            {
                Console.WriteLine($"It took {i} attempts");
                // This last check is valuable for testing,
                // if you want to see what is going on when the system has declared there are no more improvements
                // CheckForSwaps(out hasFoundImprovement);
                break;
            }
        }
    }
    
    
    /// <summary>
    /// Check the rooms for swaps that improve the rooms' scores
    /// </summary>
    /// <param name="hasFoundImprovement"></param>
    void CheckForSwaps(out bool hasFoundImprovement)
    {
        CandidateData? bestCandidate = null;
        hasFoundImprovement = false;
        OrderRoomsByScores();
        
        for (int i = 0; i < allRooms.Length; i++)
        {
            if (allRooms[i].AllMembers.Count == 0) continue;
            List<Member> worstMembers = GetWorstMembers(i);
            foreach (var worstMember in worstMembers)
            {
                for (int j = 0; j < allRooms.Length; j++)
                {
                    if (i == j) continue;

                    int numOfCandidates = (int)(allRooms[j].AllMembers.Count * 0.3f);
                    
                    List<Member> candidates = allRooms[j].AllMembers.
                        OrderBy(x => x.ScoreInRoom).
                        Take(numOfCandidates).
                        ToList();
                    
                    foreach (var candidate in candidates)
                    {
                        int value = GetValueFromSwapping(allRooms[i], worstMember, allRooms[j], candidate);
                        if (value <= 0) continue;
                        if (bestCandidate != null && bestCandidate.ValueGained > value) continue;

                        bestCandidate = new(allRooms[j], candidate, value);
                    }
                }
                
                if (bestCandidate == null) continue;
                SwapRooms(allRooms[i], 
                    worstMember, 
                    bestCandidate.RoomWithCandidate, 
                    bestCandidate.Member);
                hasFoundImprovement = true;
                return;
            }
        }

        
        void OrderRoomsByScores()
        {
            for (int i = 0; i < allRooms.Length; i++)
            {
                allRooms[i].AllMembers = allRooms[i].AllMembers
                    .OrderBy(m => m.ScoreInRoom)
                    .ToList();
            }
        }
        
        
        List<Member> GetWorstMembers(int index)
        {
            List<Member> worstMembers = new();
            int numberOfWorstMembers = Math.Max(1, (int)(allRooms[index].AllMembers.Count * 0.2));
            for (int i = 0; i < numberOfWorstMembers; i++)
                worstMembers.Add(allRooms[index].AllMembers[i]);
            return worstMembers;
        }


        int GetValueFromSwapping(Room roomA, Member memberA, Room roomB, Member memberB)
        {
            int previousRoomScoreA = roomA.Score;
            int previousRoomScoreB = roomB.Score;
            
            int potentialScoreA = roomA.GetPossibleScoreForSwap(memberA, memberB);
            int potentialScoreB = roomB.GetPossibleScoreForSwap(memberB, memberA);
            return (potentialScoreA + potentialScoreB) - (previousRoomScoreA + previousRoomScoreB);
        }

        
        void SwapRooms(Room roomA, Member memberA, Room roomB, Member memberB)
        {
            roomA.SwapMember(memberA, memberB);
            roomB.SwapMember(memberB, memberA);
        }
    }
    
    
    /// <summary>
    /// Last step, since all members are in their rooms,
    /// it's time to insert the data to the array 
    /// </summary>
    void SetRoomsToSeparationDatas()
    {
        for (int i = 0; i < allRooms.Length; i++)
        {
            List<MemberModel> memberModels = allRooms[i].AllMembers.Select(x => x.Model).ToList();
            allRoomSeparationData[i].Members = memberModels;
        }
    }
}