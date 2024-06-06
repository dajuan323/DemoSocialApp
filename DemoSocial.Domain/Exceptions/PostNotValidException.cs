using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Domain.Exceptions;

public class PostNotValidException : NotValidException
{
    internal PostNotValidException() {}

    internal PostNotValidException(string message) : base(message) {}

    internal PostNotValidException(string message, Exception innerValue) : base(message, innerValue) {}
}
