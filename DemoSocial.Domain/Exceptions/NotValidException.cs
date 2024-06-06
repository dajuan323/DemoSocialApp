using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Domain.Exceptions;

public class NotValidException : Exception
{
    internal NotValidException()
    {
        List<string> ValidationErrors = [];
    }

    internal NotValidException(string message) : base(message)
    {
        List<string> ValidationErrors = [];
    }

    internal NotValidException(string message, Exception innerValue) : base(message, innerValue)
    {
        List<string> ValidationErrors = [];
    }
    public List<string> ValidationErrors { get; }
}
