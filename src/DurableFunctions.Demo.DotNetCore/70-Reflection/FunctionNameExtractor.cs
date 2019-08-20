using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.Azure.WebJobs;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Reflection
{
    public class FunctionNameExtractor
    {
        public static FunctionsOverview GetFunctionsOverview(Assembly assembly)
        {
            var functionMethods = assembly
                .GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(FunctionNameAttribute)).Any())
                .ToImmutableList();
            var clientMethods = GetMethodsWithAttributeOf<OrchestrationClientAttribute>(functionMethods).ToImmutableList(); 
            var orchestratorMethods = GetMethodsWithAttributeOf<OrchestrationTriggerAttribute>(functionMethods).ToImmutableList();
            var activityMethods = GetMethodsWithAttributeOf<ActivityTriggerAttribute>(functionMethods).ToImmutableList();
            var otherMethods = functionMethods.Except(clientMethods).Except(orchestratorMethods)
                .Except(activityMethods);

            return new FunctionsOverview
            {
                ActivityFunctions = GetFunctionNamesInAlphabeticalOrder(activityMethods),
                ClientFunctions = GetFunctionNamesInAlphabeticalOrder(clientMethods),
                OrchestratorFunctions = GetFunctionNamesInAlphabeticalOrder(orchestratorMethods),
                OtherFunctions = GetFunctionNamesInAlphabeticalOrder(otherMethods)
            };
        }

        private static IEnumerable<MethodInfo> GetMethodsWithAttributeOf<T>(IEnumerable<MethodInfo> methods)
        {
            return methods.Where(
                m => m.GetParameters().Any(
                    p=>p.GetCustomAttributes(typeof(T)).Any())
            );
        }

        private static IEnumerable<string> GetFunctionNamesInAlphabeticalOrder(IEnumerable<MethodInfo> methods)
        {
            var functionNames = methods
                .SelectMany(m => m.CustomAttributes.Where(ca => ca.AttributeType == typeof(FunctionNameAttribute)))
                .SelectMany(a => a.ConstructorArguments.Where(c => c.Value != null))
                .Select(c => c.Value.ToString()).OrderBy(_ => _);

            return functionNames;
        }
    }
}