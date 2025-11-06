using Microsoft.AspNetCore.SignalR;
using System.Reflection;

namespace Skill_Exchange.API.Helpers
{
    /// <summary>
    /// Utility class that uses reflection to discover all SignalR Hubs
    /// and list their public methods with parameter information.
    /// </summary>
    public static class SignalRHubExplorer
    {
        /// <summary>
        /// Scans the current assembly for all classes that inherit from <see cref="Hub"/>,
        /// and returns a structured list of their public instance methods.
        /// </summary>
        /// <returns>
        /// A collection of hub information, including hub name,
        /// method names, and parameter details.
        /// </returns>
        public static IEnumerable<object> GetHubMethods()
        {
            // Get all non-abstract types that inherit from Hub
            var hubTypes = Assembly.GetExecutingAssembly()
                                   .GetTypes()
                                   .Where(t => typeof(Hub).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var hub in hubTypes)
            {
                // Collect hub methods (only those declared in this hub)
                var hubMethods = hub.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                                    .Select(method => new
                                    {
                                        MethodName = method.Name,
                                        Parameters = method.GetParameters()
                                                           .Select(p => new
                                                           {
                                                               ParameterName = p.Name,
                                                               ParameterType = p.ParameterType.Name
                                                           })
                                    })
                                    .OrderBy(m => m.MethodName); // optional: sort alphabetically

                // Return structured info per hub
                yield return new
                {
                    HubName = hub.Name,
                    Methods = hubMethods
                };
            }
        }
    }
}
