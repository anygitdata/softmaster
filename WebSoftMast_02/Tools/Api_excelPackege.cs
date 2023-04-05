using OfficeOpenXml;
using OfficeOpenXml.Utils;
using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace WebSoftMast_02.Tools
{
    public class Api_excelPackege
    {

        public class ContentExcel
        {
            public int Id { get; set; }
            public string SKey { get; set; }
            public string Data { get; set; }
            public string Type { get; set; }                 

        }


        public class Title_NatSheet
        {
            #region property class
            public int TrainNumber { get; set; }
            public string TrainIndexCombined { get; set; }
            public string ToStationName { get; set; }
            public DateTime WhenLastOperation { get; set; }


            public int TrainIndex { get {

                    var ar = TrainIndexCombined.Split("-");

                    int res = 0;

                    // TODO: Модифицировать код через return ResProc
                    try
                    {
                        res = int.Parse(ar[1]);
                    }
                    catch (Exception ex)
                    {

                    }

                    return res; 
                
                } }


            #endregion

            public static ResProcNext<Title_NatSheet> Init_data(ExcelRange cells)
            {
                var resProc = new ResProcNext<Title_NatSheet>();

                var natSheet = new Title_NatSheet();

                int row = 3; //Row is the tile data
                int col3 = 3;
                int col5 = 5;

                try
                {
                    natSheet.TrainNumber = int.Parse(cells[row, col3].Value.ToString());

                    natSheet.ToStationName = cells[row++, col5].Value.ToString();

                    // TODO: Решить вопрос с оформлением natSheet.TrainIndexCombined 
                    var strNum_train = cells[row, col3].Value.ToString();
                    if (int.TryParse(strNum_train, out int train))
                        natSheet.TrainIndexCombined = $"86560-{strNum_train}-98470";
                    else
                        throw new Exception($"No data for TrainIndexCombined: {strNum_train}");


                    var strDt = cells[row, col5].Value.ToString();
                    natSheet.WhenLastOperation = DateTime.Parse(strDt);


                    resProc.DataT = natSheet;

                    resProc.Result = true; 

                }
                catch (Exception ex)
                {
                    resProc.Message = ex.Message;
                }


                return resProc;
            }

        }


        public static ResProcNext<List<ContentExcel>> Load_FromExcel(string file)
        {
            var resProc = new ResProcNext<List<ContentExcel>>();


            // TODO: убрать прямое присвоение path for excel file
            file = Path.Combine(Directory.GetCurrentDirectory(), "Upload", "NL_simple.xlsx") ;

            FileInfo fileInfo = new FileInfo(file);
            if (!File.Exists(file))
            {
                resProc.Message = "Нет файла " +  fileInfo.Name;
                return resProc;
            }

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet? worksheet = null;

                foreach (var ws in package.Workbook.Worksheets)
                    worksheet = ws as ExcelWorksheet;


                var resData = Title_NatSheet.Init_data(worksheet.Cells);
                


            }


            return resProc;

        }

    }
}
