using WebSoftMast_02.Models;
using WebSoftMast_02.Tools;

namespace WebSoftMaster_02.Test.API_XML_operations
{
    public class Api_XML__loadData__test
    {
        [Fact]
        public async void Api_XML__Conv_XMLdata_intoModels()
        {
            var res = Api_XML.Conv_XMLdata_intoModels(null, "Data.xml");

            if (DataContext.Get_DataContext().NatSheets.Any())
                Assert.False(res.Result);
            else
            {
                Assert.True(res.Result);
                Assert.True(res.DataT.Count > 0);

                var resSave = await Document_NatSheet.Save_LS_DocumentsAsync(res.DataT);

                Assert.True(resSave.Result);
            }

        }


        [Fact]
        public async void Api_XML__Conv_XMLdata_intoModels__withExistsData()
        {
            var file = "DataNext.xml";
            var res = Api_XML.Conv_XMLdata_intoModels(null, file);

            Assert.False(res.Result);

        }
    }
}
