using System;

namespace BCD.WebApi.Services.Exception
{
    public class NotFoundException : AccessViolationException
    {
        public NotFoundException(string message): base(message) {  }
    }
}