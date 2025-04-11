using System;

namespace ReservaFacil.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)  { }
}

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}


