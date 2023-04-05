using WebSoftMast_02.Models;
using WebSoftMast_02.Tools;

namespace WebSoftMaster_02.Test.API_XML_operations
{
    public class Api_XML__test
    {


        [Fact]
        public void Api_XML__Conv_XMLdata_intoModels_parsXML()
        {
            var ls = new List<Document_NatSheet>();

            var context = DataContext.Get_DataContext();

            var row = @"<row>
    <TrainNumber>2236</TrainNumber>
    <TrainIndexCombined>86560-725-98470</TrainIndexCombined>
    <FromStationName>САРБАЛА</FromStationName>
    <ToStationName>НАХОДКА (ЭКСП.)</ToStationName>
    <LastStationName>ЧЕРНОРЕЧЕНСКАЯ</LastStationName>
    <WhenLastOperation>30.06.2019 14:07:00</WhenLastOperation>
    <LastOperationName>ОТПРАВЛЕНИЕ  ВАГОНА В СОСТАВЕ ПОЕЗДА СО СТАНЦИИ</LastOperationName>
    <InvoiceNum>ЭЛ598121</InvoiceNum>
    <PositionInTrain>33</PositionInTrain>
    <CarNumber>63731863</CarNumber>
    <FreightEtsngName>УГОЛЬ КАМЕННЫЙ МАРКИ Т-ТОЩИЙ</FreightEtsngName>
    <FreightTotalWeightKg>74700</FreightTotalWeightKg>
  </row>";

            Api_XML.Conv_XMLdata_intoModels_parsXML(context, row, ls);


        }
    }
}
