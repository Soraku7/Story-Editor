namespace Editor.Story
{
    public enum NodeType
    {
        Base = 0,
        ZeroInZeroOut = 1,
        ZeroInSingleOut = 2,
        ZeroInMultiOut = 3,
        SingleInZeroOut = 4,
        SingleInSingleOut = 5,
        SingleInMultiOut = 6,
        MultiInZeroOut = 7,
        MultiInSingleOut = 8,
        MultiInMultiOut = 9,
        
        Start = 21,
        End = 31,
        Dialogue = 41,
        Branch = 51,
    }
}