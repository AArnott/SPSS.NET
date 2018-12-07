// Copyright (c) Andrew Arnott. All rights reserved.

namespace Spss
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    internal class SpssVariableKeyedCollection : KeyedCollection<string, SpssVariable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpssVariableKeyedCollection"/> class.
        /// </summary>
        public SpssVariableKeyedCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Gets the key for item.
        /// </summary>
        /// <param name="item">The item.</param>
        protected override string GetKeyForItem(SpssVariable item)
        {
            return item.Name;
        }
    }
}
