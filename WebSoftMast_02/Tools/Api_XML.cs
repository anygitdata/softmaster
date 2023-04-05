using Microsoft.EntityFrameworkCore;
using System.Xml;
using System.Xml.Linq;
using WebSoftMast_02.Models;

namespace WebSoftMast_02.Tools
{

    public record Err_parsXML_NatSheet (long? NatSheetId, string TrainNumber, DateTime? WhenLastOperation, string Err);


    public class Api_XML
    {

        // Буфер документов не прошедших проверку или с ошибками загрузки
        public static List<Err_parsXML_NatSheet>? Ls_Err_parsXML_NatSheet = null;


        public static void Conv_XMLdata_intoModels_parsXML(DataContext? context, string xml, List<Document_NatSheet> ls)
        {
            Action<Err_parsXML_NatSheet> _write_into_Ls_Doc_errNatSheet = (dataPars) =>
            {
                if (Ls_Err_parsXML_NatSheet is null) Ls_Err_parsXML_NatSheet = new List<Err_parsXML_NatSheet>();

                Ls_Err_parsXML_NatSheet.Add(
                    new Err_parsXML_NatSheet(dataPars.NatSheetId,
                        dataPars.TrainNumber, dataPars.WhenLastOperation, dataPars.Err)
                    );
            };


            if (context is null)
                context = DataContext.Get_DataContext();
            

            var xEl = XElement.Parse(xml);

            if (!NatSheet.Try_CreateKeyID(xEl, out Err_parsXML_NatSheet dataPars))
            {
                _write_into_Ls_Doc_errNatSheet(dataPars); // Запись в журнал не обработанных документов

                return ;
            }


            try
            {
                Document_NatSheet? docNatSheet = ls.FirstOrDefault(p => 
                            p.NatSheet.NatSheetId == dataPars.NatSheetId);

                if ( docNatSheet == null)
                {
                    docNatSheet = new Document_NatSheet
                    {
                        NatSheet = new NatSheet
                        {
                            NatSheetId = (long) dataPars.NatSheetId,
                            TrainNumber = dataPars.TrainNumber,
                            TrainIndexCombined = xEl.Element("TrainIndexCombined").Value,
                            WhenLastOperation = dataPars.WhenLastOperation,
                            FromStationName = xEl.Element("FromStationName").Value,
                            ToStationName = xEl.Element("ToStationName").Value,
                            LastStationName = xEl.Element("LastStationName").Value,
                            LastOperationName = xEl.Element("LastOperationName").Value
                        }
                    };

                    ls.Add( docNatSheet);


                    // если документ был помещен в буфер документов не прошедших проверку
                    // дальнейшую обработку пропускаем
                    if (Ls_Err_parsXML_NatSheet != null && Ls_Err_parsXML_NatSheet.Any(p => p.NatSheetId == dataPars.NatSheetId))
                        return;

                    if (Document_NatSheet.ExistsData_inDB)  // блокирование повторного ввода документа
                    {
                        if (context.NatSheets.AsNoTracking().Any(p => p.NatSheetId == dataPars.NatSheetId))
                        {
                            var dataParsErr = new Err_parsXML_NatSheet(dataPars.NatSheetId, dataPars.TrainNumber, dataPars.WhenLastOperation, "Повторный ввод документа");

                            _write_into_Ls_Doc_errNatSheet(dataParsErr);

                            return;
                        }
                    }
                }


                // Заполнение данных по вагонам
                docNatSheet.LsDetails.Add(
                    new Detail
                    {
                        DetailId = Document_NatSheet.Get_Max_DetailId(),
                        PositionInTrain = (int)xEl.Element("PositionInTrain"),
                        CarNumber = (int)xEl.Element("CarNumber"),
                        InvoiceNum = xEl.Element("InvoiceNum").Value,
                        FreightEtsngName = xEl.Element("FreightEtsngName").Value,
                        FreightTotalWeightKg = (int)xEl.Element("FreightTotalWeightKg"),
                        NatSheetId = (long) dataPars.NatSheetId
                    });
            }
            catch (XmlException ex)
            {
                var dataParsErr = new Err_parsXML_NatSheet(dataPars.NatSheetId, dataPars.TrainNumber, dataPars.WhenLastOperation, ex.Message);

                _write_into_Ls_Doc_errNatSheet(dataParsErr);
            }
            catch (Exception ex)
            {
                var dataParsErr = new Err_parsXML_NatSheet(dataPars.NatSheetId, dataPars.TrainNumber, dataPars.WhenLastOperation, ex.Message);

                _write_into_Ls_Doc_errNatSheet(dataParsErr);
            }

            return;
            
        }

        public static ResProcNext<List<Document_NatSheet>> Conv_XMLdata_intoModels(DataContext? context, string file, EFull_path fullPath = EFull_path.shortPath)
        {

            var resProc = new ResProcNext<List<Document_NatSheet>>();

            Ls_Err_parsXML_NatSheet = null;

            if (context is null)
                context = DataContext.Get_DataContext();

            string path = "";
            if (fullPath == EFull_path.shortPath)
                path = Path.Combine(Api_files.Path_uploads, file);
            else
                path = file;

            if (!File.Exists(path))
            {
                resProc.Message = "Нет файла " + file;
                return resProc;
            }

            var ls_natSheet = new List<Document_NatSheet>();

            try
            {
                var pathSchema = Path.Combine(Directory.GetCurrentDirectory(), "DataSchema.xsd");

                XmlReaderSettings settings = new XmlReaderSettings();              

                settings.IgnoreWhitespace = true;
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, pathSchema);

                using (var r = XmlReader.Create(path, settings))
                {
                    r.MoveToContent();
                    r.ReadStartElement("Root");

                    do
                    {
                        if (r.NodeType == XmlNodeType.Element && r.Name == "row")
                        {
                            // ВСЕ сообщения записываются в буфер Ls_Doc_errNatSheet
                            // TODO: пересмотреть результат из Conv_XMLdata_intoModels_parsXML mes не используется
                            Conv_XMLdata_intoModels_parsXML(context, r.ReadOuterXml(), 
                                ls_natSheet);

                        }
                    }
                    while (r.Read());

                }

                if (Ls_Err_parsXML_NatSheet != null && Ls_Err_parsXML_NatSheet.Count>0)
                {
                    resProc.Message = "Документы с ошибками загрузки";
                    Api_files.Write_Doc_existsNatSheet(file);
                }
                else
                {
                    resProc.Result = true;

                    // для процедуры Document_NatSheet.Save_LS_DocumentsAsync
                    resProc.DataT = ls_natSheet;    
                }

            }
            catch (Exception ex)
            {
                resProc.Message = ex.Message;
            }


            return resProc;

        }

        

    }
}
