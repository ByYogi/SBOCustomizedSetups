using System;

namespace UDTnFGenerator.Helper.SAPData
{

    ///BoFieldTypes.db_Alpha: 50/100;  //Max: 254; 100 characters = 50 Unicodes
    ///BoFieldTypes.db_Memo: 1000;
    ///BoFieldTypes.db_Numeric: 2/11;
    ///BoFieldTypes.db_Date: 0;

    /// SI: SabreInfo
    /// 7: ?? No.
    public class TablePost
    {
        /// <summary>
        /// Create UDTs for SAP
        /// </summary>
        /// <returns></returns>
        private bool CreateUDTInSAP()
        {
            try
            {
                Console.WriteLine($"Start creating UDTs:");
                GeneratUDT.Generate();
                Console.WriteLine($"Create UDTs Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred while creating a UDT. message: {ex.Message}");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Create UDFs for SAP UDT
        /// </summary>
        /// <returns></returns>
        private bool CreateUDFInUDT()
        {
            try
            {
                Console.WriteLine($"Start creating UDF for SAP UDT:");
                GeneratUDF.GenToUDT();
                Console.WriteLine($"Create UDF of SAP UDT Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred while creating a UDT for an SAP table. message: {ex.Message}");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Create UDFs for SAP Origin Table
        /// </summary>
        /// <returns></returns>
        private bool CreateUDFInTable()
        {
            try
            {
                Console.WriteLine($"Start creating UDF for SAP Tables:");
                GeneratUDF.GenToSysTable();
                Console.WriteLine($"Create UDF of SAP Tables Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred while creating a UDF for an SAP table. message: {ex.Message}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Set UDT and UDF for SAP
        /// </summary>
        public void Setting()
        {
            try
            {
                RecordHtml.InputMsg("<h2>UDT</h2> <hr/>");
                CreateUDTInSAP();
                RecordHtml.InputMsg("<h2>UDF for UDT</h2> <hr/>");
                CreateUDFInUDT();
                RecordHtml.InputMsg("<h2>UDF for Table</h2> <hr/>");
                CreateUDFInTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Set UDT and UDF Error:" + ex.Message);
            }
        }
    }
}