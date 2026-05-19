namespace GymDivision;

class CandidateData(Room roomWithCandidate, Member member, int valueGained)
{
    public Member Member { get; private set; } = member;
    public int ValueGained { get; private set; } = valueGained;
    public Room RoomWithCandidate { get; private set; } = roomWithCandidate;
}