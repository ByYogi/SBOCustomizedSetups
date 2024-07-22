using SAPbobsCOM;
using System;
using System.Linq;
using System.Threading.Tasks;
using UDTnFGenerator.Model;

namespace UDTnFGenerator.Helper.SAPData
{
    internal class SAPObjectCreator
    {
        private static Company _company = new Company();
        public static Company Company { get { return _company; } }
        public static Task<bool> ConnectCompanyAsync()
        {
            Console.WriteLine($"{SAPConfigCls.Database} SAP Connecting... ");
            return Task.Run(() =>
            {
                if (Company.Connected) return true;
                _company.Server = SAPConfigCls.SBOServer;
                _company.CompanyDB = SAPConfigCls.Database;
                _company.DbServerType = (BoDataServerTypes)Enum.Parse(typeof(BoDataServerTypes), Convert.ToString(SAPConfigCls.SQLVersion));
                _company.DbUserName = SAPConfigCls.SQLUserName;
                _company.DbPassword = SAPConfigCls.SQLPassword;
                _company.LicenseServer = SAPConfigCls.LicenseServer;
                _company.UserName = SAPConfigCls.SAPUser;
                _company.Password = SAPConfigCls.SAPPassword;
                _company.language = BoSuppLangs.ln_English;

                if (Company.Connect() != 0)
                {
                    int errCode = 0;
                    string errDesc = null;
                    Company.GetLastError(out errCode, out errDesc);
                    Console.WriteLine($"SAP Failed to Connect! {errCode} {errDesc}");
                    return false;
                }

                Console.WriteLine("SAP Connected successfully! ", false, true);
                return true;
            });
        }
        public static void DisposeSAPObject()
        {
            if (_company != null && _company.Connected == true)
                Company.Disconnect();
            _company = null;
        }

