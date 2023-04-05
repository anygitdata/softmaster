using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using WebSoftMast_02.Tools;

namespace WebSoftMast_02.Models
{
    public class NatSheet
    {
        #region Columns

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long NatSheetId { get; set; }

        
        [Column(TypeName = "char(5)")]
        public string TrainNumber { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string TrainIndexCombined { get; set; } = "";

        [Column(TypeName = "varchar(250)")]
        public string? FromStationName { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string? ToStationName { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string? LastStationName { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? WhenLastOperation { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string? LastOperationName { get; set; }


        public IEnumerable<Detail>? Details { get; set; }

        #endregion


        private static Regex rgIndex = new Regex(@"(\d+)-(\d+)-(\d+)");
        private static Regex rgTrNumber = new Regex(@"\d+");


        public int Get_trainIndex { get {
                var index = rgIndex.Match(TrainIndexCombined).Groups[2].Value;

                return int.Parse(index);
            
            } } 


        /// <summary>
        /// на каждый день только один поезд с номером trainNumber 
        /// </summary>
        /// <param name="trainNumber"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long CreateKeyID(string trainNumber, DateTime? dateT)
        {
            DateTime dt = dateT ?? DateTime.Now;

            long yy = dt.Year - 2000;
            var mm = dt.Month;
            var dd = dt.Day;
            var sTr = rgTrNumber.Match(trainNumber).Value;

            var tr = int.Parse(sTr);

            long keyId = ((yy * 100 + mm) * 100 + dd) * 100000 + tr;

            // Последние 00000 замещаются номером поезда
            // символы в номере поезда исключаются из результата регВыражения


            return keyId;

        }

        public static bool Try_CreateKeyID(XElement xEl, out Err_parsXML_NatSheet resData)
        {
            var sDt = xEl.Element("WhenLastOperation").Value;

            var traingNumber = xEl.Element("TrainNumber").Value;

            if (!DateTime.TryParse(sDt, out DateTime dt))
            {
                resData = new Err_parsXML_NatSheet(null, traingNumber, null, $"Error converting to DateTime: {sDt}");

                return false;
            }


            var keyId = CreateKeyID(traingNumber, dt);

            resData = new Err_parsXML_NatSheet(keyId, traingNumber, dt, "ok");
                

            return true;
        }

    }
}
