namespace GymDivision.Controllers;

public struct MemberDistributor(int totalMembers, RoomSeparationData[] roomSeparationDatas)
{
    readonly int totalMembers = totalMembers;
    RoomSeparationData[] roomSeparationDatas = roomSeparationDatas;
    
    /// <summary>
    /// Distribute the Members to the rooms
    /// Get the results from the array which returns
    /// </summary>
    /// <returns></returns>
    public void SetDistribution()
    {
        int totalIdeal = roomSeparationDatas.Sum(x => x.RoomSizeIdeal);
        int extras = totalMembers - totalIdeal;
        if (extras > 0)
        {
            ExtrasArePositive(extras);
            return;
        }
        
        int totalMin = roomSeparationDatas.Sum(x => x.RoomSizeMin);
        if (totalMin <= totalMembers) ExtrasAreMoreThanCombinedMin(totalMin);
        else ThereAreNotEnoughMinForAllRooms();
    }
    
    
    /// <summary>
    /// When we have more Members than ideal, we distribute the extras to the rooms
    /// </summary>
    /// <param name="extras"></param>
    void ExtrasArePositive(int extras)
    {
        for (int i = 0; i < roomSeparationDatas.Length; i++)
            roomSeparationDatas[i].MembersToAdd = roomSeparationDatas[i].RoomSizeIdeal;

        DistributeRemaining(extras, room => room.MembersToAdd < room.RoomSizeMax);
    }
    
    
    /// <summary>
    /// When we have more Members than combined min, we set all the rooms to their minimum
    /// The extras are distributed to all of the rooms
    /// </summary>
    /// <param name="totalMin"></param>
    void ExtrasAreMoreThanCombinedMin(int totalMin)
    {
        for (int i = 0; i < roomSeparationDatas.Length; i++)
            roomSeparationDatas[i].MembersToAdd = roomSeparationDatas[i].RoomSizeMin;
        if (totalMin == totalMembers) return;
            
        int remainingMembers = totalMembers - totalMin;
        DistributeRemaining(remainingMembers, room => room.MembersToAdd < room.RoomSizeIdeal);
    }
    
    
    /// <summary>
    /// When we have not enough min for all rooms, we first set which rooms will have the minimum
    /// Then we distribute the remaining Members to those rooms
    /// </summary>
    void ThereAreNotEnoughMinForAllRooms()
    {
        int remainingMembers = totalMembers;
        for (int i = 0; i < roomSeparationDatas.Length; i++)
        {
            RoomSeparationData room = roomSeparationDatas[i];
            if (remainingMembers < room.RoomSizeMin) continue;
            remainingMembers -= room.RoomSizeMin;
            room.MembersToAdd = room.RoomSizeMin;
            roomSeparationDatas[i] = room;
        }

        DistributeRemaining(remainingMembers, 
            room => room.MembersToAdd > 0 && room.MembersToAdd < room.RoomSizeMax);
    }

    
    /// <summary>
    /// Distribute the remaining Members to the rooms that meet the condition
    /// </summary>
    /// <param name="remainingMembers"></param>
    /// <param name="condition"></param>
    void DistributeRemaining(int remainingMembers, Func<RoomSeparationData, bool> condition)
    {
        int roomIndex = 0;
        bool hasMadeProgress = false;
        
        while (remainingMembers > 0)
        {
            RoomSeparationData room = roomSeparationDatas[roomIndex];
            if (condition(room))
            {
                remainingMembers--;
                room.MembersToAdd++;
                roomSeparationDatas[roomIndex] = room;
                hasMadeProgress = true;
            }
            roomIndex++;

            if (roomIndex < roomSeparationDatas.Length) continue;
            if (!hasMadeProgress) break;
            roomIndex = 0;
            hasMadeProgress = false;
        }
    }
}