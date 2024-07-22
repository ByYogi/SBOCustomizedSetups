using SAPbobsCOM;
using static UDTnFGenerator.Helper.SAPData.SAPObjectCreator;

namespace UDTnFGenerator
{
    internal static class GeneratUDT
    {
        public static void Generate()
        {
            CreateUDT("SI_TableName", "This is your UDT shwoing in sap name", BoUTBTableType.bott_NoObject);
        }
    }
}
