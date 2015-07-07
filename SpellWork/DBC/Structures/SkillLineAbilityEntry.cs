using DBFilesClient.NET;

namespace SpellWork.DBC.Structures
{
    public sealed class SkillLineAbilityEntry
    {
        public uint Id;                                             // 0
        public uint SkillLine;                                      // 1
        public uint SpellID;                                        // 2
        public uint RaceMask;                                       // 3
        public uint ClassMask;                                      // 4
        public uint MinSkillLineRank;                               // 5
        public uint SupercedesSpell;                                // 6
        public uint AquireMethod;                                   // 7
        public uint TrivialSkillLineRankHigh;                       // 8
        public uint TrivialSkillLineRankLow;                        // 9
        public uint NumSkillUps;                                    // 10
        public uint UniqueBit;                                      // 11
        public uint TradeSkillCategoryID;                           // 12
    }
}
