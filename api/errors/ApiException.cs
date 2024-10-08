using System;

namespace api.errors;

public class ApiExceptions ( int statuscode , string message , string? details)
{
    public int StatuscCode { get; set; } = statuscode;
    public string Message { get; set; } = message ; 
    public string? Details { get; set; } = details ;

}
