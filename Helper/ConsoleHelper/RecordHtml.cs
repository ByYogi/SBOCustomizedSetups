using System.IO;
using System.Text;

namespace UDTnFGenerator.Helper
{
    public class RecordHtml
    {
        private static StringBuilder consoleOutputSB { get; set; } = new StringBuilder();
        public static void SaveToHtml(string filePath)
        {
            string htmlHeader = @"
        <html>
        <head>
            <title>Console Output to HTML</title>

			<style>
                .err { color: rgb(255, 101, 101); }
                .succ { color: rgb(81, 199, 81); }
                .mark { color: rgb(64, 151, 250); }
                :root{background-color: rgb(39, 39, 39); color: rgb(187, 187, 187);}
            </style>
        </head>
        <body>";

            string htmlFooter = @"
        </body>
        </html>";


            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(htmlHeader);
                writer.Write(consoleOutputSB.ToString());
                writer.Write(htmlFooter);
            }
        }
        public static void InputGroupStart(string groupName)
        {
            consoleOutputSB.AppendLine($"<fieldset><legend>{groupName}</legend>");
        }
        public static void InputGroupEnd()
        {
            consoleOutputSB.AppendLine("</fieldset><br/>");
        }
        public static void InputMsg(string content)
        {
            consoleOutputSB.AppendLine(content);
        }
        public static void InputMsgLine(string content)
        {
            consoleOutputSB.AppendLine($"<p>{content}</p>");
        }
        public static void InputErrMsg(string content)
        {
            consoleOutputSB.AppendLine($"<p><span class='err'> {content} </span></p>");
        }

        public static void InputSuccMsg(string content)
        {
            consoleOutputSB.AppendLine($"<p><span class='succ'> {content} </span></p>");
        }
        public static void InputMarkMsg(string content)
        {
            consoleOutputSB.AppendLine($"<p><span class='mark'> {content} </span></p>");
        }

        public static void RecordUDFInfo(string table, string field, string fieldType)
        {
            string outputMsg = $"<p>UDF for {table} with <span class='succ'>{field}</span> and {fieldType} type Added Successfully.</p>";
            consoleOutputSB.AppendLine(outputMsg);
        }
        public static void RecordUDTInfo(string table)
        {
            string outputMsg = $"<p>UDT with <span class='succ'>{table}</span> Added Successfully.</p>";
            consoleOutputSB.AppendLine(outputMsg);
        }
    }
}
