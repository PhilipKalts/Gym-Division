namespace GymDivision.ScoreCalculators;

public enum WeightType { Level, Age, Injury, Pair }

public struct WeightData(WeightType type, int value)
{
    public WeightType Type { get; private set; } = type;
    public int Value { get; private set; } = value;
}