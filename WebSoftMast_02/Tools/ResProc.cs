namespace WebSoftMast_02.Tools
{
    public class ResProc
    {
        public bool Result { get; set; } = false;
        public string Message { get; set; } = "Ok";
        public bool ChangeData { get; set; } = false;

        public int ValInt { get; set; }
        public long ValLong { get; set; }
        public object? valObj { get; set; }
    }

    public class ResProcNext<T> : ResProc
    {
        public T DataT { get; set; }

    }

}
