namespace EticaretWebCoreHelper
{
    public class DataTableFilter
    {
        public int totalRecord { get; set; } = 0;
        public string draw { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDirection { get; set; }
        public string searchValue { get; set; } = "";
        public int filterRecord { get; set; } = 0;
        public int pageSize { get; set; } = 25;
        public int skip { get; set; } = 0;




    }
}


