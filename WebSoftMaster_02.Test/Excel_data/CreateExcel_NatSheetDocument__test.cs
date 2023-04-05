using WebSoftMast_02.Tools;
using WebSoftMast_02.Tools.Excel;

namespace WebSoftMaster_02.Test.Excel_data
{
    public class CreateExcel_NatSheetDocument__test
    {
        /*[Fact]
        public void CreateExcel_NatSheetDocument__createFile()
        {
            long id = 19063002226;
            var resProc = Api_XML.CreateExcel_NatSheetDocument(null, id);

            Assert.True(resProc.Result);
        }*/


        [Fact]
        public void CreateExcel_NatSheetDocument__createFile()
        {
            long id = 19063002226;

            var natSheet_excel = new NatSheet_excel(null, EType_excel.natSheet, id);

            var resProc = natSheet_excel.CreateExcel_NatSheetDocument();

            Assert.True(resProc.Result);
        }


        [Fact]
        public void CreateExcel_List_NatSheetDocument__createFile()
        {
            var list_natSheet_excel = new NatSheet_list_excel(null, EType_excel.listDoc, null);

            var resProc = list_natSheet_excel.CreateExcel_NatSheetDocument();

            Assert.True(resProc.Result);

        }

    }
}