        /// <summary>
        /// The "U_" prefix is automatically added to the column name.
        /// <br/>
        /// <returns>Is Success</returns>
        public static bool CreateUDF(String tableName, String fieldName, String fieldDescription, BoFieldTypes fieldType, int fieldSize = 0,
            BoFldSubTypes subfieldType = BoFldSubTypes.st_None, String fieldValues = "", String defaultValue = "", bool mandatory = false,
            String linkTable = null)
        {
            Recordset oRecset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            UserFieldsMD oUDF = (UserFieldsMD)Company.GetBusinessObject(BoObjectTypes.oUserFields);
            string outputMsg;
            //Declarations for SQL As Boolean
            try
            {
                string sqlScript = @"select Top 1 ""FieldID"" from " + Company.CompanyDB + ".dbo" + @".CUFD where ""TableID"" = '" + tableName + @"' and ""AliasID"" = '" + fieldName + "'";
                oRecset.DoQuery(sqlScript);
                //Execute Selected Query
                if (oRecset.RecordCount != 0)
                {
                    outputMsg = $"UDF for {tableName} with {fieldName} type Already Exists.";
                    Console.WriteLine(outputMsg);
                    RecordHtml.InputMsgLine(outputMsg);
                    return true;
                }
                //The previous resource needs to be released before the next SAP data operation can be performed
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecset);
                oUDF.Name = fieldName;
                oUDF.Type = fieldType;
                if (fieldSize > 0) oUDF.Size = oUDF.EditSize = fieldSize;

                oUDF.Description = fieldDescription;
                oUDF.TableName = tableName;
                oUDF.SubType = subfieldType;
                oUDF.Mandatory = mandatory ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                if (!string.IsNullOrEmpty(fieldValues.Trim()))
                {
                    foreach (string s1 in fieldValues.Split('|'))
                    {
                        if (string.IsNullOrEmpty(s1.Trim())) continue;
                        string[] s2 = s1.Split('-');
                        oUDF.ValidValues.Value = s2[0];
                        oUDF.ValidValues.Description = (s2.Length > 1 ? string.Join("-", s2.Skip(1)) : s1); //Situation: val1|val2-2; : val1|val2; 
                        oUDF.ValidValues.Add();
                        oUDF.DefaultValue = defaultValue;
                    }
                }

                oUDF.LinkedTable = linkTable;
                if (oUDF.Add() != 0)
                {
                    throw new Exception($"{Company.GetLastErrorDescription()}");
                }
                Console.ForegroundColor = ConsoleColor.Green;//set text color is green
                outputMsg = $"UDF for {tableName} with {fieldName} and {fieldType} type Added Successfully." + Environment.NewLine;
                Console.WriteLine(outputMsg);
                //RecordHtml.InputSuccMsg(outputMsg);
                RecordHtml.RecordUDFInfo(tableName, fieldName, fieldType.ToString());

                Console.ResetColor();
                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;//set text color is red
                outputMsg = $"UDF for {tableName} with {fieldName} and {fieldType} type Add Error! message: {ex.Message}" + Environment.NewLine;
                Console.WriteLine(outputMsg);
                RecordHtml.InputErrMsg(outputMsg);
                Console.ResetColor();
                throw new Exception(ex.Message);
            }
            finally
            {
                //If the resource is not freed, an exception is thrown: Failed to add! Ref count for this object is higher then 0
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecset);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUDF);
                GC.Collect();
            }
        }


        public static bool CreateUDF(UDFInfo udfInfo)
        {
            Recordset oRecset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            UserFieldsMD oUDF = (UserFieldsMD)Company.GetBusinessObject(BoObjectTypes.oUserFields);
            string outputMsg;
            string tableName = udfInfo.TableName;
            string fieldName = udfInfo.FieldName;
            BoFieldTypes fieldType = udfInfo.FieldType;
            //Declarations for SQL As Boolean
            try
            {
                string sqlScript = @"select Top 1 ""FieldID"" from " + Company.CompanyDB + ".dbo" + @".CUFD where ""TableID"" = '" + tableName + @"' and ""AliasID"" = '" + fieldName + "'";
                oRecset.DoQuery(sqlScript);
                //Execute Selected Query
                if (oRecset.RecordCount != 0)
                {
                    outputMsg = $"UDF for {tableName} with {fieldName} type Already Exists.";
                    Console.WriteLine(outputMsg);
                    RecordHtml.InputMsgLine(outputMsg);
                    return true;
                }
                //The previous resource needs to be released before the next SAP data operation can be performed
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecset);
                oUDF.Name = fieldName;
                oUDF.Type = fieldType;
                if (udfInfo.FieldSize.HasValue)
                {
                    oUDF.Size = oUDF.EditSize = udfInfo.FieldSize.Value;
                }

                oUDF.Description = udfInfo.Description;
                oUDF.TableName = tableName;
                oUDF.SubType = udfInfo.FieldSubType;
                oUDF.Mandatory = udfInfo.Mandatory ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                oUDF.DefaultValue = udfInfo.DefaultValue;
                if (udfInfo.ValidValues != null)
                {
                    foreach (var itm in udfInfo.ValidValues)
                    {
                        string validValue = itm.Value.Trim();
                        string valueDesc = itm.Description;

                        if (string.IsNullOrEmpty(validValue)) continue;

                        oUDF.ValidValues.Value = validValue;
                        oUDF.ValidValues.Description = valueDesc;
                        oUDF.ValidValues.Add();
                    }
                }

                //Link other object
                if (null != udfInfo.LinkedSystemObject)
                {
                    oUDF.LinkedSystemObject = udfInfo.LinkedSystemObject.Value;
                }
                else if (!string.IsNullOrEmpty(udfInfo.LinkedUDO))
                {
                    oUDF.LinkedUDO = udfInfo.LinkedUDO;
                }
                else if (!string.IsNullOrEmpty(udfInfo.LinkedTable))
                {
                    oUDF.LinkedTable = udfInfo.LinkedTable;
                }

                if (oUDF.Add() != 0)
                {
                    throw new Exception($"{Company.GetLastErrorDescription()}");
                }
                Console.ForegroundColor = ConsoleColor.Green;//set text color is green
                outputMsg = $"UDF for {tableName} with {fieldName} and {fieldType} type Added Successfully." + Environment.NewLine;
                Console.WriteLine(outputMsg);
                //RecordHtml.InputSuccMsg(outputMsg);
                RecordHtml.RecordUDFInfo(tableName, fieldName, fieldType.ToString());

                Console.ResetColor();
                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;//set text color is red
                outputMsg = $"UDF for {tableName} with {fieldName} and {fieldType} type Add Error! message: {ex.Message}" + Environment.NewLine;
                Console.WriteLine(outputMsg);
                RecordHtml.InputErrMsg(outputMsg);
                Console.ResetColor();
                throw new Exception(ex.Message);
            }
            finally
            {
                //If the resource is not freed, an exception is thrown: Failed to add! Ref count for this object is higher then 0
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecset);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUDF);
                GC.Collect();
            }
        }


        /// <summary>
        /// The "@_" prefix is automatically added to the table name.
        /// <br/>
        /// Code and name fields will be added automatically, And Code is Primary Key.
        /// </summary>
        /// <param name="tableName">Database Table Name</param>
        /// <param name="description">Show description in SAP</param>
        /// <param name="tableType">Specification table types for SAP</param>
        /// <returns>Is Success</returns>
        public static bool CreateUDT(String tableName, String description, BoUTBTableType tableType)
        {
            string outputMsg;
            UserTablesMD oUDT = default(UserTablesMD);
            try
            {
                oUDT = (UserTablesMD)Company.GetBusinessObject(BoObjectTypes.oUserTables);
                oUDT.GetByKey(tableName);
                if (oUDT.GetByKey(tableName))
                {
                    outputMsg = $"UDT with {tableName} Already Exists";
                    Console.WriteLine(outputMsg);
                    RecordHtml.InputMsgLine(outputMsg);
                    return true;
                }

                oUDT.TableName = tableName;
                oUDT.TableDescription = description;
                oUDT.TableType = tableType;
                int iRet = oUDT.Add();
                if (iRet != 0)
                {
                    throw new Exception($"{Company.GetLastErrorDescription()}");
                }

                Console.ForegroundColor = ConsoleColor.Blue;//set text color is blue
                outputMsg = $"UDT with {tableName} Added Successfully!" + Environment.NewLine;
                Console.WriteLine(outputMsg);
                //RecordHtml.InputMarkMsg(outputMsg);
                RecordHtml.RecordUDTInfo(tableName);
                Console.ResetColor();
                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                outputMsg = $"UDT with {tableName} Add Error! {ex.Message}" + Environment.NewLine;
                Console.WriteLine(outputMsg);
                RecordHtml.InputErrMsg(outputMsg);
                Console.ResetColor();
                return false;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUDT);
            }
        }

        //public void CreateUDO(string udoCode, string udoName)
        //{
        //    UserObjectsMD userObject = (UserObjectsMD)Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
        //    userObject.Code = udoCode;
        //    userObject.Name = udoName;
        //    userObject.TableName = "YourUDOTableName";
        //    userObject.ObjectType = BoUDOObjType.boud_Document; // 根据需要设置UDO类型
        //    userObject.CanCancel = BoYesNoEnum.tYES;
        //    userObject.CanClose = BoYesNoEnum.tYES;
        //    userObject.CanDelete = BoYesNoEnum.tYES;
        //    userObject.CanFind = BoYesNoEnum.tYES;
        //    userObject.CanYearTransfer = BoYesNoEnum.tYES;
        //    userObject.ManageSeries = BoYesNoEnum.tYES;
        //    userObject.EnableEnhancedForm = BoYesNoEnum.tYES;
        //    userObject.MenuItem = BoYesNoEnum.tNO;
        //    userObject.FatherMenuID = 0; // 如果作为菜单项，指定父菜单ID
        //    int result = userObject.Add();

        //    if (result == 0)
        //    {
        //        Console.WriteLine("UDO created successfully.");
        //    }
        //    else
        //    {
        //        string errorDescription = Company.GetLastErrorDescription();
        //        Console.WriteLine($"Failed to create UDO. Error: {errorDescription}");
        //    }
        //}
    }
}
