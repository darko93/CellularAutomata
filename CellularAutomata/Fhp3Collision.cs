namespace CellularAutomata
{
    class Fhp3Collision
    {
        public byte InState { get; set; }
        public byte OutState1 { get; set; }
        public byte OutState2 { get; set; }

        public Fhp3Collision(byte inState, byte outState1, byte outState2)
        {
            InState = inState;
            OutState1 = outState1;
            OutState2 = outState2;
        }
    }
}
