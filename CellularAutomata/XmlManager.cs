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

        public Fhp3Collision[] GetFhp3Collisions()
        {
            Assembly assembly = typeof(Fhp3).GetTypeInfo().Assembly;
            Fhp3Collision[] fhp3Collisions;
            using (Stream fhp3CollisionsStream = assembly.GetManifestResourceStream("CellularAutomata.Fhp3Collisions.xml"))
            {
                XElement fhp3CollisionsXElement = XElement.Load(fhp3CollisionsStream);
                fhp3Collisions =
                    fhp3CollisionsXElement.Elements("Fhp3Collision")
                    .Select(fhp3CollEl =>
                        new Fhp3Collision
                        (
                            Convert.ToByte(fhp3CollEl.Attribute("inState").Value),
                            Convert.ToByte(fhp3CollEl.Attribute("outState1").Value),
                            Convert.ToByte(fhp3CollEl.Attribute("outState2").Value)
                        )
                    ).ToArray();
            }
            return fhp3Collisions;
        }
    }
}
