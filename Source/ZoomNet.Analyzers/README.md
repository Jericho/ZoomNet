# ZoomNet Analyzers - Build-Time Pagination Detection

## Overview

The **ZoomNet.Analyzers** project provides Roslyn analyzers that scan your code at build-time to detect paginated API methods and ensure they have corresponding pagination helper extensions.

## Features

### 🔍 **ZOOM001: Paginated Method Without Helper Extension**

**Severity:** Info  
**Category:** API

Detects public methods in ZoomNet resource interfaces that return `PaginatedResponseWithToken<T>` or `PaginatedResponseWithTokenAndDateRange<T>` without corresponding helper extension methods.

**Example:**

```csharp
// This will trigger ZOOM001 if no helper exists
public interface IWebinars
{
    Task<PaginatedResponseWithToken<Registrant>> GetRegistrantsAsync(
        long webinarId, 
        int recordsPerPage = 30, 
        string pagingToken = null, 
        CancellationToken cancellationToken = default);
}
```

**Expected Helper:**

```csharp
public static class PaginationHelperExtensions
{
    public static IAsyncEnumerable<Registrant> GetAllRegistrantsAsync(
        this IWebinars webinars,
        long webinarId,
        int recordsPerPage = 300,
        CancellationToken cancellationToken = default)
    {
        return PaginationExtensions.GetAllRecordsAsync(
            () => webinars.GetRegistrantsAsync(webinarId, recordsPerPage, null, cancellationToken),
            token => webinars.GetRegistrantsAsync(webinarId, recordsPerPage, token, cancellationToken),
            cancellationToken);
    }
}
```

### 📊 **ZOOM002: Pagination API Report**

**Severity:** Info (Disabled by default)  
**Category:** API

Generates a comprehensive report of all paginated API methods in the codebase, including:
- Interface and method names
- Return types
- Item types
- Helper availability status
- Coverage percentage

**Sample Output:**

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                    ZOOM API PAGINATION METHODS REPORT                           │
└─────────────────────────────────────────────────────────────────────────────────┘

📦 IMeetings
────────────────────────────────────────────────────────────────────────────────
  ✅ GetAllAsync
     Returns: PaginatedResponseWithToken<MeetingSummary>
     Helper: Available

  ✅ GetRegistrantsAsync
     Returns: PaginatedResponseWithToken<Registrant>
     Helper: Available

📦 IWebinars
────────────────────────────────────────────────────────────────────────────────
  ✅ GetAllAsync
     Returns: PaginatedResponseWithToken<WebinarSummary>
     Helper: Available

  ❌ GetPanelistsAsync
     Returns: PaginatedResponseWithToken<Panelist>
     Helper: Missing

📦 ICloudRecordings
────────────────────────────────────────────────────────────────────────────────
  ✅ GetRecordingsForUserAsync
     Returns: PaginatedResponseWithTokenAndDateRange<Recording>
     Helper: Available

┌─────────────────────────────────────────────────────────────────────────────────┐
│                                   SUMMARY                                       │
└─────────────────────────────────────────────────────────────────────────────────┘
  Total Paginated Methods: 12
  With Helper Extensions: 11
  Without Helpers: 1
  Coverage: 91.7%
```

## Installation

### 1. Add Analyzer Project Reference

In `ZoomNet.csproj`, add:

```xml
<ItemGroup>
  <ProjectReference Include="..\ZoomNet.Analyzers\ZoomNet.Analyzers.csproj" 
                    OutputItemType="Analyzer" 
                    ReferenceOutputAssembly="false" />
</ItemGroup>
```

### 2. Enable Report Generator (Optional)

Create or edit `.editorconfig` in the solution root:

```ini
# Enable pagination report analyzer
[*.cs]
dotnet_diagnostic.ZOOM002.severity = suggestion
```

## Configuration

### Suppressing Warnings

If you want to suppress ZOOM001 for specific methods, use:

```csharp
#pragma warning disable ZOOM001
public interface IWebinars
{
    Task<PaginatedResponseWithToken<Registrant>> GetRegistrantsAsync(...);
}
#pragma warning restore ZOOM001
```

Or in `.editorconfig`:

```ini
# Disable ZOOM001 globally
[*.cs]
dotnet_diagnostic.ZOOM001.severity = none
```

### Customizing Severity

```ini
[*.cs]
# Change to warning to make it more visible
dotnet_diagnostic.ZOOM001.severity = warning

# Enable report (default is disabled)
dotnet_diagnostic.ZOOM002.severity = suggestion
```

## How It Works

### Detection Logic

The analyzer:

1. **Scans all method declarations** in syntax trees
2. **Filters for public/internal methods** in interfaces
3. **Checks return type** for `Task<PaginatedResponseWithToken<T>>` or `Task<PaginatedResponseWithTokenAndDateRange<T>>`
4. **Verifies namespace** is `ZoomNet.Models`
5. **Excludes obsolete methods** (old pagination APIs)
6. **Checks for helper extensions** in `PaginationHelperExtensions`

### Helper Detection

The analyzer looks for extension methods in `ZoomNet.PaginationHelperExtensions` that:
- Extend the same interface
- Have similar names (e.g., `GetAllAsync`, `GetAllRegistrantsAsync`)

## Build Integration

### MSBuild Output

When building with the analyzer enabled, you'll see:

```
Building...
  ZoomNet -> D:\_build\ZoomNet\Source\ZoomNet\bin\Debug\net10.0\ZoomNet.dll
  
