using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace CellularAutomata
{
    class FHPColissionStatePair
    {
        [XmlAttribute("inState")]
        public FHPParticleState InState { get; set; }
        [XmlAttribute("outState1")]
        public FHPParticleState OutState1 { get; set; }
        [XmlAttribute("outState2")]
        public FHPParticleState OutState2 { get; set; }

        public FHPColissionStatePair() { }
        public FHPColissionStatePair(FHPParticleState inState, FHPParticleState outState1, FHPParticleState outState2)
        {
            InState = inState;
            OutState1 = outState1;
            OutState2 = outState2;
        }
    }
}
