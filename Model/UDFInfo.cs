using SAPbobsCOM;
using System.Collections.Generic;

namespace UDTnFGenerator.Model
{
    internal class UDFInfo
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string Description { get; set; }
        public BoFieldTypes FieldType { get; set; }
        public BoFldSubTypes FieldSubType { get; set; }
        public int? FieldSize { get; set; }
        public bool Mandatory { get; set; }
        public string DefaultValue { get; set; }
        public string LinkedTable { get; set; }
        public UDFLinkedSystemObjectTypesEnum? LinkedSystemObject { get; set; }
        public string LinkedUDO { get; set; }
        public List<UDFInfo_ValidValue> ValidValues { get; set; }

        //Set Default Value
        internal UDFInfo()
        {
            SetDefaultValue();
        }
        internal UDFInfo(string fieldName, string description, BoFieldTypes fieldType, int? fieldSize = null)
        {
            SetDefaultValue();

            //TableName = tableName;
            FieldName = fieldName;
            Description = description;
            FieldType = fieldType;
            FieldSize = fieldSize;
        }

        private void SetDefaultValue()
        {
            FieldSubType = BoFldSubTypes.st_None;
            Mandatory = false;
        }
        internal UDFInfo SetLinkedSystemObject(UDFLinkedSystemObjectTypesEnum obj)
        {
            LinkedSystemObject = obj;
            return this;
        }
        internal UDFInfo SetLinkedTable(string table)
        {
            LinkedTable = table;
            return this;
        }
        internal UDFInfo SetLinkedUDO(string udo)
        {
            LinkedUDO = udo;
            return this;
        }

    }

    internal class UDFInfo_ValidValue
    {
        public string Value { get; set; }
        public string Description { get; set; }
        internal UDFInfo_ValidValue()
        {
        }
        internal UDFInfo_ValidValue(string value, string description)
        {
            Value = value;
            Description = description;
        }
    }
}
