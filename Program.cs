using System;
using UDTnFGenerator.Helper;
using UDTnFGenerator.Helper.SAPData;

namespace UDTnFGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            TablePost postSAP = new TablePost();
            if (SAPObjectCreator.ConnectCompanyAsync().Result)
            {
                Console.WriteLine("Connect SAP Success");
                try
                {
                    postSAP.Setting();
                    string outputFilePath = $"udtlog_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.html";
                    RecordHtml.SaveToHtml(outputFilePath);

                    Console.WriteLine($"The output saved to ./{outputFilePath}");

                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("UDT&UDF Create Failed。Error：" + ex.Message);
                }
            }
            else
            {
                throw new Exception("Connect SAP Failed.");
            }
            SAPObjectCreator.DisposeSAPObject();
        }
    }
}
