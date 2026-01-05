# Pagination Helper - Implementation Guide

## Overview

The ZoomNet library now includes powerful pagination helpers that leverage C#'s `IAsyncEnumerable<T>` to eliminate manual pagination token handling. This feature dramatically simplifies consuming paginated Zoom API responses.

## Problem Statement

### Before Pagination Helpers ❌

Manual pagination requires boilerplate code that's error-prone and repetitive:

```csharp
var allMeetings = new List<MeetingSummary>();
string nextPageToken = null;

do
{
    var response = await client.Meetings.GetAllAsync(
        userId: "user@example.com",
        recordsPerPage: 100,
        pagingToken: nextPageToken,
        cancellationToken: cancellationToken);
    
    allMeetings.AddRange(response.Records);
    nextPageToken = response.NextPageToken;
    
} while (!string.IsNullOrEmpty(nextPageToken));

// Now process allMeetings
foreach (var meeting in allMeetings)
{
    Console.WriteLine(meeting.Topic);
}
```

**Issues:**
- Manual loop management
- Tracking pagination tokens
- Memory consumption (loading all records)
- Code duplication across different endpoints

### After Pagination Helpers ✅

Clean, idiomatic C# with automatic pagination:

```csharp
await foreach (var meeting in client.Meetings.GetAllAsync("user@example.com"))
{
    Console.WriteLine(meeting.Topic);
}
```

**Benefits:**
- ✅ No manual token management
- ✅ Streaming results (low memory footprint)
- ✅ Consistent API across all resources
- ✅ Cancellation support built-in
- ✅ Lazy evaluation

## Implementation Details

### Core Components

#### 1. **PaginationExtensions** (`ZoomNet/Utilities/PaginationExtensions.cs`)

Provides low-level pagination utilities:

```csharp
public static class PaginationExtensions
{
    // Stream all records from all pages
    public static IAsyncEnumerable<T> GetAllRecordsAsync<T>(
        Func<Task<PaginatedResponseWithToken<T>>> getFirstPage,
        Func<string, Task<PaginatedResponseWithToken<T>>> getNextPage,
        CancellationToken cancellationToken = default);
    
    // Stream all pages (if you need page metadata)
    public static IAsyncEnumerable<PaginatedResponseWithToken<T>> GetAllPagesAsync<T>(
        Func<Task<PaginatedResponseWithToken<T>>> getFirstPage,
        Func<string, Task<PaginatedResponseWithToken<T>>> getNextPage,
        CancellationToken cancellationToken = default);
}
```

#### 2. **PaginationHelperExtensions** (`ZoomNet/Extensions/PaginationHelperExtensions.cs`)

Convenient extension methods for specific resources:

- `IMeetings.GetAllAsync()` - Stream all meetings
- `IWebinars.GetAllAsync()` - Stream all webinars
- `IWebinars.GetAllRegistrantsAsync()` - Stream all webinar registrants
- `IUsers.GetAllAsync()` - Stream all users
- `ICloudRecordings.GetAllRecordingsAsync()` - Stream all recordings
- `IReports.GetAllMeetingParticipantsAsync()` - Stream all participants
- `IReports.GetAllMeetingsAsync()` - Stream all meetings from reports

## Usage Examples

### Example 1: Stream All Meetings

```csharp
// Simple enumeration
await foreach (var meeting in client.Meetings.GetAllAsync("user@example.com"))
{
    Console.WriteLine($"{meeting.Topic} - {meeting.StartTime}");
}
```

### Example 2: Filter and Process

```csharp
await foreach (var meeting in client.Meetings.GetAllAsync("user@example.com"))
{
    if (meeting.StartTime > DateTime.UtcNow.AddDays(-7))
    {
        Console.WriteLine($"Recent meeting: {meeting.Topic}");
    }
}
```

### Example 3: With LINQ

```csharp
using System.Linq;

var recentMeetings = client.Meetings
    .GetAllAsync("user@example.com")
    .Where(m => m.StartTime > DateTime.UtcNow.AddDays(-30))
    .OrderByDescending(m => m.StartTime);

await foreach (var meeting in recentMeetings)
{
    Console.WriteLine(meeting.Topic);
}
```

### Example 4: Collect Into List (When Needed)

```csharp
var meetings = new List<MeetingSummary>();

await foreach (var meeting in client.Meetings.GetAllAsync("user@example.com"))
{
    meetings.Add(meeting);
}

// Or using ToListAsync (requires System.Linq.Async)
var meetingsList = await client.Meetings
    .GetAllAsync("user@example.com")
    .ToListAsync();
```

### Example 5: Stream All Users

