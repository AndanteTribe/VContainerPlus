#nullable enable

using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace VContainerPlus
{
    /// <summary>
    /// Base class for <see cref="LifetimeScope"/> with automatic component binding.
    /// </summary>
    public class LifetimeScopeBase : LifetimeScope
    {
        [SerializeField]
        private Component[] _autoBindComponents = Array.Empty<Component>();

#if UNITY_EDITOR
        private bool _passConfigure = false;
#endif

        /// <summary>
        /// Configures the container by registering auto-bind components.
        /// </summary>
        /// <param name="builder">The container builder to configure.</param>
        protected override void Configure(IContainerBuilder builder)
        {
#if UNITY_EDITOR
            _passConfigure = true;
#endif

            // Register all auto-bind components with their interfaces.
            foreach (var component in _autoBindComponents.AsSpan())
            {
                if (component != null)
                {
                    builder.RegisterInstance(component, component.GetType()).AsImplementedInterfaces();
                }
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Attempts to configure the builder for debug validation purposes.
        /// Verifies that the Configure method properly registers auto-bind components.
        /// </summary>
        /// <param name="builder">The container builder to configure.</param>
        /// <returns>True if configuration was successful and components were registered; otherwise, false.</returns>
        internal bool TryConfigureDebug(IContainerBuilder builder)
        {
            _passConfigure = false;
            Configure(builder);
            return _passConfigure;
        }
#endif
    }
}
