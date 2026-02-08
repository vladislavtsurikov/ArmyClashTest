#if UNITY_EDITOR
using VladislavTsurikov.AutoDefines.Editor;

namespace VladislavTsurikov.MegaWorld.Editor
{
    public sealed class MegaWorldRendererStackRule : TypeDefineRule
    {
        protected override string GetDefineToApplySymbol() => "RENDERER_STACK";
        public override string GetTypeFullName() =>
            "VladislavTsurikov.RendererStack.Runtime.TerrainObjectRenderer.TerrainObjectRenderer";
    }
}
#endif
