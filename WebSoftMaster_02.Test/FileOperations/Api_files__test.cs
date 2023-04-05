using WebSoftMast_02.Tools;

namespace WebSoftMaster_02.Test.FileOperations
{
    public class Api_files__test
    {
        [Fact]
        public void Api_files__Path_uploads()
        {
            var res = Api_files.Path_uploads;

            Assert.True(!string.IsNullOrEmpty(res));
        }


        [Fact]
        public void Api_files__Path_downloads()
        {
            var res = Api_files.Path_downloads;

            Assert.True(!string.IsNullOrEmpty(res));
        }


    }
}
