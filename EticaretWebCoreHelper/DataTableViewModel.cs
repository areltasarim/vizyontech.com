using EticaretWebCoreEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EticaretWebCoreHelper
{
    public class DataTableViewColumn
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Title { get; set; }

    }

    public class DataTableViewOrder
    {
        public int Id { get; set; } = 0;
        public string Asc { get; set; } = "asc";
    }

    public class DataTableViewModel
    {

        public string TableId { get; set; } = "";
        public string Url { get; set; }
        public string Type { get; set; } = "post";
        public string DataType { get; set; } = "json";
        public bool Processing { get; set; } = true;
        public bool ServerSide { get; set; } = true;
        public bool Filter { get; set; } = true;
        public bool OrderMulti { get; set; } = true;
        public bool Searchable { get; set; } = true;
        public bool Orderable { get; set; } = true;

        public bool StateSave { get; set; } = true;

        public DataTableViewOrder Order { get; set; }
        public List<DataTableViewColumn> Columns { get; set; } = new List<DataTableViewColumn>();
    }

}
