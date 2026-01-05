using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace ZoomNet.Analyzers
{
	/// <summary>
	/// Analyzer that generates a report of all methods returning paginated response types.
	/// This runs at build time and outputs a summary to the build output.
	/// </summary>
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class PaginationReportAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "ZOOM002";
		private const string Category = "API";

		private static readonly LocalizableString Title = "Pagination API Report";
		private static readonly LocalizableString MessageFormat = "Paginated API Methods Report: {0}";
		private static readonly LocalizableString Description = "Generates a report of all paginated API methods in the codebase.";

		private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
			DiagnosticId,
			Title,
			MessageFormat,
			Category,
			DiagnosticSeverity.Info,
			isEnabledByDefault: false, // Disabled by default, can be enabled via .editorconfig
			description: Description,
			customTags: new[] { "CompilationEnd" });

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

		public override void Initialize(AnalysisContext context)
		{
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
			context.EnableConcurrentExecution();

			// Register a compilation start action to collect paginated methods using semantic models from syntax tree analysis
			context.RegisterCompilationStartAction(compilationContext =>
			{
				var paginatedMethods = new List<PaginatedMethodInfo>();

				// Register a syntax node action for method declarations
				compilationContext.RegisterSyntaxNodeAction(syntaxContext =>
				{
					var methodDeclaration = (MethodDeclarationSyntax)syntaxContext.Node;
					var semanticModel = syntaxContext.SemanticModel;
					var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration, syntaxContext.CancellationToken);
					if (methodSymbol == null)
						return;

					// Check if it's a public API method
					if (methodSymbol.DeclaredAccessibility != Accessibility.Public)
						return;

					// Get return type info
					var returnType = methodSymbol.ReturnType;
					if (returnType is not INamedTypeSymbol namedReturnType)
						return;

					// Check if it's Task<PaginatedResponse...>
					if (!IsTaskType(namedReturnType))
						return;

					var taskArgument = namedReturnType.TypeArguments.FirstOrDefault();
					if (taskArgument is not INamedTypeSymbol taskArgumentType)
						return;

					var typeName = taskArgumentType.Name;
					if (typeName != "PaginatedResponseWithToken" &&
						typeName != "PaginatedResponseWithTokenAndDateRange" &&
						typeName != "PaginatedResponse")
						return;

					// Check namespace
					if (taskArgumentType.ContainingNamespace?.ToDisplayString() != "ZoomNet.Models")
						return;

					// Check if in interface
					var containingType = methodSymbol.ContainingType;
					if (containingType == null || containingType.TypeKind != TypeKind.Interface)
						return;

					// Skip obsolete methods
					if (HasObsoleteAttribute(methodSymbol))
						return;

					// Add to report
					var itemType = taskArgumentType.TypeArguments.FirstOrDefault()?.Name ?? "?";
					var hasHelper = CheckIfHelperExists(syntaxContext.Compilation, containingType.Name, methodSymbol.Name);

					paginatedMethods.Add(new PaginatedMethodInfo
					{
						InterfaceName = containingType.Name,
						MethodName = methodSymbol.Name,
						ReturnTypeName = typeName,
						ItemType = itemType,
						HasHelper = hasHelper,
						FilePath = methodDeclaration.SyntaxTree.FilePath
					});
				}, SyntaxKind.MethodDeclaration);

				// Register compilation end action to generate report
				compilationContext.RegisterCompilationEndAction(endContext =>
				{
					if (paginatedMethods.Count == 0)
						return;

					var report = GenerateReportText(paginatedMethods);

					var diagnostic = Diagnostic.Create(
						Rule,
						Location.None,
						report);

					endContext.ReportDiagnostic(diagnostic);
				});
			});
		}

		private static string GenerateReportText(List<PaginatedMethodInfo> methods)
		{
			var sb = new StringBuilder();
			sb.AppendLine();
			sb.AppendLine("┌─────────────────────────────────────────────────────────────────────────────────┐");
			sb.AppendLine("│                    ZOOM API PAGINATION METHODS REPORT                           │");
			sb.AppendLine("└─────────────────────────────────────────────────────────────────────────────────┘");
			sb.AppendLine();

			// Group by interface
			var grouped = methods.GroupBy(m => m.InterfaceName).OrderBy(g => g.Key);

			foreach (var group in grouped)
			{
				sb.AppendLine($"📦 {group.Key}");
				sb.AppendLine(new string('─', 80));

				foreach (var method in group.OrderBy(m => m.MethodName))
				{
					var helperStatus = method.HasHelper ? "✅" : "❌";
					sb.AppendLine($"  {helperStatus} {method.MethodName}");
					sb.AppendLine($"     Returns: {method.ReturnTypeName}<{method.ItemType}>");
					sb.AppendLine($"     Helper: {(method.HasHelper ? "Available" : "Missing")}");
					sb.AppendLine();
				}
			}

			// Summary
			var totalMethods = methods.Count;
			var methodsWithHelpers = methods.Count(m => m.HasHelper);
			var coverage = totalMethods > 0 ? (methodsWithHelpers * 100.0 / totalMethods) : 0;

			sb.AppendLine("┌─────────────────────────────────────────────────────────────────────────────────┐");
			sb.AppendLine("│                                   SUMMARY                                       │");
			sb.AppendLine("└─────────────────────────────────────────────────────────────────────────────────┘");
			sb.AppendLine($"  Total Paginated Methods: {totalMethods}");
			sb.AppendLine($"  With Helper Extensions: {methodsWithHelpers}");
			sb.AppendLine($"  Without Helpers: {totalMethods - methodsWithHelpers}");
			sb.AppendLine($"  Coverage: {coverage:F1}%");
			sb.AppendLine();

			return sb.ToString();
		}

		private static bool IsTaskType(INamedTypeSymbol type)
		{
			if (type.Name == "Task" && type.ContainingNamespace?.ToDisplayString() == "System.Threading.Tasks")
			{
				return type.TypeArguments.Length == 1;
			}

			return false;
		}

		private static bool HasObsoleteAttribute(IMethodSymbol method)
		{
			return method.GetAttributes().Any(attr =>
				attr.AttributeClass?.Name == "ObsoleteAttribute" &&
				attr.AttributeClass?.ContainingNamespace?.ToDisplayString() == "System");
		}

		private static bool CheckIfHelperExists(Compilation compilation, string interfaceName, string methodName)
		{
			// Look for PaginationHelperExtensions class
			var extensionsType = compilation.GetTypeByMetadataName("ZoomNet.PaginationHelperExtensions");
			if (extensionsType == null)
				return false;

			// Check if there's an extension method for this interface/method combination
			// Look for methods that extend the interface and have similar names
			var extensionMethods = extensionsType.GetMembers().OfType<IMethodSymbol>()
				.Where(m => m.IsExtensionMethod);

			foreach (var extensionMethod in extensionMethods)
			{
				// Get the first parameter (the 'this' parameter)
				var firstParam = extensionMethod.Parameters.FirstOrDefault();
				if (firstParam?.Type is INamedTypeSymbol paramType)
				{
					// Check if it extends the right interface
					if (paramType.Name == interfaceName)
					{
						// Check if method name is similar (could be GetAllAsync, GetAllRecordingsAsync, etc.)
						if (extensionMethod.Name.Contains("GetAll") || extensionMethod.Name == methodName)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		private class PaginatedMethodInfo
		{
			public string InterfaceName { get; set; }
			public string MethodName { get; set; }
			public string ReturnTypeName { get; set; }
			public string ItemType { get; set; }
			public bool HasHelper { get; set; }
			public string FilePath { get; set; }
		}
	}
}
