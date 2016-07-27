using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Reflection;

namespace CellularAutomata
{
    class XMLManager
    {
        public static XMLManager Instance { get; } = new XMLManager();

        private XMLManager() { }

        public FHPCollision[] GetFHPCollisions()
        {
            Assembly assembly = typeof(FHP).GetTypeInfo().Assembly;
            FHPCollision[] fhpCollisions;
            using (Stream fhpCollisionsStream = assembly.GetManifestResourceStream("CellularAutomata.FHPCollisions.xml"))
            {
                XElement fhpCollisionsXElement = XElement.Load(fhpCollisionsStream);
                fhpCollisions =
                    fhpCollisionsXElement.Elements("FHPCollision")
                    .Select(fhpCollEl =>
                        new FHPCollision
                        (
                            Convert.ToByte(fhpCollEl.Attribute("inState").Value),
                            Convert.ToByte(fhpCollEl.Attribute("outState1").Value),
                            Convert.ToByte(fhpCollEl.Attribute("outState2").Value)
                        )
                    ).ToArray();
            }
            return fhpCollisions;
        }
    }
}
