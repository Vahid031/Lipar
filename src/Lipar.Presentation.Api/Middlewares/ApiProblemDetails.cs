using System;
using System.Collections.Generic;
namespace Lipar.Presentation.Api.Middlewares;

public class ApiProblemDetails
{
    public string TraceId { get; set; }
    public string Details { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public string Instance { get; set; }
    //public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}


