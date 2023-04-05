using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSoftMast_02.Models
{

    public class Detail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DetailId { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string? InvoiceNum { get; set; }         // Номер накладной
        public int PositionInTrain { get; set; }        // Позиция вагона в составе 
        public int CarNumber { get; set; }              // Номер вагона 

        [Column(TypeName = "varchar(250)")]
        public string? FreightEtsngName { get; set; }   // Наименование груза 
        public int FreightTotalWeightKg { get; set; }   // Общий вес вагона с грузом 


        public long NatSheetId { get; set; }
        public NatSheet? NatSheet { get; set; }
    }

}
