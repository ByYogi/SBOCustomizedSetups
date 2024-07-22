namespace UDTnFGenerator.Helper
{
    //this class is to be responsible for the instantiation appsetting value
    public class SAPConfigCls
    {
        public static string LicenseServer = System.Configuration.ConfigurationManager.AppSettings["LicenseServer"].ToString();
        public static string SBOServer = System.Configuration.ConfigurationManager.AppSettings["SBOServer"].ToString();
        public static string SQLUserName = System.Configuration.ConfigurationManager.AppSettings["SQLUserName"].ToString();
        public static string SQLPassword = System.Configuration.ConfigurationManager.AppSettings["SQLPassword"].ToString();
        public static string SQLVersion = System.Configuration.ConfigurationManager.AppSettings["SQLVersion"].ToString();
        public static string SAPUser = System.Configuration.ConfigurationManager.AppSettings["SAPUser"].ToString();
        public static string SAPPassword = System.Configuration.ConfigurationManager.AppSettings["SAPPassword"].ToString();
        public static string Database = System.Configuration.ConfigurationManager.AppSettings["Database"].ToString();
        //public static string CreateView = System.Configuration.ConfigurationManager.AppSettings["CreateView"];
        //public static string CreateUDF = System.Configuration.ConfigurationManager.AppSettings["CreateUDF"];
    }
}