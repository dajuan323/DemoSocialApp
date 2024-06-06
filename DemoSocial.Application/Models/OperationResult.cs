﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Models;

public class OperationResult<T>
{
    public T Payload { get; set; }
    public bool IsError { get; set; }
    public List<Error> Errors { get; set; } = [];
}
