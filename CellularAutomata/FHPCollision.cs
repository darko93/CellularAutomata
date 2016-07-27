namespace CellularAutomata
{
    class FHPCollision
    {
        public byte InState { get; set; }
        public byte OutState1 { get; set; }
        public byte OutState2 { get; set; }

        public FHPCollision(byte inState, byte outState1, byte outState2)
        {
            InState = inState;
            OutState1 = outState1;
            OutState2 = outState2;
        }
    }
}
