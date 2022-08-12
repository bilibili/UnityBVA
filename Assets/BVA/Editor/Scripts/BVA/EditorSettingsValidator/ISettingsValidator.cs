namespace BVA
{

    public interface ISettingsValidator
    {
        bool IsValid { get; }
        bool CanFix { get; }
        string HeaderDescription { get; }
        string CurrentValueDescription { get; }
        string RecommendedValueDescription { get; }
        void Validate();
    }
}