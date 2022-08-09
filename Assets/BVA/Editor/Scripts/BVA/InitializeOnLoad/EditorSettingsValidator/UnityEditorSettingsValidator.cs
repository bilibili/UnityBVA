using System.Collections.Generic;
using System.Linq;

namespace BVA.EditorSettingsValidator
{
    public sealed class UnityEditorSettingsValidator
    {
        public IEnumerable<IUnitySettingsValidator> Validators { get; } = new List<IUnitySettingsValidator>
        {
            new UnityColorSpaceSettingsValidator(),
            new UnityURPSettingsValidator(),
        };
        
        public bool IsValid()
        {
            return Validators.All(validator => validator.IsValid);
        }
    }
}