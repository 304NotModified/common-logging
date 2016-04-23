﻿
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Common.Logging.MultipleLogger
{
    /// <summary>
    /// A global context for logger variables
    /// </summary>
    public class MultiLoggerThreadVariablesContext : IVariablesContext
    {
        private readonly List<ILog> _loggers;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLoggerThreadVariablesContext"/> class.
        /// </summary>
        /// <param name="loggers">The loggers.</param>
        public MultiLoggerThreadVariablesContext(IEnumerable<ILog> loggers)
        {
            _loggers = loggers.ToList();
        }

        /// <summary>
        /// Sets the value of a new or existing variable within the global context
        /// </summary>
        /// <param name="key">The key of the variable that is to be added</param>
        /// <param name="value">The value to add</param>
        public void Set(string key, object value)
        {
            _loggers.ForEach(logger => logger.ThreadVariablesContext.Set(key, value));
        }

        /// <summary>
        /// Gets the value of a variable within the global context
        /// </summary>
        /// <param name="key">The key of the variable to get</param>
        /// <returns>The value or null if not found</returns>
        public object Get(string key)
        {
            //note: this impl. assumes that all loggers in the multi-logger collection have the same value for the key
            // this is a safe assumption *only* because of the enforced semantics around the Set() implementation for this class 
            return _loggers.FirstOrDefault(logger => logger.ThreadVariablesContext.Get(key) != null)?.ThreadVariablesContext.Get(key);
        }

        /// <summary>
        /// Checks if a variable is set within the global context
        /// </summary>
        /// <param name="key">The key of the variable to check for</param>
        /// <returns>True if the variable is set</returns>
        public bool Contains(string key)
        {
            return _loggers.Any(logger => logger.ThreadVariablesContext.Contains(key));
        }

        /// <summary>
        /// Removes a variable from the global context by key
        /// </summary>
        /// <param name="key">The key of the variable to remove</param>
        public void Remove(string key) {
            _loggers.ForEach(logger => logger.ThreadVariablesContext.Remove(key));
        }

        /// <summary>
        /// Clears the global context variables
        /// </summary>
        public void Clear() {
            _loggers.ForEach(logger => logger.ThreadVariablesContext.Clear());
        }
    }
}