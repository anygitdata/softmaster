using OfficeOpenXml;
using System;
using WebSoftMast_02.Models;

namespace WebSoftMast_02.Tools.Excel
{
    public class NatSheet_excel : Abstr_excel
    {
        public NatSheet_excel(DataContext? context, EType_excel typeExcel, long? natSheetId=null) : base(context, typeExcel, natSheetId) { }

        public override ResProc CreateExcel_NatSheetDocument()
        {
            var resProc = new ResProc();

            var rowBase = 7;
            var row = rowBase;

            Document_NatSheetGrData resGrData;

            try
            {
                resGrData = new Document_NatSheetGrData(Context, (long)NatSheetId);
            }
            catch (Exception ex)
            {
                resProc.Message = ex.Message;
                return resProc;
            }

            using (ExcelPackage package = new ExcelPackage(new FileInfo(PathExcel_Dist)))
            {
                var natSheet = resGrData.NatSheet;

                foreach (var item in package.Workbook.Worksheets)
                    Sheet = item;


                Sheet.Cells[3, 3].Value = natSheet.TrainNumber;
                Sheet.Cells[4, 3].Value = natSheet.Get_trainIndex; // .TrainIndexCombined.Split("-")[1];

                Sheet.Cells[3, 5].Value = natSheet.ToStationName;
                Sheet.Cells[4, 5].Value = natSheet.WhenLastOperation;

                Set_Numberformat(new int[] {4, 6}, "dd-MM-yyyy");


                // Данные по вагонам
                var num = 0;
                foreach (var item in resGrData.LsDetails)
                {
                    Sheet.Cells[row, 1].Value = ++num;
                    Sheet.Cells[row, 2].Value = item.CarNumber;
                    Sheet.Cells[row, 3].Value = item.InvoiceNum;
                    Sheet.Cells[row, 4].Value = natSheet.WhenLastOperation;
                    Sheet.Cells[row, 5].Value = item.FreightEtsngName;
                    Sheet.Cells[row, 6].Value = (decimal)item.FreightTotalWeightKg / 1000;

                    Sheet.Cells[row, 7].Value = natSheet.LastOperationName;

                    row++;
                }


                Set_Numberformat(new int[] {rowBase, 4, row, 4}, "dd-MM-yyyy");

                // Сводные данные
                var rowGr = row;
                foreach (var item in resGrData.Ls_GrData_natSheet)
                {
                    Sheet.Cells[row, 2].Value = item.NumCount;
                    Sheet.Cells[row, 5].Value = item.Info;
                    Sheet.Cells[row, 6].Value = item.SumAll;

                    row++;
                }

                var arRange = new int[] { row, 1, row, 2 };
                var cells = Get_range(arRange);

                cells.Merge = true;
                cells.Value = $"Всего: {resGrData.AllCount}";

                Set_HorizontalAlignment_Cent(arRange);
                Set_StyleFont_bold(arRange);


                Sheet.Cells[row, 5].Value = resGrData.GrCount;
                Sheet.Cells[row, 6].Value = resGrData.SumGr;


                // заключительные стили 
                Set_HorizontalAlignment_Cent(new int[] {rowBase,1, row, 6});

                Set_Numberformat(new int[] {rowBase, 6, row, 6 }, "# ##0.000");

                Set_StyleFont_bold(new int[] {rowGr, 2, row, 6});

                arRange = new int[] { row, 5, row, 6 };
                Set_HorizontalAlignment_Cent(arRange);
                Set_StyleFont_bold(arRange);

                arRange = new int[] {rowBase, 1, row, 7 };
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
