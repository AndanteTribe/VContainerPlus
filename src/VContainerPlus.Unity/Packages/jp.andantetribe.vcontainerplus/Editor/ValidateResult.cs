#nullable enable

using System;
using System.Collections.Generic;

namespace VContainerPlus.Editor
{
    /// <summary>
    /// Represents the result of a dependency validation operation.
    /// </summary>
    public sealed class ValidateResult
    {
        private readonly List<FailedItem> _results = new List<FailedItem>();

        /// <summary>
        /// Gets the list of validation failures.
        /// </summary>
        public IReadOnlyList<FailedItem> Results => _results;

        /// <summary>
        /// Gets whether the validation passed with no errors.
        /// </summary>
        public bool IsValid => _results.Count == 0;

        /// <summary>
        /// Adds a validation failure to the result.
        /// </summary>
        /// <param name="type">The type that failed validation.</param>
        /// <param name="message">The error message.</param>
        internal void AddFail(Type type, string message) => _results.Add(new FailedItem(type, message));

        internal ValidateResult()
        {
        }

        /// <summary>
        /// Formats all validation failures into a human-readable string.
        /// </summary>
        /// <returns>A formatted string containing all validation errors, or a success message if valid.</returns>
        public string FormatFailedResults()
        {
            if (IsValid)
            {
                return "No validation errors.";
            }
            var sb = new System.Text.StringBuilder(_results.Count * 16);
            sb.AppendLine("Validation Failed:");
            foreach (var item in _results)
            {
                sb.Append("- ");
                sb.Append(item.Type.FullName);
                sb.Append(": ");
                sb.AppendLine(item.Message);
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// Represents a single validation failure item.
    /// </summary>
    public readonly struct FailedItem
    {
        /// <summary>
        /// Gets the type that failed validation.
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// Gets the error message describing the validation failure.
        /// </summary>
        public readonly string Message;

        internal FailedItem(Type type, string message)
        {
            Type = type;
            Message = message;
        }

        /// <summary>
        /// Deconstructs the failed item into its components.
        /// </summary>
        /// <param name="type">The type that failed validation.</param>
        /// <param name="message">The error message.</param>
        public void Deconstruct(out Type type, out string message)
        {
            type = Type;
            message = Message;
        }
    }
}