Info ZOOM001: Method 'GetPanelistsAsync' returns 'PaginatedResponseWithToken<Panelist>' 
              but may not have a corresponding pagination helper extension method
              (ZoomNet\Resources\IWebinars.cs, Line 42)

Build succeeded.
    1 Warning(s)
    0 Error(s)
```

### CI/CD Integration

In your build pipeline, you can:

#### 1. Treat as Warnings

```xml
<!-- In .csproj -->
<PropertyGroup>
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  <WarningsAsErrors>ZOOM001</WarningsAsErrors>
</PropertyGroup>
```

#### 2. Generate Reports

```bash
# Enable report in CI
dotnet build /p:ZOOM002=suggestion

# Save build log
dotnet build > build-report.txt
```

#### 3. Parse and Fail Build

```powershell
# Check for missing helpers
$buildOutput = dotnet build 2>&1
$missingHelpers = $buildOutput | Select-String "ZOOM001"

if ($missingHelpers.Count -gt 0) {
    Write-Error "Found $($missingHelpers.Count) paginated methods without helpers"
    exit 1
}
```

## Development Workflow

### When Adding New Paginated Endpoints

1. **Add the API method** to the resource interface:
   ```csharp
   Task<PaginatedResponseWithToken<Item>> GetItemsAsync(...);
   ```

2. **Build the project** - The analyzer will warn you:
   ```
   Info ZOOM001: Method 'GetItemsAsync' may not have a helper
   ```

3. **Add the helper extension**:
   ```csharp
   public static IAsyncEnumerable<Item> GetAllItemsAsync(
       this IResource resource, ...) { }
   ```

4. **Rebuild** - Warning should disappear!

### Running Tests

```bash
# Test the analyzers
cd ZoomNet.Analyzers.Tests
dotnet test

# Test against actual ZoomNet code
cd ../ZoomNet
dotnet build
```

## Testing

The analyzer comes with comprehensive unit tests:

```csharp
[Fact]
public async Task AnalyzerDetectsPaginatedResponseWithToken()
{
    // Test code with paginated method
    var test = @"
        public interface IMeetings
        {
            Task<PaginatedResponseWithToken<string>> GetAllAsync(...);
        }
    ";
    
    // Verify diagnostic is produced
    await VerifyAnalyzer(test, expectedDiagnostic);
}
```

**Test Coverage:**
- ✅ Detects `PaginatedResponseWithToken<T>`
- ✅ Detects `PaginatedResponseWithTokenAndDateRange<T>`
- ✅ Ignores obsolete methods
- ✅ Ignores private methods
- ✅ Ignores non-interface methods
- ✅ Ignores non-Task return types

## Benefits

### For Developers

- 🔍 **Automatic Detection** - No need to manually track paginated methods
- ✅ **Consistency** - Ensures all paginated APIs have helpers
- 📊 **Visibility** - Clear report of coverage
- 🚀 **Faster Development** - Immediate feedback during build

### For Code Reviews

- 📋 Easy to verify all new endpoints have helpers
- 📈 Track pagination helper coverage over time
- ✨ Maintain API consistency

### For CI/CD

- 🤖 Automated checks in build pipeline
- 📉 Prevent regressions
- 📊 Generate coverage reports

## Troubleshooting

### Analyzer Not Running

**Problem:** No diagnostics appearing

**Solutions:**
1. Check project reference includes `OutputItemType="Analyzer"`
2. Restart Visual Studio / VS Code
3. Clean and rebuild solution
4. Check `.editorconfig` severity settings

### False Positives

**Problem:** Analyzer flags methods that have helpers

**Solutions:**
1. Ensure helper is in `ZoomNet.PaginationHelperExtensions` class
2. Check method name pattern (should contain "GetAll")
3. Verify extension method extends correct interface
4. Add `#pragma warning disable ZOOM001` if intentional

### Performance Issues

**Problem:** Build is slower

**Solutions:**
- Disable ZOOM002 (report generator) for regular builds
- Only enable reports in CI/CD or on-demand
- Analyzer is lightweight, but report generation can be verbose

## Future Enhancements

Possible improvements:

1. **Code Fixes** - Automatic generation of helper methods
2. **Smart Suggestions** - Suggest helper method signatures
3. **Coverage Trends** - Track coverage over time
4. **Custom Rules** - Configurable naming patterns
5. **IDE Integration** - Quick actions in Visual Studio

## Related Documentation

- [Pagination Helper Guide](../PAGINATION_HELPER.md)
- [Pagination Implementation Summary](../PAGINATION_IMPLEMENTATION_SUMMARY.md)
- [Before & After Comparison](../PAGINATION_BEFORE_AFTER.md)

## Support

For issues or questions:
- Open an issue on GitHub
- Check existing analyzer tests for examples
- Review Roslyn analyzer documentation

---

**Note:** This analyzer runs at build-time and does not affect runtime performance. It's a development tool to help maintain code quality and consistency.
