// Copyright (c) Andrew Arnott. All rights reserved.

namespace Spss
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// An exception thrown when a variable has a name that conflicts with
    /// the name of another variable in the same <see cref="SpssVariablesCollection"/>.
    /// </summary>
    [Serializable]
    public class SpssVariableNameConflictException : ApplicationException
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SpssVariableNameConflictException"/> class
        /// for deserialization.
        /// </summary>
        public SpssVariableNameConflictException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpssVariableNameConflictException"/> class
        /// for deserialization.
        /// </summary>
        protected SpssVariableNameConflictException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpssVariableNameConflictException"/> class
        /// for leaving a custom message.
        /// </summary>
        public SpssVariableNameConflictException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpssVariableNameConflictException"/> class.
        /// </summary>
        /// <param name="newVarName">
        /// The new name of the variable being changed, which caused the conflict.
        /// </param>
        /// <param name="oldVarName">
        /// The original variable name that was acceptable.
        /// </param>
        public SpssVariableNameConflictException(string newVarName, string oldVarName)
            : base("The variable name you have selected conflicts with an existing variable.")
        {
            this.oldVarName = oldVarName;
            this.newVarName = newVarName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpssVariableNameConflictException"/> class
        /// Thrown when a variable is being added that conflicts with another variable.
        /// </summary>
        /// <param name="addedVarName">
        /// The name of the variable being added, which caused the conflict.
        /// </param>
        public SpssVariableNameConflictException(string addedVarName)
            : this(addedVarName, (string)null)
        {
        }
        #endregion

        #region Attributes
        private readonly string oldVarName;

        /// <summary>
        /// Gets the original variable name that was acceptable, if applicable.
        /// </summary>
        public string OldVarName => this.oldVarName;

        private readonly string newVarName;

        /// <summary>
        /// Gets the new name of the variable being changed, which caused the conflict.
        /// </summary>
        public string NewVarName => this.newVarName;
        #endregion
    }
}
