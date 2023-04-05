using WebSoftMast_02.Models;

using EApi_models = WebSoftMast_02.Tools.EApi_models;

namespace WebSoftMaster_02.Test.ModelsApi
{
    public class Api_models__test
    {

        [Fact]
        public void Try_getMaxID__natSheet()
        {

            var res = Api_models.Try_getMaxID(null, EApi_models.natSheet, out (int maxId, string mes) resData);

            Assert.False(res);

            Assert.Equal("Нет обработчика для natSheet", resData.mes);

            Assert.True(resData.maxId == 0);

        }


        [Fact]
        public void Try_getMaxID__detail()
        {

            var res = Api_models.Try_getMaxID(null, EApi_models.detail, out (int maxId, string mes) resData);

            Assert.True(res);

            Assert.Equal("ok", resData.mes);

            Assert.True(resData.maxId > 0);

        }


        [Fact]
        public void Try_getMaxID__tmp_natSheet()
        {

            var res = Api_models.Try_getMaxID(null, EApi_models.tmp_natSheet, out (int maxId, string mes) resData);

            Assert.True(res);

            Assert.Equal("ok", resData.mes);

            Assert.True(resData.maxId > 0);

        }


        [Fact]
        public void Try_getMaxID__tmp_detail()
        {

            var res = Api_models.Try_getMaxID(null, EApi_models.tmp_detail, out (int maxId, string mes) resData);

            Assert.True(res);

            Assert.Equal("ok", resData.mes);

            Assert.True(resData.maxId > 0);

        }


        [Fact]
        public void Try_getMaxID__empty()
        {

            var res = Api_models.Try_getMaxID(null, EApi_models.empty, out (int maxId, string mes) resData);

            Assert.False(res);

            Assert.Equal("Нет обработчика для empty", resData.mes);

            Assert.True(resData.maxId == 0);

        }


    }
}
