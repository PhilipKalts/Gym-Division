using GymDivision.Controllers;
using GymDivision.Models;

namespace GymDivision.Domain;

public class RoomPopulator(List<MemberModel> allMemberModels, RoomSeparationData[] roomSeparationDatas)
{
    #region Fields

    Room[] allRooms;
    List<MemberModel> allMemberModels = allMemberModels;
    RoomSeparationData[] roomSeparationDatas = roomSeparationDatas;
    DistributionWeights distributionWeights;
    
    #endregion
    
    
    /// <summary>
    /// The starting point for settings the members inside their appropriate rooms.
    /// </summary>
    /// <param name="types"></param>
    /// <exception cref="Exception"></exception>
    public void SetRoomSeparations(List<RequirementType> types)
    {
        Console.WriteLine("______________________________");
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        int totalMembersInRoomSeparationDatas = roomSeparationDatas.Sum(x => x.MembersToAdd);
        if (allMemberModels.Count != totalMembersInRoomSeparationDatas)
            throw new Exception("Number of members in RoomSeparationData and allMembers are not equal");

        distributionWeights = new DistributionWeights(types);
        
        CheckToSortMembers(types);
        SetMembersToRooms();
        ImproveRooms();
        SetRoomsToSeparationDatas();
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
    }
    
    
    /// <summary>
    /// If there is only 1 Requirement type, we can sort the members based on that type.
    /// That may make the swapping later faster since the members will already be in good rooms  
    /// </summary>
    void CheckToSortMembers(List<RequirementType> types)
    {
        if (types.Count != 1) return;

        switch (types[0])
        {
            case RequirementType.Age:
                allMemberModels = allMemberModels.OrderBy(x => x.Age).ToList();
                break;
            case RequirementType.Level:
                allMemberModels = allMemberModels.OrderBy(x => x.Level).ToList();
                break;
            default: 
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
        allRooms = new Room[roomSeparationDatas.Length];

        for (int i = 0; i < allRooms.Length; i++)
        {
            allRooms[i] = new Room(distributionWeights);
            for (int j = 0; j < roomSeparationDatas[i].MembersToAdd; j++)
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
        SwapCandidate? bestCandidate = null;
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
                    
                    List<Member> candidates = allRooms[j].AllMembers.
                        OrderBy(x => x.ScoreInRoom).
                        Take(3).
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
                    bestCandidate.Candidate);
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
            roomSeparationDatas[i].Members = memberModels;
        }
    }
}