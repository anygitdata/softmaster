using System.Text;

namespace WebSoftMast_02.Tools
{

    public class Api_files
    {
        private static string _pathUploads;
        public static string Path_uploads { get { 
                if (string.IsNullOrEmpty(_pathUploads))
                {
                    var dir = Directory.GetCurrentDirectory();
                    _pathUploads = Path.Combine(dir, "Upload");
                }

                return _pathUploads; } }

        private static string _pathDownload;
        public static string Path_downloads { get
            {
                if (string.IsNullOrEmpty(_pathDownload))
                {
                    var dir = Directory.GetCurrentDirectory();
                    _pathDownload = Path.Combine(dir, "DownloadDoc");
                }

                return _pathDownload;

            } }


        public static ResProc Write_Doc_existsNatSheet(string sXML="")
        {
            
            var resProc = new ResProc();

            var dir = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

            if (!File.Exists(dir))
                Directory.CreateDirectory(dir);

            var date = DateTime.Now;

            var file = $"list_write_Doc_existsNatSheet_{date.Year}-{date.Month}-{date.Day}.log";
            var path = Path.Combine(dir, file);

            try
            {
                using (var wr = new StreamWriter(path, false, Encoding.UTF8))
                {
                    var ls = Api_XML.Ls_Err_parsXML_NatSheet;

                    var sb = new StringBuilder();
                    sb.AppendLine("Список не обработанных натурных документов");
                    sb.AppendLine($"File xml: {sXML}\n");

                    if (ls != null)
                    {
                        foreach (var item in ls)
                        {
                            sb.AppendLine($"{item.NatSheetId,12} {item.TrainNumber,6}  {item.WhenLastOperation}  {item.Err}");
                        }
                    }
                    else
                        sb.Append("Пустой список не обработанных документов");

                    wr.Write(sb.ToString());
                }

                resProc.Result = true;
                resProc.Message = file;

            }
            catch (Exception e)
            {
                resProc.Message = e.Message;
            }


            return resProc;
        }


    }

}
