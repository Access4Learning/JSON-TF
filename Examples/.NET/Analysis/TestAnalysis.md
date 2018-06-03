# Test Analysis for .NET JSON Serialization

## Description

This analysis has been produced in order to discover how objects are serialized/deserialized within a .NET environment.

Test scripts can be found at: [GitHub](https://github.com)

## Disclaimer

JSON functionality used within this test analysis is provided by the [Newtonsoft JSON.NET library v11.0.2](https://www.nuget.org/packages/Newtonsoft.Json/)

## Entities

Basic entities that are used to test JSON serialization/deserialization.  

### Student

```csharp
public class Student
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string MiddleName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    public bool IsActive { get; set; }

    [Required]
    public int YearLevel { get; set; }

    public int? NextYearLevel { get; set; }

    [Required]
    public School CurrentSchool { get; set; }

    public List<School> PreviousSchools { get; set; }

    public int[] PreviousYearLevels { get; set; }

}
```

### School

```csharp
public class School
{
    [Required]
    public string Name { get; set; }
}
```

## Analysis

### Serialization Tests

#### Test 1: Test for basic JSON serialization (subset of elements)

##### Setup

```csharp
FirstName = "John",
LastName = "Smith"
```

##### Result

```json
{
  "FirstName": "John",
  "LastName": "Smith",
  "MiddleName": null,
  "DateOfBirth": "0001-01-01T00:00:00",
  "IsActive": false,
  "YearLevel": 0,
  "NextYearLevel": null,
  "CurrentSchool": null,
  "PreviousSchools": null,
  "PreviousYearLevels": null
}
```

#### Test 2: Test for basic JSON serialization (all elements)

##### Setup

```csharp
FirstName = "John",
LastName = "Smith",
MiddleName = "Fred",
DateOfBirth = new DateTime(2010,1,1),
IsActive = true,
YearLevel = 3,
NextYearLevel = 4,
PreviousYearLevels = new int[] { 1, 2 },
CurrentSchool = new School() { Name = "Test School 1" },
PreviousSchools = new List<School>(new School[] { new School() { Name = "Test School Z" } }),
```

##### Result

```json
{
  "FirstName": "John",
  "LastName": "Smith",
  "MiddleName": "Fred",
  "DateOfBirth": "2010-01-01T00:00:00",
  "IsActive": true,
  "YearLevel": 3,
  "NextYearLevel": 4,
  "CurrentSchool": {
    "Name": "Test School 1"
  },
  "PreviousSchools": [
    {
      "Name": "Test School Z"
    }
  ],
  "PreviousYearLevels": [
    1,
    2
  ]
}
```

#### Test 3: Test for serialization of empty complex and list elements

##### Setup

```csharp
FirstName = "John",
LastName = "Smith",
MiddleName = "Fred",
DateOfBirth = new DateTime(2010,1,1),
IsActive = true,
YearLevel = 3,
NextYearLevel = 4,
PreviousYearLevels = new int[] { },
CurrentSchool = new School() { },
PreviousSchools = new List<School>(),
```

##### Result

```json
{
  "FirstName": "John",
  "LastName": "Smith",
  "MiddleName": "Fred",
  "DateOfBirth": "2010-01-01T00:00:00",
  "IsActive": true,
  "YearLevel": 3,
  "NextYearLevel": 4,
  "CurrentSchool": {
    "Name": null
  },
  "PreviousSchools": [],
  "PreviousYearLevels": []
}
```

### Deserialization Tests

#### Test 4: Test for basic JSON deserialization (subset of elements)

##### Setup

```json
{
  "FirstName": "John",
  "LastName": "Smith"
}
```

##### Result

```text
FirstName = "John",
LastName = "Smith",
MiddleName = null,
DateOfBirth = {1/01/0001 12:00:00 AM},
IsActive = false,
YearLevel = 0,
NextYearLevel = null,
PreviousYearLevels = null,
CurrentSchool = null,
PreviousSchools = null
```

#### Test 5: Test for basic JSON deserialization (all elements)

##### Setup

```json
{
  "FirstName": "John",
  "LastName": "Smith",
  "MiddleName": "Fred",
  "DateOfBirth": "2010-01-01T00:00:00",
  "IsActive": true,
  "YearLevel": 3,
  "NextYearLevel": 4,
  "CurrentSchool": {
    "Name": "Test School 1"
  },
  "PreviousSchools": [
    {
      "Name": "Test School Z"
    }
  ],
  "PreviousYearLevels": [
    1,
    2
  ]
}
```

##### Result

```text
FirstName = "John",
LastName = "Smith",
MiddleName = "Fred",
DateOfBirth = {1/01/2010 12:00:00 AM},
IsActive = true,
YearLevel = 3,
NextYearLevel = 4,
PreviousYearLevels = { 1, 2 },
CurrentSchool = { Name = "Test School 1" },
PreviousSchools = [ { Name = "Test School Z" } ],
```

#### Test 6: Test for serialization of empty complex and list elements

##### Setup

```json
{
  "FirstName": "John",
  "LastName": "Smith",
  "MiddleName": "Fred",
  "DateOfBirth": "2010-01-01T00:00:00",
  "IsActive": true,
  "YearLevel": 3,
  "NextYearLevel": 4,
  "CurrentSchool": {
    "Name": null
  },
  "PreviousSchools": [],
  "PreviousYearLevels": []
}
```

##### Result

```text
FirstName = "John",
LastName = "Smith",
MiddleName = "Fred",
DateOfBirth = {1/01/2010 12:00:00 AM},
IsActive = true,
YearLevel = 3,
NextYearLevel = 4,
PreviousYearLevels = [],
CurrentSchool = {},
PreviousSchools = [],
```

#### Test 7: Test for serialization of string to number/bool

##### Setup

```json
{
  "FirstName": "John",
  "LastName": "Smith",
  "IsActive": "true",
  "YearLevel": "3",
  "PreviousYearLevels": [ "1", "2" ],
}
```

##### Result

```text
FirstName = "John",
LastName = "Smith",
MiddleName = null,
DateOfBirth = {1/01/0001 12:00:00 AM},
IsActive = true,
YearLevel = 3,
NextYearLevel = null,
PreviousYearLevels = [ 1, 2 ],
CurrentSchool = null,
PreviousSchools = null
```

#### Test 8: Test for serialization of invalid boolean string

##### Setup

```json
{
  "IsActive": "yes"
}
```

##### Result

```text
Cannot convert string to boolean.
```

#### Test 9: Test for serialization of invalid integer string

##### Setup

```json
{
  "YearLevel": "3.5"
}
```

##### Result

```text
Cannot convert string to integer.
```

#### Test 10: Test for serialization of invalid integer string

##### Setup

```json
{
  "PreviousYearLevels": [ "-1" ]
}
```

##### Result

```text
FirstName = null,
LastName = null,
MiddleName = null,
DateOfBirth = {1/01/0001 12:00:00 AM},
IsActive = false,
YearLevel = null,
NextYearLevel = null,
PreviousYearLevels = [ -1 ],
CurrentSchool = null,
PreviousSchools = null
```