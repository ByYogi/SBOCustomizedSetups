using SAPbobsCOM;
using System.Collections.Generic;
using UDTnFGenerator.Model;
using static UDTnFGenerator.Helper.SAPData.CreateUDFUtilities;

namespace UDTnFGenerator
{
    internal static class GeneratUDF
    {
        /// <summary>
        /// Here create UDFs to SAP official table
        /// </summary>
        public static void GenToSysTable()
        {
            CreateUDFs("ORDR", new List<UDFInfo>
            {
                new UDFInfo("SI_Field1", "Field1 for official table", BoFieldTypes.db_Alpha, 50),
                new UDFInfo("SI_Field2", "Field2 for official table", BoFieldTypes.db_Float){FieldSubType = BoFldSubTypes.st_Price},
                new UDFInfo("SI_Field3", "Field3 for official table", BoFieldTypes.db_Date),
            });
        }
        /// <summary>
        /// Here you're UDFs for UDT 
        /// </summary>
        public static void GenToUDT()
        {
            CreateUDFs("@SI_TableName", new List<UDFInfo>
            {
                new UDFInfo("SI_Field1", "Field1 for UDT", BoFieldTypes.db_Alpha, 50)
            });
        }
    }
}
