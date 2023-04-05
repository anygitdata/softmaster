using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSoftMast_02.Models;

namespace WebSoftMaster_02.Test.ModelsApi
{
    public class Document_NatSheetGrData__test
    {
        [Fact]
        public void Document_NatSheetGrData_createDoc_test()
        {
            var context = DataContext.Get_DataContext();

            long id = 19063002226;
            var res = new Document_NatSheetGrData(context, id);

            var lsDetails = context.Details.Where(p => p.NatSheetId == id).AsNoTracking().ToList();

            var grData = lsDetails.GroupBy(p=> p.FreightEtsngName);

            decimal weight = 0M;
            int numCount = 0;
            int numGr = 0;
            foreach (var item in grData)
            {
                weight += (decimal)item.Sum(p => p.FreightTotalWeightKg)/1000;
                numCount += item.ToList().Count;
                numGr++;
            }

            Assert.Equal(numGr, res.GrCount);
            Assert.Equal(numCount, res.AllCount);
            Assert.Equal(weight, res.SumGr);
            

        }

    }
}
