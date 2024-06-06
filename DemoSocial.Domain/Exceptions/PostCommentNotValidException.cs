using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Domain.Exceptions;

public class PostCommentNotValidException : NotValidException
{
    internal PostCommentNotValidException() {}

    internal PostCommentNotValidException(string message) : base(message) {}

    internal PostCommentNotValidException(string message, Exception innerValue): base(message, innerValue) {}
}
