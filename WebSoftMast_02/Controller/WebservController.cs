using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.IO;
using WebSoftMast_02.Models;
using WebSoftMast_02.Pages.Natsheet;
using WebSoftMast_02.Tools;
using WebSoftMast_02.Tools.Excel;

namespace WebSoftMast_02.Controller
{
    // api/Webserv/all   api/Webserv/upload
    // api/Webserv/natsheet/2226  api/Webserv/natsheet/2226
    // api/Webserv/download/ls-natsheet
    [ApiController]
    [Route("api/[controller]")]
    public class WebservController : ControllerBase
    {
        static string _path_uploads = string.Empty;

        private DataContext context;
        

        Func<string, string> _Get_fileName = (formFile) => {

            var filePath = Path.Combine(_path_uploads, formFile);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            return filePath;

        };


        public WebservController(DataContext cont, IWebHostEnvironment env) 
        {
            context = cont;

            _path_uploads = Path.Combine(env.ContentRootPath, "Upload");
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadXML_async()
        {
            var files = HttpContext.Request.Form.Files;

            string filePath = string.Empty;
            

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    filePath = _Get_fileName(formFile.FileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            var resConv = Api_XML.Conv_XMLdata_intoModels(context, filePath);

            if (Api_XML.Ls_Err_parsXML_NatSheet != null && Api_XML.Ls_Err_parsXML_NatSheet.Count > 0)
                return Ok(new { result="err", listerr = Api_XML.Ls_Err_parsXML_NatSheet } );

            var resSave = await Document_NatSheet.Save_LS_DocumentsAsync(resConv.DataT);

            var fileName = Path.GetFileName(filePath);

            if (!resSave.Result)
                return Ok(new { result = "ok", file = fileName, info = resSave.Message});

            return Ok(new { result = "ok",  file = fileName, info = "Загружен в БД" });
        }


        [HttpGet("ls-natsheet")]
        public IActionResult DownloadExcel_List()
        {
            var natSheetList_excel = new NatSheet_list_excel(context, EType_excel.listDoc, null);
            var res = natSheetList_excel.CreateExcel_NatSheetDocument();

            if (!res.Result)
                return NotFound();

            var pathDist = res.Message;
            var fileName = Path.GetFileName(pathDist);

            return File(System.IO.File.OpenRead(pathDist), "application/*", fileName);

        }


        [HttpGet("download/{id:long}")]
        public IActionResult DownloadExcel_byIdAsync(long id)
        {
            var natSheet_excel = new NatSheet_excel(null, EType_excel.natSheet, id);
            var res = natSheet_excel.CreateExcel_NatSheetDocument();

            if (!res.Result)
                return NotFound();

            var pathDist = res.Message;
            var fileName = Path.GetFileName(pathDist);

            return File(System.IO.File.OpenRead(pathDist), "application/*", fileName);

        }


        [HttpGet("natsheet/{id}")]
        public Model_NatSheet GetNatSheet(string id)
        {
            var res = Document_NatSheet.Create_Model_NatSheet(context, id);

            return res.DataT;
        }


        [HttpGet("all")]
        public List<Item_NatSheet> GetAllNatSheet()
        {
            List<Item_NatSheet> ls_natSheet = new List<Item_NatSheet>();

            foreach (var item in context.NatSheets)
            {
                ls_natSheet.Add(
                    new Item_NatSheet
                    {
                        NatSheetId = item.NatSheetId,
                        TrainNumber = item.TrainNumber,
                        TrainIndexCombined = item.TrainIndexCombined,
                        ToStationName = item.ToStationName,
                        WhenLastOperation = (DateTime)item.WhenLastOperation
                    });
            }

            return  ls_natSheet;
        }


    }
}
