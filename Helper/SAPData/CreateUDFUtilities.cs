using System.Collections.Generic;
using UDTnFGenerator.Model;

namespace UDTnFGenerator.Helper.SAPData
{
    internal static class CreateUDFUtilities
    {
        public static void CreateUDFs(string tableName, List<UDFInfo> udfs)
        {
            RecordHtml.InputGroupStart($"[{tableName}]");
            foreach (UDFInfo udfInfo in udfs)
            {
                udfInfo.TableName = tableName;
                SAPObjectCreator.CreateUDF(udfInfo);
            }
            RecordHtml.InputGroupEnd();
        }
    }
}
