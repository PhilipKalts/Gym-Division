namespace GymDivision;

class SwapCandidate
{
    public Room RoomWithCandidate;
    public Member Candidate;
    public int ValueGained;

    public SwapCandidate(Room roomWithCandidate, Member candidate, int valueGained)
    {
        Candidate = candidate;
        ValueGained = valueGained;
        RoomWithCandidate = roomWithCandidate;
    }
}