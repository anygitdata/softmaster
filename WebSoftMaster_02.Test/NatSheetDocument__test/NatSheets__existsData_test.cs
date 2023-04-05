using WebSoftMast_02.Models;
using WebSoftMast_02.Tools;

namespace WebSoftMaster_02.Test.NatSheetDocument__test
{
    public class NatSheets__existsData_test
    {
        [Fact]
        public void Document_NatSheet__existsData() 
        {
            var res = Document_NatSheet.ExistsData_inDB;

            var context = DataContext.Get_DataContext();

            var exists = context.NatSheets.Any();

            Assert.Equal(exists, res);
        }
    }
}
