using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public class FHP
    {
        private FHPParticleState[][] outState = null;

        public FHP()
        {
            if (outState == null)
                InitializeOutStates();
        }

        private void InitializeOutStates()
        {
            outState = new FHPParticleState[256][];
            for (int i = 0; i < 256; i++)
            {
                FHPParticleState iState = (FHPParticleState)i;
                outState[i] = new[] { iState, iState };
            }

            FHPCollision[] fhpCollisions = XMLManager.Instance.GetFHPCollisions();

            foreach (FHPCollision fhpCollision in fhpCollisions)
            {
                FHPParticleState[] fhpParticleStatesPair = outState[fhpCollision.InState];
                fhpParticleStatesPair[0] = (FHPParticleState)fhpCollision.OutState1;
                fhpParticleStatesPair[1] = (FHPParticleState)fhpCollision.OutState2;
            }
        }
    }
}
