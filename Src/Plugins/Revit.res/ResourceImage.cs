using System;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Revit.res
{
    /// <summary>
    /// Gets the embedded resource image from the cbb.res assembly based on user provided file name with extension.
    /// Helper methods.
    /// </summary>
    public static class ResourceImage
    {
        #region public methods

        /// <summary>
        /// Gets the icon image from resource assembly.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static BitmapImage GetIcon(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Resource name cannot be null or empty.");
            }

            var availableResources = ResourceAssembly.GetAssembly()?.GetManifestResourceNames();

            if (availableResources == null || availableResources.Length == 0)
            {
                throw new ArgumentException("No resources found in the assembly.");
            }

            var resourceName = availableResources.FirstOrDefault(r => r.EndsWith(name, StringComparison.OrdinalIgnoreCase));

            if (resourceName == null)
            {
                var resourcesList = string.Join(Environment.NewLine, availableResources);
                throw new ArgumentException($"Resource '{name}' not found. Available resources:\n{resourcesList}");
            }

             var stream = ResourceAssembly.GetAssembly().GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new ArgumentException($"Resource stream for '{resourceName}' not found.");
            }

            var image = new BitmapImage();
            try
            {
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze(); // Make it thread-safe
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create BitmapImage for resource '{resourceName}'.", ex);
            }

            return image;
        }

        #endregion
    }
}
