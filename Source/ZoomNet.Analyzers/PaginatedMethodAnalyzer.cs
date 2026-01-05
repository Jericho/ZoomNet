using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace ZoomNet.Analyzers
{
	/// <summary>
	/// Analyzer that detects methods returning paginated response types without corresponding "EnumerateAllAsync" methods.
	/// </summary>
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class PaginatedMethodAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "ZOOM001";
		private const string Category = "API";

		private static readonly LocalizableString Title = "Paginated method without corresponding \"EnumerateAllAsync<T>\" method";
		private static readonly LocalizableString MessageFormat = "Method '{0}' returns '{1}' but may not have a corresponding \"EnumerateAllAsync<T>\" method";
		private static readonly LocalizableString Description = "Methods returning PaginatedResponseWithToken<T> or PaginatedResponseWithTokenAndDateRange<T> should have corresponding extension methods in PaginationHelperExtensions for easier consumption.";

		// RS2008: Enable analyzer release tracking for the analyzer project containing rule 'ZOOM001'
		private const string HelpLinkUri = "https://github.com/Jericho/ZoomNet/ZoomNet.Analyzers/blob/main/docs/ZOOM001.md"; // Update with actual documentation link

		private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
			DiagnosticId,
			Title,
			MessageFormat,
			Category,
			DiagnosticSeverity.Info,
			isEnabledByDefault: true,
			description: Description,
			helpLinkUri: HelpLinkUri);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

		public override void Initialize(AnalysisContext context)
		{
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
			context.EnableConcurrentExecution();

			// Register to analyze method declarations
			context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
		}

		private static void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
		{
			var methodDeclaration = (MethodDeclarationSyntax)context.Node;

			// Get the semantic model to access type information
			var methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration);
			if (methodSymbol == null)
				return;

			// Check if the method is public or internal (APIs we care about)
			if (methodSymbol.DeclaredAccessibility != Accessibility.Public &&
				methodSymbol.DeclaredAccessibility != Accessibility.Internal)
				return;

			// Get the return type
			var returnType = methodSymbol.ReturnType;
			if (returnType is not INamedTypeSymbol namedReturnType)
				return;

			// Check if it's a Task<T>
			if (!IsTaskType(namedReturnType))
				return;

			// Get the T from Task<T>
			var taskArgument = namedReturnType.TypeArguments.FirstOrDefault();
			if (taskArgument is not INamedTypeSymbol taskArgumentType)
				return;

			// Check if the T is PaginatedResponseWithToken<T> or PaginatedResponseWithTokenAndDateRange<T>
			var typeName = taskArgumentType.Name;
			if (typeName != "PaginatedResponseWithToken" && typeName != "PaginatedResponseWithTokenAndDateRange")
				return;

			// Check if the type is from ZoomNet.Models namespace
			if (taskArgumentType.ContainingNamespace?.ToDisplayString() != "ZoomNet.Models")
				return;

			// Check if method is in an interface (Resource interfaces like IMeetings, IUsers, etc.)
			var containingType = methodSymbol.ContainingType;
			if (containingType == null || containingType.TypeKind != TypeKind.Interface)
				return;

			// Skip if method is obsolete (old pagination methods)
			if (HasObsoleteAttribute(methodSymbol))
				return;

			// Create diagnostic
			var diagnostic = Diagnostic.Create(
				Rule,
				methodDeclaration.Identifier.GetLocation(),
				methodSymbol.Name,
				taskArgumentType.ToDisplayString());

			context.ReportDiagnostic(diagnostic);
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
	}
}
