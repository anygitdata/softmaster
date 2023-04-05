using System.Xml.Linq;
using WebSoftMast_02.Models;
using WebSoftMast_02.Tools;

namespace WebSoftMaster_02.Test.ModelsApi
{
    public class NatSheet_CreateKeyID__test
    {

        [Fact]
        public void NatSheet__CreateKeyId__withStr()
        {

            DateTime.TryParse("30.06.2019 14:07:00", out DateTime dt);
            
            long yy = dt.Year - 2000;
            var mm = dt.Month;
            var dd = dt.Day;
            var data = ((yy * 100 + mm) * 100 + dd) * 100000 + 2236;

            var trainNum = "2236";
            var res = NatSheet.CreateKeyID(trainNum, dt);

            Assert.NotNull(res);
            Assert.IsType<long>(res);

            Assert.Equal(data, res);

        }


        [Fact]
        public void NatSheet__Try_CreateKeyID__test()
        {
            var xEl = XElement.Parse("<row><TrainNumber>2236</TrainNumber><WhenLastOperation>30.06.2019 14:07:00</WhenLastOperation></row>");

            long dataKeyId = 19063002236;

            var valDt = DateTime.Parse("30.06.2019 14:07:00");

            var res = NatSheet.Try_CreateKeyID(xEl, out Err_parsXML_NatSheet dataRes);

            Assert.True(res);

            Assert.NotNull(dataRes.NatSheetId);
            Assert.True(dataRes.NatSheetId>0);
            Assert.False(string.IsNullOrEmpty(dataRes.TrainNumber));
            Assert.Equal(dataKeyId, dataRes.NatSheetId);
            Assert.Equal("2236", dataRes.TrainNumber);

            Assert.NotNull(dataRes.WhenLastOperation);
            Assert.IsType<DateTime>(dataRes.WhenLastOperation);
            Assert.Equal(valDt, dataRes.WhenLastOperation);

            Assert.Equal("ok", dataRes.Err);


        }


        [Fact]
        public void NatSheet__Try_CreateKeyID_trainNum_isErr_dateTime__test()
        {
            var sXML = @"<row>
    <TrainNumber>2226а</TrainNumber>
    <TrainIndexCombined>86380-177-98570</TrainIndexCombined>
    <FromStationName>ЧЕРКАСОВ КАМЕНЬ</FromStationName>
    <ToStationName>МЫС АСТАФЬЕВА (ЭКСП.)</ToStationName>
    <LastStationName>НОВАЯ</LastStationName>
    <WhenLastOperation>30.06.201914:35:00</WhenLastOperation>
    <LastOperationName>ОТПРАВЛЕНИЕ ВАГОНА В СОСТАВЕ ПОЕЗДА СО СТАНЦИИ</LastOperationName>
    <InvoiceNum>ЭЛ314035</InvoiceNum>
    <PositionInTrain>41</PositionInTrain>
    <CarNumber>63676878</CarNumber>
    <FreightEtsngName>УГОЛЬ КАМЕННЫЙ МАРКИ Т-ТОЩИЙ</FreightEtsngName>
    <FreightTotalWeightKg>74950</FreightTotalWeightKg>
  </row>";

            var xEl = XElement.Parse(sXML);

            var res = NatSheet.Try_CreateKeyID(xEl, out Err_parsXML_NatSheet dataRes);

            Assert.False(res);

            Assert.Null(dataRes.NatSheetId);
            Assert.Null(dataRes.WhenLastOperation);

            Assert.Equal("2226а", dataRes.TrainNumber);

            Assert.Equal("Error converting to DateTime: 30.06.201914:35:00", dataRes.Err);
        }


        [Fact]
        public void NatSheet__Try_CreateKeyID_trainNum_isStr__test()
        {
            var xEl = XElement.Parse("<row><TrainNumber>2236а</TrainNumber><WhenLastOperation>30.06.2019 14:07:00</WhenLastOperation></row>");

            long dataKeyId = 19063002236;

            var valDt = DateTime.Parse("30.06.2019 14:07:00");

            var res = NatSheet.Try_CreateKeyID(xEl, out Err_parsXML_NatSheet dataRes);

            Assert.True(res);

            Assert.NotNull(dataRes.NatSheetId);
            Assert.True(dataRes.NatSheetId > 0);
            Assert.False(string.IsNullOrEmpty(dataRes.TrainNumber));
            Assert.Equal(dataKeyId, dataRes.NatSheetId);
            Assert.Equal("2236а", dataRes.TrainNumber);

            Assert.NotNull(dataRes.WhenLastOperation);
            Assert.IsType<DateTime>(dataRes.WhenLastOperation);
            Assert.Equal(valDt, dataRes.WhenLastOperation);

            Assert.Equal("ok", dataRes.Err);


        }

    }
}
