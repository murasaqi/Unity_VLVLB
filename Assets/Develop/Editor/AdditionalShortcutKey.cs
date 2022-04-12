using UnityEditor;

public static class AdditionalShortcutKey {
    [MenuItem("Edit/Redo %#Z")]
    static void Redo() {
        Undo.PerformRedo ();
    }
}