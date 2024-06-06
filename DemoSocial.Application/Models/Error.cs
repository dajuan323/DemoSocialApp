using DemoSocial.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Models;

public record Error
{
    public ErrorCode Code { get; set; }
    public string Message { get; set; }
};

