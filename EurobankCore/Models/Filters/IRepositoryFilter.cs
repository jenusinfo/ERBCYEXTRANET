﻿using CMS.DataEngine;

using Eurobank.Infrastructure;

namespace Eurobank.Models
{
    /// <summary>
    /// Defines a method to get a <see cref="WhereCondition"/> representing the filter configuration.
    /// </summary>
    public interface IRepositoryFilter : ICacheKey
    {
        /// <summary>
        /// Returns a filter <see cref="WhereCondition"/>.
        /// </summary>
        WhereCondition GetWhereCondition();
    }
}
