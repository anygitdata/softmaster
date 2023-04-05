using Microsoft.EntityFrameworkCore;
using WebSoftMast_02.Models;
using WebSoftMast_02.Tools;

namespace WebSoftMaster_02.Test.NatSheetDocument__test
{
    // TODO: внести изменения с учетом того, что NatSheetId is long
    public class NatSheetDoc__testing
    {
        [Fact]
        public void Document_NatSheet_Create_Document__noData__test()
        {
            var resCreate = Document_NatSheet.Create_Document(null, 1); // такого номер не должно быть

            Assert.NotNull(resCreate);

            Assert.False(resCreate.Result);

            Assert.Equal("No data for 1", resCreate.Message);
        }


        [Fact]
        public void Document_NatSheet_Create_Document__test()
        {
            var context = DataContext.Get_DataContext();

            var id = context.NatSheets.AsNoTracking().FirstOrDefault().NatSheetId;

            var resCreate = Document_NatSheet.Create_Document(context, id); 

            Assert.NotNull(resCreate);

            Assert.True(resCreate.Result);

            Assert.NotNull(resCreate.DataT);

            Assert.IsType<Document_NatSheet>(resCreate.DataT);

        }


    }
}
