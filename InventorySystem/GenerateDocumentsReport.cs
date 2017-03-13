using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace InventorySystem
{
    class GenerateDocumentsReport
    {

        public void GenerateDeffectWorld(string inventnumber, string denomination, string mark, string responsible, string place)
        {
            Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
            Object missingObj = Missing.Value;
            Object templatePathObj = "templateDefect.docx";
            Document document = application.Documents.Add(ref templatePathObj, ref missingObj, ref missingObj,
                ref missingObj);


            var name = responsible.Split(' ');
            string resp = name[0] + " " + name[1][0] + ". " + name[2][0] + ".";
            object findPlace = "%%place%%";
            object findNumber = "%%inventnumber%%";
            object findDenomination = "%%Denomination%%";
            object findResponsible = "%%responsible%%";

            // диапазон документа Word
            Range wordRange;
            //тип поиска и замены
            object replaceTypeObj;
            replaceTypeObj = WdReplace.wdReplaceAll;
            // обходим все разделы документа
            for (int i = 1; i <= document.Sections.Count; i++)
            {
                // берем всю секцию диапазоном
                wordRange = document.Sections[i].Range;

                Find wordFindObj = wordRange.Find;
                object[] wordFindParameters = new object[15]
                {
                    findPlace, missingObj, missingObj, missingObj,
                    missingObj, missingObj, missingObj, missingObj,
                    missingObj, place, replaceTypeObj,
                    missingObj, missingObj, missingObj, missingObj
                };

                wordFindObj.GetType()
                    .InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);

                wordFindParameters = new object[15]
                {
                    findNumber, missingObj, missingObj, missingObj,
                    missingObj, missingObj, missingObj, missingObj,
                    missingObj, inventnumber, replaceTypeObj,
                    missingObj, missingObj, missingObj, missingObj
                };
                wordFindObj.GetType()
                    .InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);

                wordFindParameters = new object[15]
                {
                    findDenomination, missingObj, missingObj, missingObj,
                    missingObj, missingObj, missingObj, missingObj,
                    missingObj, denomination + mark, replaceTypeObj,
                    missingObj, missingObj, missingObj, missingObj
                };
                wordFindObj.GetType()
                    .InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);

                wordFindParameters = new object[15]
                {
                    findResponsible, missingObj, missingObj, missingObj,
                    missingObj, missingObj, missingObj, missingObj,
                    missingObj, resp, replaceTypeObj,
                    missingObj, missingObj, missingObj, missingObj
                };
                wordFindObj.GetType()
                    .InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);
            }


            application.Visible = true;
        }
        public void GenerateReportExcel( List <DataBaseService.Equipment> _equips)
        {
            Microsoft.Office.Interop.Excel.Application ObjExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook ObjWorkBook = ObjExcel.Workbooks.Add(Missing.Value);
            Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ObjWorkBook.Sheets[1];

            ObjExcel.StandardFont = "Times New Roman";
            ObjExcel.StandardFontSize = 14;


            var excelcells = ObjWorkSheet.Range["C2", "F2"];
            excelcells.Merge(Type.Missing);
            excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
            excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
            excelcells.Font.Bold = true;

            //Выводим число
            excelcells.Value2 = "Отчет за " + DateTime.Now;

            //Значения [y - строка,x - столбец]
            ObjWorkSheet.Cells[5, 1] = "Инвентарный номер";
            ObjWorkSheet.Cells[5, 2] = "Старый инвентарный номер";
            ObjWorkSheet.Cells[5, 3] = "Наименование";
            ObjWorkSheet.Cells[5, 4] = "Марка - модель";
            ObjWorkSheet.Cells[5, 5] = "Расположение";
            ObjWorkSheet.Cells[5, 6] = "Ответственное лицо";
            ObjWorkSheet.Cells[5, 7] = "Статус";
            ObjWorkSheet.Cells[5, 8] = "Примечание";

            excelcells = ObjWorkSheet.Range["A5", "H5"];
            excelcells.Font.Bold = true;


            int j = 6;
            foreach (var equip in _equips)
            {
                var res = new DataBaseService.Employee[1];
                res[0] = equip.Responsible;
                ObjWorkSheet.Cells[j, 1] = equip.InventoryNumber;
                ObjWorkSheet.Cells[j, 2] = equip.OldInventoryNumber;
                ObjWorkSheet.Cells[j, 3] = equip.Denomination.Naming;
                ObjWorkSheet.Cells[j, 4] = equip.Mark + " " + equip.Model;
                ObjWorkSheet.Cells[j, 5] = equip.City.Naming;
                ObjWorkSheet.Cells[j, 6] = DataBaseService.Service.GetEmployeeString(res)[0];
                ObjWorkSheet.Cells[j, 7] = equip.Status.Naming;
                ObjWorkSheet.Cells[j, 8] = equip.Comment;
                j++;
            }

            excelcells = ObjWorkSheet.get_Range("A5", "H" + (j - 1));
            excelcells.Borders.ColorIndex = 1;
            excelcells.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            excelcells.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

            ObjExcel.Visible = true;
            ObjExcel.UserControl = true;
        }
    }
}
