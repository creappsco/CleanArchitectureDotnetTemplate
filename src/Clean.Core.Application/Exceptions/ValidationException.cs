﻿using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Clean.Core.Application.Exceptions
{
    /// <summary>
    /// This class represent a  exception that is generated by Validations errors
    /// </summary>
    public class ValidationException : ApplicationException
    {
        /// <summary>
        /// Validations Errors
        /// </summary>
        public List<string> ValidationErrors { get; set; }

        /// <summary>
        /// Create a new ValidationException
        /// </summary>
        /// <param name="validationResult">Validation Result object</param>
        public ValidationException(ValidationResult validationResult)
        {
            ValidationErrors = new List<string>();

            foreach (var validationError in validationResult.Errors)
            {
                ValidationErrors.Add(validationError.ErrorMessage);
            }
        }
    }
}
