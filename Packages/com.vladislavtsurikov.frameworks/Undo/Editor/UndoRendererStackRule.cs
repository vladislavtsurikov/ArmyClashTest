#if UNITY_EDITOR
using VladislavTsurikov.AutoDefines.Editor;

namespace VladislavTsurikov.Undo.Editor
{
    public sealed class UndoRendererStackRule : TypeDefineRule
    {
        protected override string GetDefineToApplySymbol() => "UNDO_RENDERER_STACK";
        public override string GetTypeFullName() => "VladislavTsurikov.RendererStack.Runtime.TerrainObjectRenderer.TerrainObjectRenderer";
    }
}
#endif
