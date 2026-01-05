# Quick Start: Integrating the Build-Time Analyzer

## 🚀 5-Minute Setup Guide

### Step 1: Add Analyzer to ZoomNet Project

Edit `ZoomNet/ZoomNet.csproj` and add this inside an `<ItemGroup>`:

```xml
<ItemGroup>
  <!-- Build-time analyzer for pagination helpers -->
  <ProjectReference Include="..\ZoomNet.Analyzers\ZoomNet.Analyzers.csproj" 
                    OutputItemType="Analyzer" 
                    ReferenceOutputAssembly="false" />
</ItemGroup>
```

### Step 2: Configure (Optional)

Create `.editorconfig` in the solution root or edit existing one:

```ini
# ZoomNet Analyzer Configuration
[*.cs]

# ZOOM001: Notify about paginated methods without helpers
dotnet_diagnostic.ZOOM001.severity = suggestion

# ZOOM002: Generate report (disabled by default)
dotnet_diagnostic.ZOOM002.severity = none
```

### Step 3: Build and Verify

```bash
cd Source
dotnet build
```

**Expected Output:**

```
Building...
  ZoomNet -> bin\Debug\net10.0\ZoomNet.dll

Info ZOOM001: Method 'GetSomeMethodAsync' returns 'PaginatedResponseWithToken<Item>' 
              but may not have a corresponding pagination helper extension method
              Location: ISomeResource.cs, Line 123

Build succeeded.
```

## 📊 Viewing the Report

To see a comprehensive report of all paginated methods:

```bash
# Enable report temporarily
dotnet build /p:ZOOM002=suggestion

# Or permanently in .editorconfig
# dotnet_diagnostic.ZOOM002.severity = suggestion
```

**Report Output:**

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                    ZOOM API PAGINATION METHODS REPORT                           │
└─────────────────────────────────────────────────────────────────────────────────┘

📦 IMeetings
────────────────────────────────────────────────────────────────────────────────
  ✅ GetAllAsync
     Returns: PaginatedResponseWithToken<MeetingSummary>
     Helper: Available

📦 IWebinars
────────────────────────────────────────────────────────────────────────────────
  ✅ GetAllAsync
     Returns: PaginatedResponseWithToken<WebinarSummary>
     Helper: Available

  ❌ GetPanelistsAsync
     Returns: PaginatedResponseWithToken<Panelist>
     Helper: Missing

┌─────────────────────────────────────────────────────────────────────────────────┐
│                                   SUMMARY                                       │
└─────────────────────────────────────────────────────────────────────────────────┘
  Total Paginated Methods: 15
  With Helper Extensions: 14
  Without Helpers: 1
  Coverage: 93.3%
```

## 🎯 Development Workflow

### When You See a Warning

**Warning:**
```
Info ZOOM001: Method 'GetItemsAsync' may not have a helper
```

**Action: Add Helper Extension**

In `ZoomNet/Extensions/PaginationHelperExtensions.cs`:

```csharp
/// <summary>
/// Enumerates all items across all pages.
/// </summary>
public static IAsyncEnumerable<Item> GetAllItemsAsync(
    this IResource resource,
    string parameter,
    int recordsPerPage = 300,
    CancellationToken cancellationToken = default)
{
    return Utilities.PaginationExtensions.GetAllRecordsAsync(
        () => resource.GetItemsAsync(parameter, recordsPerPage, null, cancellationToken),
        pagingToken => resource.GetItemsAsync(parameter, recordsPerPage, pagingToken, cancellationToken),
        cancellationToken);
}
```

**Rebuild** → Warning should disappear ✅

## 🔧 Configuration Options

### Make it More Visible (Warning Level)

```ini
[*.cs]
dotnet_diagnostic.ZOOM001.severity = warning
```

### Make it Break the Build (Error Level)

```ini
[*.cs]
dotnet_diagnostic.ZOOM001.severity = error
```

Or in `ZoomNet.csproj`:

```xml
<PropertyGroup>
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  <WarningsAsErrors>ZOOM001</WarningsAsErrors>
</PropertyGroup>
```

### Disable Temporarily

```csharp
#pragma warning disable ZOOM001
Task<PaginatedResponseWithToken<T>> MethodWithoutHelper(...);
#pragma warning restore ZOOM001
```

## 🧪 Testing the Analyzer

```bash
cd ZoomNet.Analyzers.Tests
dotnet test
```

**Expected:**
```
Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:     6, Skipped:     0, Total:     6
```

## 📈 CI/CD Integration

### GitHub Actions

```yaml
name: Build

