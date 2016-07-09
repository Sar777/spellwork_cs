using DBFilesClient.NET;

namespace SpellWork.DBC.Structures
{
    public sealed class SpellInterruptsEntry
    {
        public uint Id;
        public uint SpellId;                                      // 1  - Pandaria
        public uint Difficulty;                                   // 2  - Panadraia
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 2)]
        public uint[] AuraInterruptFlags;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 2)]
        public uint[] ChannelInterruptFlags;
        public uint InterruptFlags;
    }
}
