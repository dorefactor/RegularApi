using System.Collections.Generic;

namespace RegularApi.Controllers.Views
{
    
//    {"errors":{"Tag":["The Tag field is required."]},"title":"One or more validation errors occurred.","status":400,"traceId":"0HLLSN25PT5QL"}    
    public class ErrorResponse
    {
        public int Status { get; set; }
        public string TraceId { get; set; }
        public string Title { get; set; }
        public IList<IDictionary<string, string>> Errors { get; set; }        
    }
}