on: [push, pull_request]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '10.0.x'
      
      - name: Build with analyzers
        run: dotnet build --configuration Release
      
      - name: Generate pagination report
        run: dotnet build /p:ZOOM002=suggestion > pagination-report.txt
      
      - name: Upload report
        uses: actions/upload-artifact@v3
        with:
          name: pagination-report
          path: pagination-report.txt
```

### Azure DevOps

```yaml
steps:
- task: DotNetCoreCLI@2
  displayName: 'Build with analyzers'
  inputs:
    command: 'build'
    arguments: '--configuration Release'

- task: PowerShell@2
  displayName: 'Check for missing helpers'
  inputs:
    targetType: 'inline'
    script: |
      $buildOutput = dotnet build 2>&1
      $warnings = $buildOutput | Select-String "ZOOM001"
      
      if ($warnings.Count -gt 0) {
        Write-Host "##vso[task.logissue type=warning]Found $($warnings.Count) methods without pagination helpers"
        foreach ($warning in $warnings) {
          Write-Host "##vso[task.logissue type=warning]$warning"
        }
      }
```

## 🎓 What the Analyzer Does

### ✅ Detects

- Public interface methods
- Returning `Task<PaginatedResponseWithToken<T>>`
- Returning `Task<PaginatedResponseWithTokenAndDateRange<T>>`
- In `ZoomNet.Models` namespace

### ❌ Ignores

- Obsolete methods (`[Obsolete]` attribute)
- Private/protected methods
- Non-interface methods
- Non-Task return types
- Methods outside `ZoomNet.Models`

### 🔍 Verifies

- Helper extension exists in `PaginationHelperExtensions`
- Extension method extends correct interface
- Method naming follows conventions

## 💡 Tips

### 1. Start with Info Level

```ini
dotnet_diagnostic.ZOOM001.severity = suggestion
```

Review all warnings, then gradually increase severity.

### 2. Use Reports for Planning

```bash
dotnet build /p:ZOOM002=suggestion > report.txt
```

Identify gaps and prioritize.

### 3. Suppress Intentionally

For methods that shouldn't have helpers:

```csharp
#pragma warning disable ZOOM001 // Intentionally no helper - internal use only
Task<PaginatedResponseWithToken<T>> InternalMethod(...);
#pragma warning restore ZOOM001
```

### 4. Track Progress

Run report periodically:

```bash
# Week 1: 70% coverage
# Week 2: 85% coverage
# Week 3: 100% coverage ✅
```

## 🐛 Troubleshooting

### Analyzer Not Running

**Issue:** No diagnostics appearing

**Fix:**
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

### False Positives

**Issue:** Warning for method with helper

**Check:**
1. Helper is in `PaginationHelperExtensions` class
2. Method extends correct interface
3. Method name contains "GetAll"

**Suppress if needed:**
```csharp
#pragma warning disable ZOOM001
```

### Build Performance

**Issue:** Build is slow

**Fix:** Disable report generator:
```ini
dotnet_diagnostic.ZOOM002.severity = none
```

## 📚 Resources

- **Full Documentation:** [ZoomNet.Analyzers/README.md](../ZoomNet.Analyzers/README.md)
- **Implementation Guide:** [ANALYZER_IMPLEMENTATION_SUMMARY.md](../ANALYZER_IMPLEMENTATION_SUMMARY.md)
- **Pagination Helpers:** [PAGINATION_HELPER.md](../PAGINATION_HELPER.md)

## ✅ Checklist

Before committing:

- [ ] Added analyzer reference to `ZoomNet.csproj`
- [ ] Configured `.editorconfig` (if desired)
- [ ] Built project successfully
- [ ] Reviewed warnings
- [ ] Added missing helpers (or suppressed intentionally)
- [ ] Re-built to verify warnings are resolved
- [ ] Tested analyzer tests pass
- [ ] Updated CI/CD pipeline (optional)

## 🎉 Done!

Your ZoomNet project now has build-time analysis to ensure:

✅ All paginated methods have helpers  
✅ API consistency is maintained  
✅ Coverage is tracked automatically  
✅ Developers get immediate feedback  

**Happy coding!** 🚀
