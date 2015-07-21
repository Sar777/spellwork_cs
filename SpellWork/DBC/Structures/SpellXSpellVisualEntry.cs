using DBFilesClient.NET;

namespace SpellWork.DBC.Structures
{
    public sealed class SpellXSpellVisualEntry
    {
        public uint ID;
        public uint SpellID;
        public uint DifficultyID;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 2)]
        public uint[] SpellVisual;
        public float Unk620;
        public uint PlayerConditionID;
        public uint Flags;
    }
}
