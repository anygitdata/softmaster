using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using WebSoftMast_02.Tools;

namespace WebSoftMast_02.Models
{

    public record GrData_natSheet(int NumCount, string Info, decimal SumAll);


    public class Model_NatSheet
    {
        public long NatSheetId { get; set; }
        public string TrainNumber { get; set; }
        public string TrainIndexCombined { get; set; }
        public string? FromStationName { get; set; }
        public string? ToStationName { get; set; }
        public string? LastStationName { get; set; }
        public DateTime? WhenLastOperation { get; set; }
        public string? LastOperationName { get; set; }
    }


    public class Model_NatSheetExt: Model_NatSheet
    {
        public List<Detail> Ls_details { get; set; }

    }

    public class Document_NatSheetBase
    {
        public NatSheet NatSheet { get; set; }
        public List<Detail> LsDetails { get; set; } = new List<Detail>();
    }


    public class Document_NatSheetGrData: Document_NatSheetBase
    {
        public List<GrData_natSheet>? Ls_GrData_natSheet { get; set; }

        /// <summary>
        /// Кол-во вагонов по ВСЕМ группам
        /// </summary>
        public int AllCount { get; set; }

        /// <summary>
        /// Кол-во групп
        /// </summary>
        public int GrCount { get; set; }

        /// <summary>
        /// Общая сумма по ВСЕМ группам
        /// </summary>
        public decimal SumGr { get; set; }

        
        public Document_NatSheetGrData(DataContext? context, long natSheetId)
        {

            if (context == null)
                context = DataContext.Get_DataContext();

            var resDoc = Document_NatSheet.Create_Document(context, natSheetId);

            if (!resDoc.Result)
                throw new Exception(resDoc.Message);


            NatSheet = resDoc.DataT.NatSheet;
            LsDetails = resDoc.DataT.LsDetails;


            Ls_GrData_natSheet = new List<GrData_natSheet>();

            var detailGr = LsDetails.GroupBy(p => p.FreightEtsngName);

            decimal weightGr = 0M;
            foreach (var item in detailGr)
            {
                weightGr = (decimal) item.Sum(p => p.FreightTotalWeightKg)/1000;
                Ls_GrData_natSheet.Add(
                    new GrData_natSheet(item.Count(), item.Key, weightGr) );
                
                SumGr += weightGr;
                GrCount++;
            }

            AllCount = Ls_GrData_natSheet.Sum(p=> p.NumCount);
        }

    }


    public class Document_NatSheet: Document_NatSheetBase
    {
        #region any data of class
        private static int _max_DetailId = -1;
        public static int Get_Max_DetailId()
        {
            if (_max_DetailId < 0)
            {
                if (!Api_models.Try_getMaxID(null, EApi_models.detail, out (int maxId, string mes) resData))
                    throw new Exception(resData.mes);

                _max_DetailId = resData.maxId;
            }
            else
                _max_DetailId++;

            return _max_DetailId;

        }

        private static bool _rowExists = true;

        public static void Update_Params_dataExists(DataContext? context)
        {
            if (context is null)
                context = DataContext.Get_DataContext();

            _rowExists = context.NatSheets.AsNoTracking().Any();

        }

        static Document_NatSheet()
        {
            Update_Params_dataExists(null);
        }

        public static bool ExistsData_inDB { get => _rowExists; }

        #endregion


        public static async Task<ResProc> Save_LS_DocumentsAsync(List<Document_NatSheet> ls)
        {
            var resProc = new ResProc();

            if (Api_XML.Ls_Err_parsXML_NatSheet != null && Api_XML.Ls_Err_parsXML_NatSheet.Count > 0)
            {
                resProc.Message = "Не обработан список ошибок или пропущенных документов";
                return resProc;
            }

            var lsNatSheet = new List<NatSheet>();

            var lsDetails = new List<Detail>();

            foreach (var item in ls)
            {
                lsNatSheet.Add(item.NatSheet);
                lsDetails.AddRange(item.LsDetails);
            }

            using (var context = DataContext.Get_DataContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.AddRangeAsync(lsNatSheet);
                        context.AddRangeAsync(lsDetails);

                        context.SaveChanges();
                        trans.Commit();

                        Update_Params_dataExists(context);

                        resProc.Result = true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        resProc.Message = ex.Message;
                    }
                }
            }

            return resProc;
        }

        public static ResProcNext<Document_NatSheet> Create_Document(DataContext? context, long natSheetId)
        {
            var resProc = new ResProcNext<Document_NatSheet>();

            if (context is null)
                context = DataContext.Get_DataContext();

            var item = context.NatSheets.Find(natSheetId);

            if (item is null)
            {
                resProc.Message = $"No data for {natSheetId}";
                return resProc;
            }

            var details = context.Details.Where(r => r.NatSheetId == item.NatSheetId).OrderBy(r => r.PositionInTrain);


            resProc.DataT = new Document_NatSheet { NatSheet = item, LsDetails = details.ToList() };

            resProc.Result = true;

            return resProc;
        }


        public static ResProcNext<Model_NatSheetExt> Create_Model_NatSheet(DataContext context, long natSheetId)
        {
            var resPoc = new ResProcNext<Model_NatSheetExt>();

            var resDoc = Create_Document(context, natSheetId);
            var natSheet = resDoc.DataT.NatSheet;

            var resData = new Model_NatSheetExt            {
                NatSheetId = natSheet.NatSheetId,
                FromStationName = natSheet.FromStationName,
                LastOperationName = natSheet.LastOperationName,
                LastStationName = natSheet.LastStationName,
                ToStationName = natSheet.ToStationName,
                TrainIndexCombined = natSheet.TrainIndexCombined,
                TrainNumber = natSheet.TrainNumber,
                WhenLastOperation = natSheet.WhenLastOperation
            };


            resData.Ls_details = new List<Detail>();

            var details = context.Details.Where(p => p.NatSheetId == natSheet.NatSheetId).ToList();
            foreach (var item in details)
            {
                resData.Ls_details.Add(
                    new Detail { DetailId = item.DetailId,
                        InvoiceNum = item.InvoiceNum,
                        CarNumber = item.CarNumber,                        
                        FreightEtsngName = item.FreightEtsngName,
                        FreightTotalWeightKg = item.FreightTotalWeightKg,
                        PositionInTrain = item.PositionInTrain,
                        NatSheetId = item.NatSheetId
                    } );
            }


            resPoc.DataT = resData;
            resPoc.Result = true;

            return resPoc; 
        }


        /// <summary>
        /// Выборка последней записи. Записей TrainNumber м/быть несколько. 
        /// комбинация TrainNumber and WhenLastOperation уникальная
        /// </summary>
        /// <param name="context"></param>
        /// <param name="trnumber"></param>
        /// <returns></returns>
        public static ResProcNext<Model_NatSheet> Create_Model_NatSheet(DataContext context, string trnumber)
        {
            var resPoc = new ResProcNext<Model_NatSheet>();

            var data = context.NatSheets.Where(p => p.TrainNumber == trnumber).AsNoTracking();

            if (!data.Any())
            {
                resPoc.Message = "Нет данных";
                resPoc.DataT = new Model_NatSheet();

                return resPoc;
            }

            var natSheetId = data.Max(p => p.NatSheetId);

            var resDat = Create_Model_NatSheet(context, natSheetId);

            resPoc.DataT = resDat.DataT;
            resPoc.Result = true;

            return resPoc;
        }

    }
}
