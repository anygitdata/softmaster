using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using WebSoftMast_02.Models;

namespace WebSoftMast_02.Tools.Excel
{
    abstract public class Abstr_excel
    {
        public string FileExcel_tmp { get; init; }
        public string PathExcel_tmp { get; init; }

        public string FileExcel_Dist { get; init; }
        public string PathExcel_Dist { get; init; }


        public long? NatSheetId { get; init; }

        public DataContext Context { get; init; }


        public ExcelWorksheet? Sheet { get; set; }

        public ExcelHorizontalAlignment alignMentCent = ExcelHorizontalAlignment.Center;


        #region Api programs
        public ExcelRange Get_range(int[] cells)
        {
            ExcelRange range;

            if (cells.Length == 4)
                range = Sheet.Cells[cells[0], cells[1], cells[2], cells[3]];
            else
                range = Sheet.Cells[cells[0], cells[1]];


            return range;
        }

        public void Set_Numberformat(int[] cells, string format )
        {
            // dd-MM-yyyy   # ##0.000

            ExcelRange range = Get_range(cells);

            range.Style.Numberformat.Format = format;

        }

        public ExcelBorderStyle borderStyle_Thin = ExcelBorderStyle.Thin;

        public void Set_borderStyle(int[] cells, EType_excelBorder excelBorder )
        {
            ExcelRange range = Get_range(cells);

            switch (excelBorder)
            {
                case EType_excelBorder.bottom:
                    range.Style.Border.Bottom.Style = borderStyle_Thin;
                    break;
                case EType_excelBorder.top:
                    range.Style.Border.Top.Style = borderStyle_Thin;
                    break;
                case EType_excelBorder.left:
                    range.Style.Border.Left.Style = borderStyle_Thin;
                    break; 
                case EType_excelBorder.right:
                    range.Style.Border.Right.Style = borderStyle_Thin;
                    break;
                default:
                    throw new NotImplementedException("Метод не определен");
            }

            range.Style.Border.Bottom.Style = borderStyle_Thin;
        }

        public void Set_HorizontalAlignment_Cent(int[] cells)
        {
            ExcelRange range = Get_range(cells);

            range.Style.HorizontalAlignment = alignMentCent;
        }

        public void Set_StyleFont_bold(int[] cells)
        {
            ExcelRange range = Get_range(cells);

            range.Style.Font.Bold = true;
        }

        #endregion

        public Abstr_excel(DataContext? context, EType_excel typeExcel, long? natSheetId=null)
        {
            if (natSheetId != null)
                NatSheetId = natSheetId;

            if (context is null)
                context = DataContext.Get_DataContext();

            Context = context;

            var _date = DateTime.Now;
            var dir = Directory.GetCurrentDirectory();

            var _sDateForm = _date.ToString("yyyMMdd");

            switch (typeExcel)
            {
                case EType_excel.natSheet:

                    FileExcel_tmp = "nat_sheet_tmp.xlsx";
                    FileExcel_Dist = $"{_sDateForm}_{NatSheetId}.xlsx";

                    break;

                case EType_excel.listDoc:

                    FileExcel_tmp = "natSheet_list_tmp.xlsx";
                    FileExcel_Dist = $"{_sDateForm}_natSheet_list_tmp.xlsx";

                    break;

                default:
                    throw new NotImplementedException("Метод не определен");
            }

            PathExcel_tmp = Path.Combine(dir, "tmp", FileExcel_tmp);
            PathExcel_Dist = Path.Combine(dir, "DownloadDoc", FileExcel_Dist);

            // Копирование шаблона 
            //Pathexcel_tmp = Path.Combine(dir, "tmp", FileExcel_tmp);

            if (!File.Exists(PathExcel_tmp))
                throw new FileNotFoundException("Нет файла:" + PathExcel_tmp);

            var fileInfo = new FileInfo(PathExcel_tmp);
            fileInfo.CopyTo(PathExcel_Dist, true);

        }


        public abstract ResProc CreateExcel_NatSheetDocument(); 


    }
}
