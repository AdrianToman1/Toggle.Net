﻿using System.Collections.Generic;

namespace Toggle.Net.Specifications
{
    /// <summary>
    ///     Decides if a <see cref="Feature" /> is enabled ot not.
    ///     Implementation is shared between different features,
    ///     so make sure you don't keep state that cannot be shared on this instance.
    /// </summary>
    public interface IToggleSpecification
    {
        /// <summary>
        ///     Is the <see cref="Feature" /> enabled or not?
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool IsEnabled(IDictionary<string, string> parameters);
    }
}