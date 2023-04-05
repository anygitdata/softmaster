using Microsoft.EntityFrameworkCore;
using WebSoftMast_02.Tools;

namespace WebSoftMast_02.Models
{
    public class SeedData
    {

        public static async Task Load_SeedDataAsync()
        {
            using (var context = DataContext.Get_DataContext())
            {
                if (context.NatSheets.AsNoTracking().Any()) return;

                var path = Path.Combine(Directory.GetCurrentDirectory(), "Models", "seedData.xml");

                var res = Api_XML.Conv_XMLdata_intoModels(context, path, EFull_path.fullPath);

                if (!res.Result &&
                        (Api_XML.Ls_Err_parsXML_NatSheet != null && Api_XML.Ls_Err_parsXML_NatSheet.Count > 0)) return;

                await Document_NatSheet.Save_LS_DocumentsAsync(res.DataT);
            }

        }
        

    }
}
