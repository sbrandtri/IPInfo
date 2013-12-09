using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace IPInfoTests
{
    /// <summary>
    /// Manages changes to the App.config file to easily test various configurations
    /// </summary>
    /// <remarks>
    /// NOTE: From http://stackoverflow.com/questions/6150644/change-default-app-config-at-runtime
    /// This is a very useful approach for unit testing!
    /// </remarks>
    /// <example>
    /// Usage example:
    ///     // the default app.config is used.
    ///     using(AppConfig.Change(tempFileName))
    ///     {
    ///         // the app.config in tempFileName is used
    ///     }
    ///     // the default app.config is used.
    /// </example>
    abstract class AppConfig : IDisposable
    {
        /// <summary>
        /// Change the active App.config file to the file specified.
        /// </summary>
        /// <remarks>
        /// The default App.config is restored when the returned object is disposed.
        /// </remarks>
        /// <param name="path">A full path to a valid App.config file.</param>
        /// <returns>An AppConfig instance to manage the change.</returns>
        public static AppConfig Change(string path)
        {
            return new ChangeAppConfig(path);
        }

        public abstract void Dispose();

        /// <summary>
        /// Changes the active App.config file on instantiation and restores the default App.config on disposal
        /// </summary>
        private class ChangeAppConfig : AppConfig
        {
            private readonly string _oldConfig =
                AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString();

            private bool _disposedValue;

            /// <summary>
            /// Change the active App.config file to the specified file.
            /// </summary>
            public ChangeAppConfig(string path)
            {
                AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", path);
                ResetConfigMechanism();
            }

            /// <summary>
            /// Reset the active App.config file to the default.
            /// </summary>
            public override void Dispose()
            {
                if (!_disposedValue)
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", _oldConfig);
                    ResetConfigMechanism();
                    _disposedValue = true;
                }
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// Force the App.config file to be reloaded.
            /// </summary>
            private static void ResetConfigMechanism()
            {
                // ReSharper disable PossibleNullReferenceException
                typeof(ConfigurationManager)
                    .GetField("s_initState", BindingFlags.NonPublic |
                                             BindingFlags.Static)
                    .SetValue(null, 0);

                typeof(ConfigurationManager)
                    .GetField("s_configSystem", BindingFlags.NonPublic |
                                                BindingFlags.Static)
                    .SetValue(null, null);

                // ReSharper disable once ReplaceWithSingleCallToFirst
                typeof(ConfigurationManager)
                    .Assembly.GetTypes()
                    .Where(x => x.FullName ==
                                "System.Configuration.ClientConfigPaths")
                    .First()
                    .GetField("s_current", BindingFlags.NonPublic |
                                           BindingFlags.Static)
                    .SetValue(null, null);
                // ReSharper restore PossibleNullReferenceException
            }
        }
    }
}