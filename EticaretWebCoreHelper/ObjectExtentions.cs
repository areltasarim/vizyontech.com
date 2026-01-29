using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace EticaretWebCoreHelper
{
    public static class ObjectExtentions
    {
        public static DataTableFilter ToDataTableFilter(this IFormCollection form)
        {
            DataTableFilter result=new DataTableFilter();

            int totalRecord = 0;
            int filterRecord = 0;
            var draw = form["draw"].FirstOrDefault();
            var sortColumn = form["columns[" + form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = form["order[0][dir]"].FirstOrDefault();
            var searchValue = form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(form["start"].FirstOrDefault() ?? "0");

    

            if (pageSize == 0)
            {
                pageSize = 25;
            }
            result.totalRecord = totalRecord;
            result.filterRecord = filterRecord;
            result.draw = draw;
            result.sortColumn = sortColumn;
            result.sortColumnDirection = sortColumnDirection;
            result.searchValue = searchValue;
            result.pageSize = pageSize;
            result.skip = skip;
    
            return result;
        }
    }
}