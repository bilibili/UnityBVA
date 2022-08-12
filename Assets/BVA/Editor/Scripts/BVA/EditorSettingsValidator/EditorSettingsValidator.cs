using System.Collections.Generic;
using System.Linq;


namespace BVA
{
    public sealed class EditorSettingsValidator
    {
        public IEnumerable<ISettingsValidator> Validators { get; } = new List<ISettingsValidator>
        {
                        new URPSettingsValidator(),
            new ColorSpaceSettingsValidator(),
            new ReflectionProbeSettingsValidator(),
            new LightmapSettingsValidator(),
            new PlayerSettingsValidator(),
            new NormalMapsValidator(),
        };
        
        public bool IsValid()
        {
            return Validators.All(validator => validator.IsValid);
        }
    }
}