```csharp
await foreach (var user in client.Users.GetAllAsync(
    status: UserStatus.Active,
    recordsPerPage: 300))
{
    Console.WriteLine($"{user.FirstName} {user.LastName} - {user.Email}");
}
```

### Example 6: Stream Cloud Recordings

```csharp
var from = DateTime.UtcNow.AddMonths(-1);
var to = DateTime.UtcNow;

await foreach (var recording in client.CloudRecordings.GetAllRecordingsAsync(
    userId: "user@example.com",
    from: from,
    to: to,
    recordsPerPage: 300))
{
    Console.WriteLine($"Recording: {recording.Topic} ({recording.TotalSize} bytes)");
}
```

### Example 7: With Cancellation

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

try
{
    await foreach (var meeting in client.Meetings.GetAllAsync("user@example.com", cancellationToken: cts.Token))
    {
        await ProcessMeetingAsync(meeting);
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation was cancelled");
}
```

### Example 8: Stream Pages (Access Metadata)

```csharp
await foreach (var page in PaginationExtensions.GetAllPagesAsync(
    () => client.Meetings.GetAllAsync(userId, recordsPerPage: 100),
    token => client.Meetings.GetAllAsync(userId, recordsPerPage: 100, pagingToken: token)))
{
    Console.WriteLine($"Page has {page.Records.Length} records");
    Console.WriteLine($"Total records: {page.TotalRecords}");
    Console.WriteLine($"More pages available: {page.MoreRecordsAvailable}");
    
    foreach (var meeting in page.Records)
    {
        Console.WriteLine($"  - {meeting.Topic}");
    }
}
```

### Example 9: Process with Progress Reporting

```csharp
var count = 0;
await foreach (var meeting in client.Meetings.GetAllAsync("user@example.com"))
{
    count++;
    if (count % 100 == 0)
    {
        Console.WriteLine($"Processed {count} meetings so far...");
    }
    
    await ProcessMeetingAsync(meeting);
}

Console.WriteLine($"Completed processing {count} meetings");
```

### Example 10: Parallel Processing with Batching

```csharp
var batch = new List<MeetingSummary>();
const int batchSize = 50;

await foreach (var meeting in client.Meetings.GetAllAsync("user@example.com"))
{
    batch.Add(meeting);
    
    if (batch.Count >= batchSize)
    {
        await Task.WhenAll(batch.Select(m => ProcessMeetingAsync(m)));
        batch.Clear();
    }
}

// Process remaining items
if (batch.Any())
{
    await Task.WhenAll(batch.Select(m => ProcessMeetingAsync(m)));
}
```

## Advanced Patterns

### Pattern 1: Custom Pagination for Other Endpoints

```csharp
// You can use the low-level PaginationExtensions for any paginated endpoint
await foreach (var registrant in PaginationExtensions.GetAllRecordsAsync(
    () => client.Webinars.GetRegistrantsAsync(webinarId, RegistrantStatus.Approved, recordsPerPage: 300),
    token => client.Webinars.GetRegistrantsAsync(webinarId, RegistrantStatus.Approved, recordsPerPage: 300, pagingToken: token),
    cancellationToken))
{
    Console.WriteLine($"Registrant: {registrant.Email}");
}
```

### Pattern 2: Conditional Processing

```csharp
await foreach (var user in client.Users.GetAllAsync())
{
    if (user.Status == UserStatus.Active && user.Type == UserType.Licensed)
    {
        await AssignLicenseAsync(user);
    }
    else if (user.Status == UserStatus.Inactive)
    {
        Console.WriteLine($"Inactive user: {user.Email}");
    }
}
```

### Pattern 3: Error Handling

```csharp
try
{
    await foreach (var meeting in client.Meetings.GetAllAsync("user@example.com"))
    {
        try
        {
            await ProcessMeetingAsync(meeting);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to process meeting {meeting.Id}: {ex.Message}");
            // Continue processing other meetings
        }
    }
}
catch (ZoomException ex)
{
    Console.WriteLine($"API Error: {ex.Message}");
}
```

## Performance Considerations

### Memory Efficiency ✅

**Streaming (Recommended):**
```csharp
await foreach (var meeting in client.Meetings.GetAllAsync("user@example.com"))
{
    // Process one at a time - low memory footprint
    await ProcessMeetingAsync(meeting);
}
```

**Loading All (Use When Necessary):**
```csharp
var allMeetings = new List<MeetingSummary>();
await foreach (var meeting in client.Meetings.GetAllAsync("user@example.com"))
{
    allMeetings.Add(meeting);
}
// Now all meetings are in memory
```

### Optimal Page Size

```csharp
// Zoom's maximum is typically 300 records per page
await foreach (var user in client.Users.GetAllAsync(recordsPerPage: 300))
{
    // Fewer API calls = better performance
}
```

### Rate Limiting

The pagination helpers work seamlessly with ZoomNet's built-in rate limiting:

```csharp
// Automatic backoff on HTTP 429 (Too Many Requests)
await foreach (var meeting in client.Meetings.GetAllAsync("user@example.com"))
{
    // ZoomNet handles rate limiting automatically
}
```

## Comparison Table

| Feature | Manual Pagination | Pagination Helpers |
|---------|------------------|-------------------|
| Code Lines | ~15 lines | 1-3 lines |
| Token Management | Manual | Automatic |
| Memory Usage | High (loads all) | Low (streaming) |
| Cancellation Support | Manual | Built-in |
| Error Handling | Manual | Consistent |
| Readability | Complex | Simple |
| Reusability | Low | High |

## Migration Guide

### Before (Manual Pagination)

```csharp
public async Task<List<MeetingSummary>> GetAllMeetingsAsync(string userId)
{
    var meetings = new List<MeetingSummary>();
    string nextToken = null;
    
    do
    {
        var response = await _client.Meetings.GetAllAsync(
            userId, 
            recordsPerPage: 100,
            pagingToken: nextToken);
        
        meetings.AddRange(response.Records);
        nextToken = response.NextPageToken;
    }
    while (!string.IsNullOrEmpty(nextToken));
    
    return meetings;
}
```

### After (Pagination Helper)

```csharp
public async IAsyncEnumerable<MeetingSummary> GetAllMeetingsAsync(
    string userId,
    [EnumeratorCancellation] CancellationToken cancellationToken = default)
{
    await foreach (var meeting in _client.Meetings.GetAllAsync(
        userId,
        recordsPerPage: 100,
        cancellationToken: cancellationToken))
    {
        yield return meeting;
    }
}

// Or even simpler - just return the IAsyncEnumerable directly:
public IAsyncEnumerable<MeetingSummary> GetAllMeetingsAsync(string userId)
{
    return _client.Meetings.GetAllAsync(userId, recordsPerPage: 100);
}
```

## Best Practices

### ✅ DO

1. **Use streaming for large datasets**
   ```csharp
   await foreach (var item in collection.GetAllAsync()) { }
   ```

2. **Specify optimal page size**
   ```csharp
   .GetAllAsync(recordsPerPage: 300)  // Maximum supported
   ```

3. **Pass cancellation tokens**
   ```csharp
   .GetAllAsync(cancellationToken: ct)
   ```

4. **Handle errors gracefully**
   ```csharp
   try { await foreach (...) } catch (ZoomException ex) { }
   ```

### ❌ DON'T

1. **Don't load all records unnecessarily**
   ```csharp
   // Avoid if you don't need everything in memory
   var all = await collection.GetAllAsync().ToListAsync();
   ```

2. **Don't ignore cancellation**
   ```csharp
   // Bad: No cancellation support
   await foreach (var item in collection.GetAllAsync()) { }
   ```

3. **Don't use small page sizes**
   ```csharp
   // Inefficient: Too many API calls
   .GetAllAsync(recordsPerPage: 10)
   ```

## Supported Resources

| Resource | Extension Method | Return Type |
|----------|-----------------|-------------|
| **Meetings** | `GetAllAsync()` | `IAsyncEnumerable<MeetingSummary>` |
| **Webinars** | `GetAllAsync()` | `IAsyncEnumerable<WebinarSummary>` |
| **Webinars** | `GetAllRegistrantsAsync()` | `IAsyncEnumerable<Registrant>` |
| **Users** | `GetAllAsync()` | `IAsyncEnumerable<User>` |
| **CloudRecordings** | `GetAllRecordingsAsync()` | `IAsyncEnumerable<Recording>` |
| **Reports** | `GetAllMeetingParticipantsAsync()` | `IAsyncEnumerable<ReportParticipant>` |
| **Reports** | `GetAllMeetingsAsync()` | `IAsyncEnumerable<PastMeeting>` |

## Framework Support

- ✅ .NET 10
- ✅ .NET Standard 2.1
- ✅ .NET Framework 4.8

All frameworks support `IAsyncEnumerable<T>` through appropriate packages.

## Conclusion

The pagination helper feature transforms how you work with paginated Zoom API responses:

**Before:** 15+ lines of boilerplate code per endpoint  
**After:** 1-3 lines of clean, maintainable code

**Memory:** Loads all records upfront  
**Now:** Streams records on-demand

**Tokens:** Manual tracking required  
**Now:** Automatic handling

This feature makes your code more readable, maintainable, and efficient! 🚀
