using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import.RunnableClasses
{
    /// <summary>
    /// class that deletes the entries of the shares in the database
    /// </summary>
    [RunnableClassAttribute(2, "Löscht die Einträge zu den Aktien aus der Datenbank")]
    public class ClearStocks : RunnableClassBase
    {
        // TODO : Erstelle eine SQL Connection über den Connectrion String vom Docker zur Datenbank und lösche die Einträge
        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}
