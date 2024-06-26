﻿namespace DemoSocial.Domain.Exceptions;

public class PostCommentNotValidException : NotValidException
{
    internal PostCommentNotValidException() {}

    internal PostCommentNotValidException(string message) : base(message) {}

    internal PostCommentNotValidException(string message, Exception innerValue): base(message, innerValue) {}
}
