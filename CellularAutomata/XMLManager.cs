using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Reflection;

namespace CellularAutomata
{
    class XmlManager
    {
        public static XmlManager Instance { get; } = new XmlManager();

        private XmlManager() { }

        public FhpCollision[] GetFhpCollisions()
        {
            Assembly assembly = typeof(Fhp).GetTypeInfo().Assembly;
            FhpCollision[] fhpCollisions;
            using (Stream fhpCollisionsStream = assembly.GetManifestResourceStream("CellularAutomata.FhpCollisions.xml"))
            {
                XElement fhpCollisionsXElement = XElement.Load(fhpCollisionsStream);
                fhpCollisions =
                    fhpCollisionsXElement.Elements("FhpCollision")
                    .Select(fhpCollEl =>
                        new FhpCollision
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
