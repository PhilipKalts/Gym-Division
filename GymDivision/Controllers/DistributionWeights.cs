using GymDivision.Controllers;

namespace GymDivision;

public struct DistributionWeights
{
    static readonly int[] scoresPerIndex =
    [
        1000, 500, 250, 100
    ];
    
    public int Age { get; private set; }
    public int Pair { get; private set; }
    public int Level { get; private set; }
    public int Injury { get; private set; }

    public DistributionWeights(List<RequirementType> requirements)
    {
        for (int i = 0; i < requirements.Count; i++) SetScore(requirements[i], i);
    }

    void SetScore(RequirementType type, int index)
    {
        switch (type)
        {
            case RequirementType.Age:
                Age = scoresPerIndex[index];
                break;
            case RequirementType.Level:
                Level = scoresPerIndex[index];
                break;
            case RequirementType.Pair:
                Pair = scoresPerIndex[index];
                break;
            case RequirementType.Injury:
                Injury = scoresPerIndex[index];
                break;
            default:
                break;
        }
    }
}