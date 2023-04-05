using OfficeOpenXml;
using System;
using WebSoftMast_02.Models;
using WebSoftMast_02.Pages.Natsheet;

namespace WebSoftMast_02.Tools.Excel
{
    public class NatSheet_list_excel: Abstr_excel
    {
        public NatSheet_list_excel(DataContext? context, EType_excel typeExcel, long? natSheetId = null) : base(context, typeExcel, natSheetId) { }

        public override ResProc CreateExcel_NatSheetDocument()
        {
            var resProc = new ResProc();

            var rowBase = 3;
            var row = rowBase;

            /*List<Item_NatSheet> ls_natSheet = new List<Item_NatSheet>();
            foreach (var item in Context.NatSheets.OrderBy(p => p.TrainNumber))
            {
                ls_natSheet.Add(
                    new Item_NatSheet
                    {
                        NatSheetId = item.NatSheetId,
                        TrainNumber = item.TrainNumber,
                        TrainIndexCombined = item.TrainIndexCombined,
                        ToStationName = item.ToStationName,
                        WhenLastOperation = (DateTime)item.WhenLastOperation
                    });
            }*/

            using (ExcelPackage package = new ExcelPackage(new FileInfo(PathExcel_Dist)))
            {
                foreach (var item in package.Workbook.Worksheets)
                    Sheet = item;


                var num = 0;
                foreach (var item in Context.NatSheets.OrderBy(p => p.TrainNumber))
                {
                    Sheet.Cells[row, 1].Value = ++num;
                    Sheet.Cells[row, 2].Value = int.Parse(item.TrainNumber);
                    Sheet.Cells[row, 3].Value = item.Get_trainIndex;                    
                    Sheet.Cells[row, 4].Value = item.WhenLastOperation;
                    Sheet.Cells[row, 5].Value = item.ToStationName ;

                    row++;
                }


                Set_Numberformat(new int[] { rowBase, 4, row, 4 }, "dd-MM-yyyy");
                Set_HorizontalAlignment_Cent(new int[] {rowBase,1, row, 4 });

                Set_Numberformat(new int[] { rowBase, 3, row, 3 }, "#000");

                var arRange = new int[] { rowBase, 1, row-1, 5 };
                Set_borderStyle(arRange, EType_excelBorder.bottom);
                Set_borderStyle(arRange, EType_excelBorder.left);
                Set_borderStyle(arRange, EType_excelBorder.right);


                package.Save();

                resProc.Result = true;
                resProc.Message = PathExcel_Dist;
            }


            return resProc; 

        }
    }
}
